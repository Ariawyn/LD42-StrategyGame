using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstance : MonoBehaviour {

	public GameObject tileMapPrefab;
	GameObject tileMap;
    //Events
    public GameTile p1StartCell;
    public GameTile p2StartCell;
    GameObject targetCell;
    //Reference to current turn?
    // Camera cam;

	public GameObject bridgePrefab;

	TurnController turnController;
	public Transform interactableTileHolder;
	int interactableTileCount;

	public int numTilesToDecayPerTurn = 3;

	ActionManager am;

	
	


    void Start() {
		am = GetComponent<ActionManager>();
        
        // cam = Camera.main;
		turnController = GameObject.FindGameObjectWithTag("TurnController").GetComponent<TurnController>();
		turnController.AddEndOfTurnEffect(this.Decay);

		interactableTileCount = interactableTileHolder.childCount;
		InstantiateLevel(1);
		InstantiateLevel(2);
		
    }
    // Just for testing
    void Update() {
        // Vector3 mousePos = Input.mousePosition;
        // mousePos = cam.ScreenToWorldPoint(mousePos);
        // GetTileAtPosition(mousePos);
		// if (Input.GetKeyDown(KeyCode.J))
		// 	Decay();
    }
    
    /// <summary>
    /// Instantiates the level.
    /// </summary>
    void InstantiateLevel(int player) {
		// tileMap = (GameObject)Instantiate(tileMapPrefab);
		// tileMap.transform.SetParent(this.transform);
		GameTile g = (player == 1) ? p1StartCell : p2StartCell;
		PlayerController p = turnController.GetSpecificPlayer(player);
		g.PlaceObjectOnThisTile(am.balloonPrefab, p, true);
		List<GameTile> gl = g.CalculateOwnership(p);
		Debug.Log(gl.Count);
		p.AddLandToList(gl);
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
			// Debug.Log(adjacentTiles);
			return adjacentTiles;
		}
		else {
			List<GameTile> adjacentTiles = new List<GameTile>();

		
			RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			hit = Physics2D.Raycast(position, Vector2.right);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			hit = Physics2D.Raycast(position, Vector2.down);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			hit = Physics2D.Raycast(position, Vector2.left);
			if (hit.collider != null) {
				GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
				adjacentTiles.Add(t);
			}
			// Debug.Log(adjacentTiles);
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
    /// Checks if the position is valid for a bridge and places it.
	/// Returns true if it succeeded.
    /// </summary>
    /// <param name="pos"></param>
    public bool PlaceBridgeAtPosition(Vector3 pos){
		// Debug.Log("Register build bridge");
		if (CheckValidBridgePosition(pos) == false)
			return false;
		else{
			// Debug.Log("Valid bridge position check succeeded");
			GameObject newBridge = (GameObject)Instantiate(bridgePrefab);
			newBridge.transform.position = pos;
			newBridge.transform.SetParent(this.transform);
			
			PlayerController p = turnController.GetActivePlayer();
			newBridge.GetComponent<GameTile>().CalculateOwnership(p);

			return true;
		}
	}

    /// <summary>
    /// Gets a tile at the position, or returns null.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameTile GetTileAtPosition(Vector3 pos){
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 0.01f);
		if (hit.collider != null) {
			// Debug.Log(hit.collider.gameObject);
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
    public bool CheckValidBridgePosition(Vector3 pos){
		GameTile t = GetTileAtPosition(pos);
		if (t != null) {
			return false;
		}
		else {
			List<GameTile> tAdj = GetTilesAdjacentToPosition(pos);
			foreach (GameTile gt in tAdj) {
				// Debug.Log(gt.gameObject.name);
				if ((gt.GetOccupyingObjectType() == PlaceableObjectType.BALLOON || gt.GetOccupyingObjectType() == PlaceableObjectType.BRIDGE) && gt.myOwner == turnController.GetActivePlayer()) {
					// Debug.Log("An adjacent tile has a balloon!");
					return true;
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
    public bool CheckValidBalloonPosition(Vector3 pos){
		GameTile t = GetTileAtPosition(pos);
		if (t == null) {
			return false;
		}
		else {
			if (t.GetOccupyingObjectType() == PlaceableObjectType.NULL)
				return true;
			else
				return false;
		}
	}

	void Decay() {
		for (int i = 0; i < numTilesToDecayPerTurn; i++) {
			int rand = Random.Range(0,interactableTileHolder.childCount);
			GameTile gt = interactableTileHolder.GetChild(rand).GetComponent<GameTile>();
			if (gt.myOwner == null) {
				gt.DestroyTile();
			}
			interactableTileCount = interactableTileHolder.childCount;
			// Debug.Log("Tally one deleted");
		}
	}

    //Activate an event.
    void DoEvent(/*event*/){}

	/// <summary>
	/// Not yet implemented.
	/// </summary>
    void DestroyLevel() {
		Destroy(tileMap);
	}


}
