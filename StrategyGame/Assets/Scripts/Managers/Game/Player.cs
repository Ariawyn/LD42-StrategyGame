using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public string name;
    public int currency;

    public GameTile[] ownedTiles;
    public PlaceableObject ownedObjects;
    
    void Start () {


		// Grab inputmanager
		inputManager = Object.FindObjectOfType<InputManager>();

		// Setup the players
		this.player1 = "A1";
		this.player2 = "A2";

		
		// Setup the turn system
		this.currentTurn = 0;
		this.currentActivePlayer = player1;

		Debug.Log("Current Turn: " + this.currentTurn + " Player: " + this.currentActivePlayer);

	}
	
	// Update is called once per frame
	void Update () {
		if(inputManager.GetKeyDown("EndTurn")) {
			this.PassTurn();
		}
	}

}
