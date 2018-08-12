using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private bool isActive;
    private string name;

    // Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
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
}