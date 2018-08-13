using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private InputManager inputManager;
	private TurnController turnController;
	private PlayerController activePlayer;

	private Camera cam;

	[HideInInspector] private float speed = 0.4f;
	[HideInInspector] private float smoothSpeed = 0.125f;
	
	[HideInInspector] private Transform movementTarget;
	[HideInInspector] private bool isUsingTarget = false;

	// Use this for initialization
	void Start () 
	{
		// Grab camera
		this.cam = Camera.main;

		// Grab inputmanager
		this.inputManager = Object.FindObjectOfType<InputManager>();

		// Grab turncontroller
		this.turnController = GameObject.FindGameObjectWithTag("TurnController").GetComponent<TurnController>();
	
		// Setup the move to target player end of turn callback
		this.turnController.AddEndOfTurnEffect(this.MoveToPlayer);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Debug.Log("Camera does not allow free movement: " + this.isUsingTarget);
		if(this.isUsingTarget == false)
		{
			// Apply movement
			this.cam.transform.Translate(new Vector3(this.inputManager.horizontal.GetInputRaw() * this.speed, this.inputManager.vertical.GetInputRaw() * this.speed, 0));
		}
		else
		{
			// Get smoothed lerp for camera and use it
			Vector3 smoothedTargetPosition = Vector3.Lerp(this.cam.transform.position, this.movementTarget.position + new Vector3(0, 0, -10), this.smoothSpeed);
			this.cam.transform.position = smoothedTargetPosition;

			// Round positions to integers for checking
			int xPosition = Mathf.RoundToInt(this.cam.transform.position.x);
			int yPosition = Mathf.RoundToInt(this.cam.transform.position.y);

			bool hasMadeItToTarget = (xPosition == this.movementTarget.position.x) && (yPosition == this.movementTarget.position.y);
			Debug.Log("Camera has made it to the desired position: " + hasMadeItToTarget);
			// If the camera has made it to the desired position, we reset the camera controls
			if(hasMadeItToTarget)
			{
				// Reset
				this.isUsingTarget = false;

				// Rotate 180 degrees so that the active player has a better view now that they have made it to the target
				// this.cam.transform.Rotate(0, 0, 180);
			}
		}
	}

	// Move to player called at the end of every turn to move the camera to the active players area
	void MoveToPlayer() 
	{
		this.isUsingTarget = true;
		this.activePlayer = this.turnController.GetActivePlayer();

		Debug.Log(this.activePlayer.name);

		this.movementTarget = this.activePlayer.myLand[0].transform;
		Debug.Log(this.movementTarget.position);
	}
}
