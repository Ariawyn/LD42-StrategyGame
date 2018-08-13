using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour 
{

	public static GameManager instance;

	private InputManager inputManager;
	private AudioManager audioManager;

	private GameObject turnControllerGameObject;
	private TurnController turnControllerInstance;

	private GAME_STATE state;

	private int player1FinalScore;
	private int player2FinalScore;
	private string winnerName;

	private TextMeshProUGUI player1ResultsField;
	private GameObject player1ResultsGameObject;
	private TextMeshProUGUI player2ResultsField;
	private GameObject player2ResultsGameObject;
	private TextMeshProUGUI winnerNameField;
	private GameObject matchResultsGameObject;
	private bool hasInsertedResults = false;



	// Use this for initialization
	void Start() 
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
		this.inputManager = Object.FindObjectOfType<InputManager>();
		this.audioManager = Object.FindObjectOfType<AudioManager>();

		this.state = GAME_STATE.MAIN_MENU;

		this.Menu();
	}
	
	// Update is called once per frame
	void Update()
	{
		// If the game has moved in to the match state, then we need to start the match through the turn controller
		// As such, we need to fetch the turn controller instance from the match scene
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

		if(this.state == GAME_STATE.END_SCORE && this.player1ResultsField == null)
		{
			this.player1ResultsGameObject = GameObject.FindGameObjectWithTag("Player1Score");

			if(this.player1ResultsGameObject)
			{
				this.player1ResultsField = this.player1ResultsGameObject.GetComponent<TextMeshProUGUI>();
			}
		}

		if(this.state == GAME_STATE.END_SCORE && this.player2ResultsField == null)
		{
			this.player2ResultsGameObject = GameObject.FindGameObjectWithTag("Player2Score");

			if(this.player2ResultsGameObject)
			{
				this.player2ResultsField = this.player2ResultsGameObject.GetComponent<TextMeshProUGUI>();
			}
		}

		if(this.state == GAME_STATE.END_SCORE && this.winnerNameField == null)
		{
			this.matchResultsGameObject = GameObject.FindGameObjectWithTag("MatchWinnerResult");

			if(this.matchResultsGameObject)
			{
				this.winnerNameField = this.matchResultsGameObject.GetComponent<TextMeshProUGUI>();
			}
		}

		if(this.state == GAME_STATE.END_SCORE && this.hasInsertedResults == false && this.player1ResultsField != null && this.player2ResultsField != null && this.winnerNameField != null)
		{
			this.player1ResultsField.text = "" + this.player1FinalScore + "";
			this.player2ResultsField.text = "" + this.player2FinalScore + "";
			this.winnerNameField.text = this.winnerName;

			this.hasInsertedResults = true;
		}
	}

	public void Menu()
	{
		this.state = GAME_STATE.MAIN_MENU;

		SceneManager.LoadScene("Menu");

		this.audioManager.Play("MenuTheme");
	}

	// Move into the match scene
	public void StartMatch()
	{
		this.state = GAME_STATE.IN_MATCH;

		SceneManager.LoadScene("Match");

		this.audioManager.Play("MatchTheme");
	}

	public void EndMatchScore(int player1score, int player2score, string name)
	{
		this.player1FinalScore = player1score;
		this.player2FinalScore = player2score;
		this.winnerName = name;

		this.state = GAME_STATE.END_SCORE;

		SceneManager.LoadScene("Results");
	}

	public void Replay()
	{
		// Reset match result variables
		this.hasInsertedResults = false;
		this.player1ResultsField = null;
		this.player2ResultsField = null;
		this.winnerNameField = null;

		this.Menu();
	}
}
