using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private bool isActive;
    private string name;
    public List<GameTile> myLand;
    public GameObject boxSpritePrefab;
    Dictionary<GameTile, GameObject> boxDict;

    public int balloonPoints = 25;
    public int cannonPoints = 10;
    public int bridgePoints = 10;
    public int landPoints = 5;

    int myScore = 0;

    public delegate void OnPointsChanged(int pc, int i);
    public event OnPointsChanged onPointsChanged;

    // Use this for initialization
	void Awake () 
    {
		myLand = new List<GameTile>();
        boxDict = new Dictionary<GameTile,GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Input.GetKeyDown(KeyCode.L)) {
            foreach (GameTile gt in myLand) {
                Debug.Log(name + " owns " + gt.name);
            }
        }
	}

    public void SetActiveState(bool active) 
    {
        this.isActive = active;
    }

    public bool GetActiveState() 
    {
        return this.isActive;
    }

    public string GetName() 
    {
        return this.name;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void AddLandToList(List<GameTile> newLand) {
        foreach (GameTile gt in newLand) {
            if (!myLand.Contains(gt)) {
                myLand.Add(gt);
                PlaceableObjectType pot = gt.GetOccupyingObjectType();
                AddToScore(pot);
                Debug.Log(name + " controls " + gt.name);
                GameObject g = (GameObject)Instantiate(boxSpritePrefab);
                g.transform.position = gt.transform.position;
                boxDict.Add(gt, g);
            }
        }
    }

    public void RemoveFromLandList(List<GameTile> land) {
        foreach (GameTile gt in land) {
            if (myLand.Contains(gt)) {
                myLand.Remove(gt);
                Destroy(boxDict[gt]);
            }
        }
    }

    public void AddToScore(PlaceableObjectType pot) {
        if (pot == PlaceableObjectType.BALLOON) {
            myScore += balloonPoints;
        }
        if (pot == PlaceableObjectType.CANNON) {
            myScore += cannonPoints;
        }
        if (pot == PlaceableObjectType.BRIDGE) {
            Debug.Log("Bridge!");
            myScore += bridgePoints;
        }
        if (pot == PlaceableObjectType.NULL) {
            myScore += landPoints;
        }
        if (this.onPointsChanged != null) {
            this.onPointsChanged((this.GetName() == "First")? 1 : 2, myScore);
        }
    }
}