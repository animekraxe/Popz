using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	static public bool togglePattern = true;
	static public bool toggleNback = true;
	static public bool toggleMultiObj = true;
	static public bool togglePlatformsNback = false;
	static public bool isSet = false;
	static public int nbackNavigationDifficulty = 3;

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
