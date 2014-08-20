using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static int level = 1;
	private bool runTimer;
	private float levelTimeLimit = 30f;
//	private bool penalty;
	private int life;

	// Use this for initialization
	void Start () {
		runTimer = true;
//		penalty = false;
		life = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (runTimer) {
			levelTimeLimit -= Time.deltaTime;
			checkState();
		}
		else 
			Application.LoadLevel(0);
	}

	void OnGUI () {
		GUI.Box(new Rect(10, 10, 50, 20), "" + levelTimeLimit.ToString("0"));
		GUI.Box(new Rect(Screen.width - 60, 10, 50, 20), "" + life.ToString("0"));
	}

	void checkState () {
		if (levelTimeLimit <= 1 || life < 1)
			runTimer = false;
	}

	public void setPenalty () {
		life--;
	}
}
