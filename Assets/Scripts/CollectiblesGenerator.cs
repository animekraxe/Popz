using UnityEngine;
using System.Collections;

public class CollectiblesGenerator : MonoBehaviour {

	public float spawnTime = 2f;
	public float spawnChance = 0.5f;
	private Transform[] collectibles;
	private bool start = true;
	private Grid grid;

	// Use this for initialization
	void Start () {
		collectibles = GameObject.FindGameObjectWithTag("Pattern").GetComponent<Pattern>().collectibles;
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			StartCoroutine(SpawnCollectible());
			start = false;
		}
		spawnTime = Random.Range (0.5f, 2f);
	}
	
	
	IEnumerator SpawnCollectible() {
		while(true) {
			for (int height = 0; height < grid.numCellsY; height++) {
				if (Random.value > spawnChance) {
					continue;
				}
				int type = Random.Range (0, collectibles.Length);
				Vector3 spawnPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight/2));
				spawnPoint.y = grid.playerPos.y + ((float) height) * grid.cellSize + (grid.cellSize/2f); 
				Transform t = GameObject.Instantiate (collectibles [type], new Vector3(spawnPoint.x,spawnPoint.y,0), Quaternion.identity) as Transform;
				t.parent = this.gameObject.transform;

			}
			yield return new WaitForSeconds(spawnTime);
		}
	}

}
