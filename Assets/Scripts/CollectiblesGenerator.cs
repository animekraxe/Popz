using UnityEngine;
using System.Collections;

public class CollectiblesGenerator : MonoBehaviour {

	public Transform[] collectibles;
	public float spawnChance = 0.85f;

	private Grid grid;
	
	void Start () {
		collectibles = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ().collectibles;
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!grid.GenerateCollectibles) { return; }

		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 0; y < grid.numCellsY; y++) {
				if (Random.value > spawnChance || !grid.containsObject(x,y)) {
					continue;
				}
				int collectibleType = Random.Range (0, collectibles.Length);
				GenerateCollectible (x, y, collectibleType);
			}
		}
		grid.GenerateCollectibles = false;

	}
	
	private void GenerateCollectible (int x, int y, int type) {
		Vector3 spawnPos = grid.GridToWorld (x,y); 
		Transform t = GameObject.Instantiate (collectibles [type], spawnPos, Quaternion.identity) as Transform;
		t.parent = this.gameObject.transform;
		Collectible col = t.gameObject.GetComponent<Collectible>();
		col.gameObject.layer = 9;
	}


}
