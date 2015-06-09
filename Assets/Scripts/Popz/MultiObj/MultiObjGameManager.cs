using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiObjGameManager : MonoBehaviour {

	public int level;
	public int stage;
	public float pushSpeed = 6.0f;

	public GameObject field;
	public GameObject player;
	public List<Transform> trackingObjects;

	// Game State Variables
	private bool gameRunning = false;

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

			int rand = Random.Range (0, trackingObjects.Count);
			Transform creature = Instantiate (trackingObjects[rand], spawnPosition, Quaternion.identity) as Transform;
			creature.gameObject.GetComponent<Movement>().field = field;
			creature.gameObject.GetComponent<Movement>().pushSpeed = pushSpeed;
		}
	}
	
	public void startLevel () {
		if (stage > level) {
			++level;
			stage = 1;
		}
		
		if (stage <= 0) {
			stage = 1;
		}

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
