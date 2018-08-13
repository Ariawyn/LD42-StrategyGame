using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : MonoBehaviour {

	public Text p1Points;
	public Text p2Points;
	public Text turnCounter;

	public int turnNum = 0;

	public TurnController tc;
	public PlayerController p1;
	public PlayerController p2;

	// Use this for initialization
	void Start () {
		tc.AddEndOfTurnEffect(this.ChangeTurnNumber);
		p1.onPointsChanged += ChangePoints;
		p2.onPointsChanged += ChangePoints;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangePoints(int player, int curPoints) {
		if (player == 1) {
			p1Points.text = "Player 1 Points: " + curPoints;
		}
		else {
			p2Points.text = "Player 2 Points: " + curPoints;
		}
	}

	void ChangeTurnNumber() {
		turnNum++;
		turnCounter.text = "Turn: " + turnNum;
	}
}
