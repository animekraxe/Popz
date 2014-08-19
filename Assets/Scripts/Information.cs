using UnityEngine;
using System.Collections;

public class Information : MonoBehaviour {

	public int numHints = 3;
	private Pattern patternManager;
	private GUIStyle style;
	public int size;

	// Use this for initialization
	void Start () {
		patternManager = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		style = new GUIStyle ();
		size = style.fontSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("h") && numHints > 0 && !patternManager.display) {
			patternManager.RevealPattern();
			numHints--;
		}

	}

	void OnGUI () {
		GUI.Label (new Rect(Screen.width - 100, Screen.height - 70, 100, 20), "Rounds: " + patternManager.numRounds.ToString());
		GUI.Label (new Rect(Screen.width - 100, Screen.height - 50, 100, 20), "Lives: " + patternManager.numLives.ToString());
		GUI.Label (new Rect (Screen.width - 100, Screen.height - 30, 100, 30), "Hints: " + numHints.ToString());
	}
}
