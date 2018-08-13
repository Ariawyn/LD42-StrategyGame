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
    [HideInInspector] private bool hasEndOTurnEffect = false;

    AudioManager am;
    public ActionUI aui;

    private GameManager gameManager;


	// Use this for initialization
	void Awake () 
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
        this.gameManager = Object.FindObjectOfType<GameManager>();
        am = Object.FindObjectOfType<AudioManager>();

        this.player1 = GameObject.FindGameObjectWithTag("Player1Controller").GetComponent<PlayerController>();
        this.player2 = GameObject.FindGameObjectWithTag("Player2Controller").GetComponent<PlayerController>();

        this.player1.SetName("First");
        this.player2.SetName("Second");

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
        this.maxAmountOfTurns = 30;

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
        am.Play("turnSFX");
        aui.CloseWindow();

        if(this.turnCounter >= this.maxAmountOfTurns)
        {
            Debug.Log("Match hit maximum number of turns allowed!");
            EndMatch();
        }
        else
        { 
            this.hasChangedPlayersThisTurn = false;

            Debug.Log("Turn: " + this.turnCounter + " Player: " + this.activePlayerName);
        }

        // Call any functions that have an effect at the end of a turn
        if(this.hasEndOTurnEffect == true)
        {
            this.onTurnEnd();
        }
    }

    public void AddEndOfTurnEffect(OnTurnEnd onTurnEndEffectFunction)
    {
        this.onTurnEnd += onTurnEndEffectFunction;
        this.hasEndOTurnEffect = true;
        Debug.Log("End of turn effect added!");
    }

    public PlayerController GetActivePlayer()
    {
        if(this.player1.GetActiveState() == true)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }

    public PlayerController GetSpecificPlayer(int num) {
        if (num == 1) {
            return player1;
        }
        else
            return player2;
    }

	public void EndMatch() {
        Debug.Log("Ending match from turn controller!");
		int p1Score = GetSpecificPlayer(1).GetScore();
		int p2Score = GetSpecificPlayer(2).GetScore();

		if (p1Score == p2Score) {
			//DRAW
            this.gameManager.EndMatchScore(p1Score, p2Score, "DRAW");
		}
		else if (p1Score > p2Score) {
			//P1 wins
            this.gameManager.EndMatchScore(p1Score, p2Score, "PLAYER 1");
		}
		else {
			//P2 wins
            this.gameManager.EndMatchScore(p1Score, p2Score, "PLAYER 2");
		}

		//TODO: everything else for the end of the match
	}
}