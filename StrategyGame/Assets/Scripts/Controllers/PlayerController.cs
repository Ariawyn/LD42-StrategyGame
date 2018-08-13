using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private bool isActive;
    private string name;
    List<GameTile> myLand;
    public GameObject boxSpritePrefab;
    Dictionary<GameTile, GameObject> boxDict;

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
}