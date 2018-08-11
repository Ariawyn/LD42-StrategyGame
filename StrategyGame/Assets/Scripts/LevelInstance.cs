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
    
    /// <summary>
    /// Instantiates the level.
    /// </summary>
    void InstantiateLevel() {
		tileMap = (GameObject)Instantiate(tileMapPrefab);
		tileMap.transform.SetParent(this.transform);
    }

	/// <summary>
	/// Finds tiles adjacent to the position specified.
	/// </summary>
	/// <param name="position"></param>
	/// <returns>A list of GameTiles which are adjacent(cardinal) to the position</returns>
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

    /// <summary>
    /// Destroys the tile at the position.
    /// </summary>
    /// <param name="pos"></param>
    public void DestroyTileAtPosition(Vector3 pos){
		GameTile t = GetTileAtPosition(pos);
		t.DestroyTile();
    }

    /// <summary>
    /// Don't use this yet.
    /// </summary>
    /// <param name="pos"></param>
    public void PlaceTileAtPosition(Vector3 pos /*tiletype*/){}

    /// <summary>
    /// Gets a tile at the position, or returns null.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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
    

    /// <summary>
    /// Currently not working. Will check if there is a tile at the position
	/// and if not, if adjacent tiles have a balloon on them.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Checks if the tile is empty.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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

	/// <summary>
	/// Not yet implemented.
	/// </summary>
    void DestroyLevel() {}


}
