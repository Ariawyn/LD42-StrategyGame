using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	public static GameManager instance;

	private InputManager inputManager;

	private GameObject turnControllerGameObject;
	private TurnController turnControllerInstance;

	private GAME_STATE state;

	[HideInInspector] private string player1;
	[HideInInspector] private string player2;

	[HideInInspector] private int actionsPerTurn = 3;
	[HideInInspector] private int currentTurn;
	[HideInInspector] private int maximumAmountOfTurns = 30;
	[HideInInspector] private string currentActivePlayer;


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

		this.state = GAME_STATE.MAIN_MENU;

		this.StartMatch();
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(this.state == GAME_STATE.IN_MATCH && this.turnControllerInstance == null) 
		{
			this.turnControllerGameObject = GameObject.FindGameObjectWithTag("TurnController");

			if(this.turnControllerGameObject)
			{
				this.turnControllerInstance = this.turnControllerGameObject.GetComponent<TurnController>();
			}

			if(this.turnControllerInstance) 
			{
				this.turnControllerInstance.StartMatch();
			}
		}
	}

	void StartMatch()
	{
		this.state = GAME_STATE.IN_MATCH;
		
		// TODO: Change scene into match scene
	}

	public GAME_STATE GetState()
	{
		return this.state;
	}
}
