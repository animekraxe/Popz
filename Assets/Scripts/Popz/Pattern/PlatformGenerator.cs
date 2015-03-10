using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

	public Platform[] platforms; // Possible platforms that will be generated
	public float spawnChance = 0.7f; // The chance a platform will be generated in a grid cell
	public int maxCellLength = 1;

	void Awake() {
		for (int i = 0; i < platforms.Length; i++) {
			if (platforms[i].numCells > maxCellLength) {
				maxCellLength = platforms[i].numCells;
			}
		}
	}
	
	// Generates platforms in cells of given grid
	public void GeneratePlatforms (Grid grid, TerrainChunk tc) {
		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 2; y < grid.numCellsY; y+=2) {
				if (Random.value > spawnChance) {
					continue;
				}
				int platformType = Random.Range (0, platforms.Length);
				GeneratePlatform (x, y, platformType, grid, tc);
			}
		}
	}

	// Generates platform of the specified type at the grid location (x,y)
	public Transform GeneratePlatform (int x, int y, int type, Grid grid, TerrainChunk tc) {
		for (int i = 0; i < platforms[type].numCells; i++) {
			if (grid.containsObject(x + i, y)) {
				return null;
			}
		}
		Vector3 offset =  new Vector3 (((float)(platforms[type].numCells - 1)) * 0.5f * grid.cellSizeX, 0f, 0f);
		Vector3 spawnPos = grid.GridToWorld (x, y) + offset + tc.transform.position;
		for (int i = 0; i < platforms[type].numCells; i++) {
			grid.MarkGrid (x + i, y);
		}
		Transform t = GeneratePlatform (spawnPos.x, spawnPos.y, type);
		t.parent = tc.gameObject.transform;
		return t;
	}

	// Generates platform of the specified type at world coordinates (x,y)
	public Transform GeneratePlatform (float x, float y, int type) {
		Vector3 spawnPos = new Vector3 (x, y, 0);
		Platform p = GameObject.Instantiate (platforms [type], spawnPos, Quaternion.identity) as Platform;
		return p.gameObject.transform;
	}
}
