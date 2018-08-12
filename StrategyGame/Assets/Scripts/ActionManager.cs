using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionManager : MonoBehaviour {

	public GameManager gm;
	public LevelInstance li;
	Camera cam;

	GameTile selectedTile;
	Vector3 selectedPosition;
	

	public int baseActionPoints = 4;
	int actionPointsThisTurn = 4;

	public int apToBuildBridge = 4;
	public int apToBuildBalloon = 2;
	public int apToBuildCannon = 4;
	public int apToFireCannon = 2;

	public ActionUI ui;

	public GameObject cannonPrefab;
	public GameObject balloonPrefab;

	public EventSystem eventSystem;

	/* We need to choose a tile, activate an option menu, register an option, tell the tile/manager
	what to do, subtract the action points for this turn, register when the action points are zero,
	 and then tell the gm that the turn is over. */

	void Awake() {
		cam = Camera.main;
		OnTurnStart();
	}

	void OnTurnStart() {
		actionPointsThisTurn = baseActionPoints;
		selectedTile = null;
	}

	void EndTurn() {
		//Pass turn
	}

	GameTile SelectTile(Vector3 pos) {
		return li.GetTileAtPosition(pos);
	}

	public void RegisterBuildBalloon() {
		Debug.Log("Register build balloon and selected tile is " + selectedTile);
		if (actionPointsThisTurn - apToBuildBalloon <0){
			Debug.Log("Not enough AP");
			return;
		}
		if (selectedTile.PlaceObjectOnThisTile(balloonPrefab) == false) {
			// Don't do the stuff
		}
		else {
			SubtractAP(apToBuildBalloon);
		}
	}

	public void RegisterBuildCannon() {
		if (actionPointsThisTurn - apToBuildCannon < 0) {
			Debug.Log("Not enough AP!");
			return;
		}
		if (selectedTile.PlaceObjectOnThisTile(cannonPrefab) == false) {
			//Don't do the stuff
		}
		else {
			SubtractAP(apToBuildCannon);
		}
	}

	public void RegisterBuildBridge() {
		
		if (actionPointsThisTurn - apToBuildBridge < 0) {
			Debug.Log("Not enough AP");
			return;
		}
		bool passed = li.PlaceBridgeAtPosition(ToTileCoords(selectedPosition));
		if (passed) {
			SubtractAP(apToBuildBridge);
		}
		else {
			//Don't do the stuff.
			Debug.Log("Bridge building failed due to some issue with position.");
		}
	}

	public void RegisterFireAction(Vector3 target) {
		if (actionPointsThisTurn - apToFireCannon < 0 ) {
			Debug.Log("Not enough AP!");
			return;
		}
		GameTile targetTile = SelectTile(target);
		PlaceableObjectType t = targetTile.GetOccupyingObjectType();
		if (t == PlaceableObjectType.BALLOON) {
			targetTile.RemoveObjectOnThisTile();
		}
	}

	bool SubtractAP(int toSub) {
		if (actionPointsThisTurn - toSub >= 0){
			actionPointsThisTurn -= toSub;
			if (actionPointsThisTurn <= 0) {
				EndTurn();
			}
			return true;
		}
		return false;
	}

	void Update() {
		Vector3 mousePos = Input.mousePosition;
		Vector3 mousePosWorld = cam.ScreenToWorldPoint(mousePos);

		if (Input.GetMouseButtonDown(0)) {
			if (eventSystem.IsPointerOverGameObject()){
				//nothing
			}
			else {
				selectedTile = SelectTile(mousePosWorld);
				selectedPosition = ToTileCoords(mousePosWorld);
				CheckValidBuildOptions();
				ui.OpenMenu(selectedPosition);
			}
		}
	}

	void CheckValidBuildOptions() {
		ui.canBalloon = li.CheckValidBalloonPosition(selectedPosition);
		ui.canCannon = ui.canBalloon;
		ui.canBridge = li.CheckValidBridgePosition(selectedPosition);
		ui.canFire = (selectedTile == null) ? false : selectedTile.GetOccupyingObjectType() == PlaceableObjectType.CANNON;
		
	}

	Vector3 ToTileCoords(Vector3 pt) {
		Vector3 newPos = new Vector3(Mathf.RoundToInt(pt.x), Mathf.RoundToInt(pt.y), 0);
		return newPos;
	}

	
}
