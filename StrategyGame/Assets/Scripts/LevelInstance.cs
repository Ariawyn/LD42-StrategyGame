using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstance : MonoBehaviour {

	public GameObject tileMapPrefab;
	GameObject tileMap;
    //Events
    GameObject p1StartCell;
    GameObject p2StartCell;
    GameObject targetCell;
    //Reference to current turn?
    Camera cam;


    void Start() {
        InstantiateLevel();
        cam = Camera.main;
    }
    // Just for testing
    void Update() {
        // Vector3 mousePos = Input.mousePosition;
        // mousePos = cam.ScreenToWorldPoint(mousePos);
        // GetTileAtPosition(mousePos);
    }
    
    //Check if a tilemap exists and if so, destroy it.
    // Then, instantiate the new tilemap, set the start cells, and set the target cell.
    // Check the list of events and tell the game manager?
    void InstantiateLevel() {
		tileMap = (GameObject)Instantiate(tileMapPrefab);
		tileMap.transform.SetParent(this.transform);
    }

	public List<GameTile> GetTilesAdjacentToPosition(Vector3 position) {
		GameTile ti = GetTileAtPosition(position);
		if (ti != null) {
			List<GameTile> adjacentTiles = ti.GetAdjacentTiles();
			Debug.Log(adjacentTiles);
			return adjacentTiles;
		}
		else {
			List<GameTile> adjacentTiles = new List<GameTile>();

		
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			hit = Physics2D.Raycast(transform.position, Vector2.right);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			hit = Physics2D.Raycast(transform.position, Vector2.down);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			hit = Physics2D.Raycast(transform.position, Vector2.left);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			Debug.Log(adjacentTiles);
			return adjacentTiles;
		}
		
		
	}

    //Decay a tile at a certain position. Make sure that the tile is
    // not occupied by a balloon
    public void DestroyTileAtPosition(Vector3 pos){
		GameTile t = GetTileAtPosition(pos);
		t.DestroyTile();
    }

    //Create a tile at a certain position. Make sure it is valid.
    public void PlaceTileAtPosition(Vector3 pos /*tiletype*/){}

    // Just get the tilemap's tile.
    // TODO: Floor to int?
    public GameTile GetTileAtPosition(Vector3 pos){
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 0.01f);
		if (hit.collider != null) {
			Debug.Log(hit.collider.gameObject);
			return hit.collider.GetComponent<GameTile>();
		}
		else {
			return null;
		}
	}
    

    //If the bridge is being placed next to land, then it is an end bridge,
    // and it the land tile should contain a balloon.
    // If not, then it should be next to another bridge tile.
    bool CheckValidBridgePosition(Vector3 pos){
		GameTile t = GetTileAtPosition(pos);
		if (t != null) {
			return false;
		}
		else {
			List<GameTile> tAdj = t.GetAdjacentTiles();
			foreach (GameTile gt in tAdj) {
				if (gt.occupyingObject != null) {
					return true;
				}
				else {
					continue;
				}
			}
			return false;
		}
	}

    //Make sure that the tile is not occupied by a balloon or a bridge.
    bool CheckValidBalloonPosition(Vector3 pos){
		GameTile t = GetTileAtPosition(pos);
		if (t == null) {
			return false;
		}
		else {
			if (t.occupyingObject == null)
				return true;
			else
				return false;
		}
	}

    //Activate an event.
    void DoEvent(/*event*/){}

    //Destroy everything in this class and tell the game manager to bring in
    // the next level.
    void DestroyLevel() {}


}
