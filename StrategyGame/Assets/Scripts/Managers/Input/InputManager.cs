using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	// The control scheme dictionary
	Dictionary<string, KeyCode> controls;

	// The default axis of movement input
	public InputAxis horizontal;
	public InputAxis vertical;

	// Save file containing control data, located in Assets/StreamingAssets
	private string filename = "controls.json";

	void Awake()
	{
		// Check if the instance of the input manager has already been set and respond according to singleton format
		if(instance != null)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	// Initialization
	void Start () 
	{
		this.initializeControls();
	}

	private void initializeControls() 
	{
		// Attempt to load from saved preferences file
		this.controls = this.loadControlLayout();

		// If we return with an empty control layout, then we add instead the default values
		if(this.controls.Count == 0) 
		{
			Debug.Log("No control scheme (or keys) was found saved, so we are defaulting to the default control scheme");

			// Default movement controls
			this.controls["Forward"] = KeyCode.W;
			this.controls["Backward"] = KeyCode.S;
			this.controls["Left"] = KeyCode.A;
			this.controls["Right"] = KeyCode.D;

			// Alternative movement controls
			this.controls["alt_Forward"] = KeyCode.UpArrow;
			this.controls["alt_Backward"] = KeyCode.DownArrow;
			this.controls["alt_Left"] = KeyCode.LeftArrow;
			this.controls["alt_Right"] = KeyCode.RightArrow;

			// Interaction controls
			this.controls["Interact"] = KeyCode.F;

			// Save the default controls
			this.saveKeyLayout();
		}

		// TODO: Initialize movement input axis
		this.horizontal = new InputAxis(this.controls["Right"], this.controls["Left"], this.controls["alt_Right"], this.controls["alt_Left"]);
		this.vertical = new InputAxis(this.controls["Forward"], this.controls["Backward"], this.controls["alt_Forward"], this.controls["alt_Backward"]);
	}

	public bool GetKey(string key) 
	{
		// Check if the key exists
		if(this.controls.ContainsKey(key)) 
		{
			// Then we do have the key in the control layout
			return Input.GetKey(this.controls[key]);
		}

		// We do not have the key registered in our layout
		return false;
	}


	public bool GetKeyDown(string key) 
	{
		// Check if the key exists
		if(this.controls.ContainsKey(key)) 
		{
			// Then we do have the key in the control layout
			return Input.GetKeyDown(this.controls[key]);
		}

		// We do not have the key registered in our layout
		return false;
	}

	public string[] getControlKeyNames()
	{
		return this.controls.Keys.ToArray();
	}

	public string getKeyCodeString(string name)
	{
		if(this.controls.ContainsKey(name))
		{
			return this.controls[name].ToString();
		}
		else
		{
			return "N/A";
		}
	}

	public void setKeyCodeForKey(string name, KeyCode code)
	{
		this.controls[name] = code;
		this.saveKeyLayout();
	}

	private Dictionary<string, KeyCode> loadControlLayout() 
	{
		Debug.Log("Loading Keys");

		// Set up controls data structure, ie. a dictionary
		Dictionary<string, KeyCode> loadedControls = new Dictionary<string, KeyCode>();

		// Get path of the controls.json file
		string filepath = Path.Combine(Application.streamingAssetsPath, this.filename);

		if(File.Exists(filepath))
		{
			// Read the data
			string jsonData = File.ReadAllText(filepath);
			ControlData loadedControlData = JsonUtility.FromJson<ControlData>(jsonData);

			// Loop through and add loaded key data into dictionary
			for(int i = 0; i < loadedControlData.keys.Length; i++)
			{
				// Add into control dict, excuse the long enum parse thing
				KeyCode keyValue = (KeyCode)System.Enum.Parse(typeof(KeyCode), loadedControlData.keys[i].keyValue);
				loadedControls.Add(loadedControlData.keys[i].keyName, keyValue);
			}
		}

		// If we did not load the data, this.controls is empty, in which case the default key set will be loaded
		return loadedControls;

	} 

	private void saveKeyLayout() 
	{
		Debug.Log("Saving keys");

		// Get path of the controls.json file
		string filepath = Path.Combine(Application.streamingAssetsPath, this.filename);
		Debug.Log(filepath);

		// Create ControlData from our control dictionary
		ControlData dataToSave = new ControlData();
		dataToSave.keys = new ControlKey[this.controls.Count];

		// Iterator to add into dataToSave array
		int i = 0;

		// Loop through dict (this is annoying, but im unsure how to improve it yet) FIXME: Maybe have json load directly
		// into dict instead of going and using ControlData class, ... whatever works though
		foreach(var key in this.controls)
		{
			dataToSave.keys[i] = new ControlKey(key.Key, key.Value.ToString());
			i++;
		}

		// Convert ControlData to json
		string jsonControlData = JsonUtility.ToJson(dataToSave, true);

		// Write or overwrite controls.json
		File.WriteAllText(filepath, jsonControlData);
	}
}
