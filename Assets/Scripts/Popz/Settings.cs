using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	static public bool togglePattern = true;
	static public bool toggleNback = true;
	static public bool toggleMultiObj = true;
	static public bool isSet = false;

	void Awake () {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
