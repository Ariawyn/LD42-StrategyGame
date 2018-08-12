using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour 
{

    public static TurnController instance;

    [HideInInspector] private bool hasChangedPlayersThisTurn;

    [HideInInspector] private PlayerController player1;
    [HideInInspector] private PlayerController player2;

    [HideInInspector] private string activePlayerName;

	[HideInInspector] private InputManager inputManager;

    [HideInInspector] private int turnCounter;
    [HideInInspector] private int maxAmountOfTurns;


    public delegate void OnTurnEnd();
    public event OnTurnEnd onTurnEnd;
    [HideInInspector] private bool hasEndOTurnEffect;


	// Use this for initialization
	void Start () 
    {
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

        this.player1 = GameObject.FindGameObjectWithTag("Player1Controller").GetComponent<PlayerController>();
        this.player2 = GameObject.FindGameObjectWithTag("Player2Controller").GetComponent<PlayerController>();

        this.player1.SetName("First");
        this.player2.SetName("Second");

        this.hasEndOTurnEffect = false;

        Debug.Log("Player 1 Name: " + this.player1.GetName());
        Debug.Log("Player 2 Name: " + this.player2.GetName());
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(inputManager.GetKeyDown("End_Turn")) 
        {
			this.EndTurn();
		}
	}

    public void StartMatch() 
    {
        this.turnCounter = 0;
        this.maxAmountOfTurns = 40;

        // TODO: Make who starts random
        this.player1.SetActiveState(true);
        this.player2.SetActiveState(false);

        this.activePlayerName = this.player1.GetName();

        this.hasChangedPlayersThisTurn = false;

        Debug.Log("Turn: " + this.turnCounter + " Player: " + this.activePlayerName);
    }

    public void EndTurn() 
    {
        if(this.player1.GetActiveState() == true && !this.hasChangedPlayersThisTurn) 
        {
            //Debug.Log("Player 1 passes the turn to Player 2");

            this.player1.SetActiveState(false);
            this.player2.SetActiveState(true);

            this.activePlayerName = this.player2.GetName();

            this.hasChangedPlayersThisTurn = true;
        }

        if(this.player2.GetActiveState() == true  && !this.hasChangedPlayersThisTurn) 
        {
            //Debug.Log("Player 2 passes the turn to Player 1");

            this.player2.SetActiveState(false);
            this.player1.SetActiveState(true);

            this.activePlayerName = this.player1.GetName();

            this.hasChangedPlayersThisTurn = true;
        }

        this.turnCounter++;

        if(this.turnCounter >= this.maxAmountOfTurns)
        {
            Debug.Log("Match hit maximum number of turns allowed!");
        }
        else
        { 
            this.hasChangedPlayersThisTurn = false;

            Debug.Log("Turn: " + this.turnCounter + " Player: " + this.activePlayerName);
        }

        // Call any functions that have an effect at the end of a turn
        if(this.hasEndOTurnEffect)
        {
            this.onTurnEnd();
        }
    }

    public void AddEndOfTurnEffect(OnTurnEnd onTurnEndEffectFunction)
    {
        this.onTurnEnd += onTurnEndEffectFunction;
        this.hasEndOTurnEffect = true;
    }
}