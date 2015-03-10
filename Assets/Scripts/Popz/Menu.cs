using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		float screenScale = Screen.width / 480.0f;
		Matrix4x4 scaledMatrix = Matrix4x4.Scale(new Vector3(screenScale,screenScale,screenScale));
		GUI.matrix = scaledMatrix;

		GUI.skin.toggle.fixedWidth = 200;

		var patternLabel = "Enable Pattern Game";
		var nbackLabel = "Enable Nback Game";
		var multiObjLabel = "Enable Multi Object Game";

		float width = 200;
		float height = 30;
		float startY = height;

		Settings.togglePattern = GUI.Toggle (new Rect (10, startY, width, height), 
		                                     Settings.togglePattern, patternLabel);
		Settings.toggleNback = GUI.Toggle (new Rect (10, startY + height, width, height), 
		                                   Settings.toggleNback, nbackLabel);
		Settings.toggleMultiObj = GUI.Toggle (new Rect (10, startY + (height * 2), width, height), 
		                                      Settings.toggleMultiObj, multiObjLabel);

		if (GUI.Button (new Rect (10, startY + (height * 3), width, height), "Play Game")) {
			Settings.isSet = true;
			Application.LoadLevel("Popz");
		}
	}
}