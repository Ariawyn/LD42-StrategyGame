using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[HideInInspector] private Player player1;
	[HideInInspector] private Player player2;

	[HideInInspector] private int actionsPerTurn = 3;
	[HideInInspector] private int currentTurn;
	[HideInInspector] private int currentActivePlayer;


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

		this.currentTurn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
