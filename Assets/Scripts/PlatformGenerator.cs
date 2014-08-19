using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

	public Platform[] platforms; // Possible platforms that will be generated
	public float spawnChance = 0.6f; // The chance a platform will be generated in a grid cell

	private Grid grid; // Reference to the grid used
	public int maxCellLength = 1;

	void Awake() {
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		for (int i = 0; i < platforms.Length; i++) {
			if (platforms[i].numCells > maxCellLength) {
				maxCellLength = platforms[i].numCells;
			}
		}
	}
	
	// Generates platforms
	void Update () {
		if (!grid.GeneratePlatforms) { return; }

		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 0; y < grid.numCellsY; y++) {
				if (Random.value > spawnChance || grid.containsObject(x,y)) {
					continue;
				}
				int platformType = Random.Range (0, platforms.Length);
				GeneratePlatform (x, y, platformType);
			}
		}
		grid.GeneratePlatforms = false;
		grid.GenerateCollectibles = true;
	}

	// Generates platform of the specified type at the grid location (x,y)
	private void GeneratePlatform (int x, int y, int type) {
		Vector3 offset =  new Vector3 (((float)(platforms[type].numCells - 1)) * 0.5f * grid.cellSizeX, -grid.cellSizeY/2f, 0f);
		Vector3 spawnPos = grid.GridToWorld (x, y) + offset;
		Platform t = GameObject.Instantiate (platforms [type], spawnPos, Quaternion.identity) as Platform;
		t.transform.parent = this.gameObject.transform;
		for (int i = 0; i < platforms[type].numCells; i++) {
			grid.MarkGrid (x + i, y);
		}
	}
}
