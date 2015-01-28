using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiObjGameManager : MonoBehaviour {
	
	public int level;
	public int stage;
	public int numDistractors;

	public GameObject field;
	public GameObject creaturePrefab;
	public GameObject player;

	private List<Color> colorSet;

	// Game State Variables
	private bool gameRunning;

	// Use this for initialization
	void Start () {
		startLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameRunning) {
			checkGameEnd ();
		} else {
			//Debug.Log ("GAME OVER");
			cleanupLevel();
			startLevel ();
		}
	}

	void startCreatures () {
		FieldInfo info = Util.getFieldInfo(field);
		for (int i = 0; i < level + numDistractors; ++i) {
			// Universal parameters for all creatures
			var spawnPosition = new Vector3(Random.Range (info.lowerX + (info.width / 2.0f), info.upperX),
			                                Random.Range (info.lowerY, info.upperY),
			                                0);
			var creature = Instantiate(creaturePrefab) as GameObject;
			creature.transform.position = spawnPosition;
			creature.GetComponent<Movement>().field = field;
			creature.GetComponent<CloakControl>().player = player.GetComponentInChildren<Player>();
			creature.GetComponent<CloakControl>().setColorSet(colorSet);

			// Distractor parameters only
			if (i >= level) {
				creature.GetComponent<CloakControl>().isDistractor = true;
			}
		}
	}

	void startPlayer () {
		player.GetComponentInChildren<Player> ().setCollectors (colorSet);
	}

	void startLevel () {
		if (stage > level) {
			++level;
			stage = 1;
		}
		
		if (stage <= 0) {
			stage = 1;
		}

		colorSet = Util.genColorSet (stage);
		startPlayer ();
		startCreatures ();
		gameRunning = true;
	}

	void cleanupLevel () {
		var creatures = FindObjectsOfType<CloakControl> ();
		for (int i = 0; i < creatures.Length; ++i)
			Destroy (creatures [i].gameObject);

		player.GetComponentInChildren<Player> ().numCloakedObtained = 0;
		
	}

	void checkGameEnd () {
		var numCloakedObtained = player.GetComponentInChildren<Player> ().NumCloakedObtained ();
		if (level == numCloakedObtained) {
			gameRunning = false;
			++stage;
		}
	}
}
