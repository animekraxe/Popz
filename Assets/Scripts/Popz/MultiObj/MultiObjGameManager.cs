﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiObjGameManager : MonoBehaviour {
	/*
	public int level;
	private float result = 0f;
	private string colorCreature = "";

	public GameObject creaturePrefab;
	public GameObject field;

	//Game State Variables
	public float pushSpeed = 6.5f;

	void Start() {
		for (int i = 0; i < level+4; i++) {
			result = Random.Range (1, 3);
			print (result);			//test
			Instantiate(creaturePrefab);
		}
	}
}
	*/
	public int level;
	public int stage;
	//public int numDistractors;

	public GameObject field;
	public GameObject creaturePrefab;
	public GameObject player;

	private List<Color> colorSet;

	// Game State Variables
	private bool gameRunning = false;
	public float pushSpeed = 6.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (gameRunning) {
			checkGameEnd ();
		}
	}

	void startCreatures () {
		FieldInfo info = Util.getFieldInfo(field);
		for (int i = 0; i < level ; ++i) {
			// Universal parameters for all creatures
			var spawnPosition = new Vector3(Random.Range (info.lowerX + 1, info.upperX + 7),
			                                Random.Range (info.lowerY + 5, info.upperY - 1),
			                                0);

			var creature = Instantiate(creaturePrefab) as GameObject;
			creature.transform.position = spawnPosition;
			creature.GetComponent<Movement>().field = field;
			creature.GetComponent<Movement>().pushSpeed = pushSpeed;
			creature.GetComponent<CloakControl>().player = player.GetComponentInChildren<MultiObjPlayer>();
			creature.GetComponent<CloakControl>().setColorSet(colorSet);
			creature.GetComponent<Selection>().player = player.GetComponentInChildren<MultiObjPlayer>();

			// Distractor parameters only
			if (i >= level) {
				creature.GetComponent<CloakControl>().is_distractor = true;
			}
		}
	}

/*
	void startPlayer () {
		player.GetComponentInChildren<MultiObjPlayer> ().setCollectors (colorSet);
	}
*/
	public void startLevel () {
		if (stage > level) {
			++level;
			stage = 1;
		}
		
		if (stage <= 0) {
			stage = 1;
		}

		colorSet = Util.genColorSet (stage);
//		startPlayer ();
		startCreatures ();
		gameRunning = true;
	}

	void cleanupLevel () {
		gameRunning = false;
		var creatures = FindObjectsOfType<CloakControl> ();
		for (int i = 0; i < creatures.Length; ++i)
			Destroy (creatures [i].gameObject);
		player.GetComponentInChildren<MultiObjPlayer> ().numCloakedObtained = 0;
	}

	void restartLevel() {
		++stage;
		startLevel ();
	}

	void checkGameEnd () {
		var numCloakedObtained = player.GetComponentInChildren<MultiObjPlayer> ().NumCloakedObtained ();
		if (level == numCloakedObtained) {
			cleanupLevel();
			restartLevel();
		}
	}
}
