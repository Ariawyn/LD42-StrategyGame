﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameTile : MonoBehaviour {
	
	public Vector3 myLocation;

	public int turnsToDecay;

	public GameObject occupyingObject;

	public bool test;

	BoxCollider2D myCollider;

	public bool isBridge;

	public Sprite p1Balloon;
	public Sprite p1Cannon;
	public Sprite p2Balloon;
	public Sprite p2Cannon;
	
	public PlayerController myOwner;

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
		Destroy(this.gameObject);
	}

	/// <summary>
	/// Sets a prefab specified as the child of this object.
	/// </summary>
	/// <param name="prefab"></param>
	public bool PlaceObjectOnThisTile(GameObject prefab) {
		if (isBridge){
			Debug.Log("I'm a bridge, don't put anything on me");
			return false;
		}
		Debug.Log("I was called");
		if (occupyingObject != null) {
			Debug.Log("That tile is occupied!");
			return false;
		}
		else {
			GameObject o = (GameObject)Instantiate(prefab);
			o.transform.SetParent(this.transform);
			o.transform.localPosition = new Vector3(0,0,1);
			occupyingObject = o;
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1.3f);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		hit = Physics2D.Raycast(transform.position, Vector2.right, 1.3f);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		hit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		hit = Physics2D.Raycast(transform.position, Vector2.left, 1.3f);
		if (hit.collider != null) {
			GameTile t = hit.collider.gameObject.GetComponent<GameTile>();
			adjTiles.Add(t);
		}
		myCollider.enabled = true;
		return adjTiles;
	}

	public List<GameTile> CalculateOwnership(PlayerController newOwner) {
		
		List<GameTile> myAdj = GetAdjacentTiles();
		List<GameTile> newOwnerList = new List<GameTile>();

		foreach (GameTile gt in myAdj) {
			if (gt.myOwner == null) {
				gt.myOwner = newOwner;
				Debug.Log("My name is " + gt + ". My owner is " + gt.myOwner);
				newOwnerList.Add(gt);
			}
		}
		if (this.myOwner == null){
			this.myOwner = newOwner;
			newOwnerList.Add(this);
		}
		return newOwnerList;
		
	}

	public List<GameTile> RevokeOwnership() {
		List<GameTile> toRemove = new List<GameTile>();
		List<GameTile> myAdj = this.GetAdjacentTiles();
		myAdj.Add(this);
		// int c0 = 0;
		foreach (GameTile gt in myAdj) {
			List<GameTile> gtAdj = gt.GetAdjacentTiles();
			int c = 0;
			foreach (GameTile gt2 in gtAdj) {
				if (gt2.myOwner == gt.myOwner && !myAdj.Contains(gt2)){
					c++;
				}
			}
			if (c <= 0) {
				gt.myOwner = null;
				toRemove.Add(gt);
			}
		}

		return toRemove;
	}


}

public enum PlaceableObjectType
{
	NULL,
	BALLOON,
	CANNON,
	BRIDGE,
	FACTORY
}
