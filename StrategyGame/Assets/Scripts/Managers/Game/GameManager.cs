using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private InputManager inputManager;

	[HideInInspector] private string player1;
	[HideInInspector] private string player2;

	[HideInInspector] private int actionsPerTurn = 3;
	[HideInInspector] private int currentTurn;
	[HideInInspector] private int maximumAmountOfTurns = 30;
	[HideInInspector] private string currentActivePlayer;


	// Use this for initialization
	void Start () {

		// Check if the instance of the input manager has already been set and respond according to singleton format
		if(instance != null)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}

		// Grab inputmanager
		inputManager = Object.FindObjectOfType<InputManager>();


		this.StartMatch();
	}
	
	// Update is called once per frame
	void Update () {
		if(inputManager.GetKeyDown("EndTurn")) {
			this.PassTurn();
		}
	}

	void StartMatch()
	{
		// Setup the players
		this.player1 = "A1";
		this.player2 = "A2";

		
		// Setup the turn system
		this.currentTurn = 0;
		this.currentActivePlayer = player1;

		Debug.Log("Current Turn: " + this.currentTurn + " Player: " + this.currentActivePlayer);
	}

	void PassTurn() {
		// Increment the turn counter
		// TODO: Should this be like turn 1 player 1 then turn 1 player 2, or turn 1 player 1 then turn 2 player 2???
		this.currentTurn++;
		this.changeActivePlayer();

		Debug.Log("Current Turn: " + this.currentTurn + " Player: " + this.currentActivePlayer);
	}

	void changeActivePlayer() {
		// TODO: Figure out a better way to handle players
		if(this.currentActivePlayer == player1) {
			this.currentActivePlayer = player2;
		
		} else {
			this.currentActivePlayer = player1;
		}
	}
}
