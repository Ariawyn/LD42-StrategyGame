using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour {

	public Button balloonButton;
	public Button cannonBuildButton;
	public Button bridgeButton;
	public Button fireButton;

	public bool canBalloon;
	public bool canCannon;
	public bool canBridge;
	public bool canFire;

	Camera cam;

	void Awake() {
		SetActiveButtons();
		SetSelfActive(false);
		cam = Camera.main;
	}

	void SetActiveButtons() {
		balloonButton.gameObject.SetActive(canBalloon);
		cannonBuildButton.gameObject.SetActive(canCannon);
		bridgeButton.gameObject.SetActive(canBridge);
		fireButton.gameObject.SetActive(canFire);

	}

	void SetSelfActive(bool active) {
		this.gameObject.SetActive(active);
	}

	public void OpenMenu(Vector3 position) {
		Vector3 p = cam.WorldToScreenPoint(position);
		this.transform.position = new Vector3(p.x, p.y+30,p.z);
		SetSelfActive((canBalloon || canBridge || canCannon || canFire));
		SetActiveButtons();
	}

	public void CloseWindow() {
		SetSelfActive(false);
		// SetActiveButtons();
	}
}
