using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlData
{
	public ControlKey[] keys;
}

[System.Serializable]
public class ControlKey
{
	public ControlKey(string name, string value)
	{
		this.keyName = name;
		this.keyValue = value;
	}

	public string keyName;
	public string keyValue;
}