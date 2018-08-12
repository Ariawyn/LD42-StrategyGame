using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTileFunctions : MonoBehaviour {

	public LevelInstance l;
	Camera cam;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		// Vector3 pos = Input.mousePosition;
		// Vector3 worldPos = cam.ScreenToWorldPoint(pos);
		// if (Input.GetMouseButtonDown(0)) {
		// 	l.GetTileAtPosition(worldPos);
		// }
		// if (Input.GetKeyDown(KeyCode.Space)) {
		// 	l.GetTilesAdjacentToPosition(worldPos);
		// }
		// if (Input.GetKeyDown(KeyCode.D)) {
		// 	l.DestroyTileAtPosition(worldPos);
		// }
	}
}
