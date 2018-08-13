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

	bool selectingFireTarget = false;

	PlayerController activePlayer;
	TurnController tc;

	/* We need to choose a tile, activate an option menu, register an option, tell the tile/manager
	what to do, subtract the action points for this turn, register when the action points are zero,
	 and then tell the gm that the turn is over. */

	void Awake() {
		cam = Camera.main;
		OnTurnStart();
		tc = GameObject.FindGameObjectWithTag("TurnController").GetComponent<TurnController>();
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
		if (selectedTile.PlaceObjectOnThisTile(balloonPrefab, tc.GetActivePlayer()) == false ) {
			// Don't do the stuff
		}
		else {
			PlayerController p = tc.GetActivePlayer();
			List<GameTile> newLand = selectedTile.CalculateOwnership(p);
			p.AddLandToList(newLand);
			if (p.GetName() == "First") {
				selectedTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
			}
			else {
				selectedTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
			}
			SubtractAP(apToBuildBalloon);
		}
	}

	public void RegisterBuildCannon() {
		if (actionPointsThisTurn - apToBuildCannon < 0) {
			Debug.Log("Not enough AP!");
			return;
		}
		if (selectedTile.PlaceObjectOnThisTile(cannonPrefab, tc.GetActivePlayer()) == false) {
			//Don't do the stuff
		}
		else {
			PlayerController p = tc.GetActivePlayer();
			List<GameTile> tempList = selectedTile.CalculateOwnership(p, true);
			p.AddLandToList(tempList);
			if (p.GetName() == "First") {
				selectedTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
			}
			else {
				selectedTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
			}
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
			List<GameTile> tempList = new List<GameTile>();
			GameTile gt = li.GetTileAtPosition(selectedPosition);
			// Debug.Log(gt.GetOccupyingObjectType());
			PlayerController p = tc.GetActivePlayer();
			tempList = gt.CalculateOwnership(p, true);
			// tempList.Add(selectedTile);
			p.AddLandToList(tempList);
			SubtractAP(apToBuildBridge);
		}
		else {
			//Don't do the stuff.
			Debug.Log("Bridge building failed due to some issue with position.");
		}
	}

	public void RegisterFireAction() {
		Debug.Log("Fire action registered");
		if (actionPointsThisTurn - apToFireCannon < 0 ) {
			Debug.Log("Not enough AP!");
			return;
		}
		GameTile targetTile = SelectTile(selectedPosition);
		PlaceableObjectType t = targetTile.GetOccupyingObjectType();
		if (t == PlaceableObjectType.BALLOON) {
			Debug.Log("That's a valid thing to destroy");
			targetTile.RemoveObjectOnThisTile();
			PlayerController p = targetTile.myOwner;
			List<GameTile> toRemove = targetTile.RevokeOwnership(tc.GetActivePlayer());
			
			p.RemoveFromLandList(toRemove);
			SubtractAP(apToFireCannon);
		}
		else {
			Debug.Log("That's not a balloon");
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
		mousePosWorld = ToTileCoords(mousePosWorld);

		if (Input.GetMouseButtonDown(0)) {
			if (eventSystem.IsPointerOverGameObject()){
				//nothing
			}
			else {
				selectedTile = SelectTile(mousePosWorld);
				selectedPosition = mousePosWorld;
				if (!selectingFireTarget){
					CheckValidBuildOptions();
					ui.OpenMenu(selectedPosition);
				}
			}
		}
	}

	IEnumerator SelectTargetToFire() {
		// Debug.Log("Coroutine started");
		selectingFireTarget = true;
		while(selectingFireTarget){
			if (Input.GetMouseButtonDown(0)) {
				RegisterFireAction();
				selectingFireTarget = false;
				
			}
			yield return null;
		}
	}

	public void StartSelectFireTarget() {
		// Debug.Log("Starting Coroutine");
		StartCoroutine(SelectTargetToFire());
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
