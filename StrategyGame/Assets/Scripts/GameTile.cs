using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameTile : MonoBehaviour {
	
	public Vector3 myLocation;

	public int turnsToDecay;

	public GameObject occupyingObject;

	public bool test;

	BoxCollider2D myCollider;

	void Awake() {
		myLocation = transform.position;
		if (transform.childCount > 0) {
			occupyingObject = transform.GetChild(0).gameObject;
		}
		myCollider = GetComponent<BoxCollider2D>();
	}

	public void Update() {
		if (test) {
			if (Input.GetKeyDown(KeyCode.F)){
				List<GameTile> l = GetAdjacentTiles();
				foreach (GameTile t in l) {
					Debug.Log(t);
				}
			}
		}
	}

	public PlaceableObjectType GetOccupyingObjectType() {
		if (occupyingObject == null) {
			return PlaceableObjectType.NULL;
		}
		return occupyingObject.GetComponent<PlaceableObject>().myType;
	}

	/// <summary>
	/// Destroys this gameobject.
	/// </summary>
	public void DestroyTile() {
		this.gameObject.SetActive(false);
	}

	/// <summary>
	/// Sets a prefab specified as the child of this object.
	/// </summary>
	/// <param name="prefab"></param>
	public bool PlaceObjectOnThisTile(GameObject prefab) {
		if (occupyingObject != null) {
			Debug.Log("That tile is occupied!");
			return false;
		}
		else {
			GameObject o = (GameObject)Instantiate(prefab);
			o.transform.SetParent(this.transform);
			return true;
		}
	}

	public void RemoveObjectOnThisTile() {
		if (occupyingObject == null) {
			Debug.Log("No occupying object to destroy!");
			return;
		}
		Destroy(this.occupyingObject);
		this.occupyingObject = null;
	}

	/// <summary>
	/// Returns a list of GameTiles adjacent(cardinal) to this tile.
	/// </summary>
	/// <returns></returns>
	public List<GameTile> GetAdjacentTiles() {
		List<GameTile> adjTiles = new List<GameTile>();

		myCollider.enabled = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		hit = Physics2D.Raycast(transform.position, Vector2.right);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		hit = Physics2D.Raycast(transform.position, Vector2.down);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		hit = Physics2D.Raycast(transform.position, Vector2.left);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		myCollider.enabled = true;
		return adjTiles;
	}


}

public enum PlaceableObjectType
{
	NULL,
	BALLOON,
	FACTORY
}
