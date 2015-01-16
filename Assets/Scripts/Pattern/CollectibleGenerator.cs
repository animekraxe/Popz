using UnityEngine;
using System.Collections;

public class CollectibleGenerator : MonoBehaviour {

	public Transform[] collectibles; // Possible collectibles that will be generated
	public float spawnChance = 0.9f; // The chance a collectible will be generated in a grid cell

	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		for (int i = 0; i < collectibles.Length; i++) {
			BoxCollider boxCol = collectibles[i].GetComponent<BoxCollider> ();
			Vector3 scale = collectibles[i].localScale;
			boxCol.size = new Vector3(grid.cellSizeX/scale.x, grid.cellSizeY/scale.y, 1f);
			boxCol.center = new Vector3(0f, 0f, 0f);
		}
	}

	// Generates collectibles in cells of the given grid
	public void GenerateCollectibles (Grid grid, TerrainChunk tc) {
		for (int x = 0; x < grid.numCellsX; x++) {
			for (int y = 3; y < grid.numCellsY; y+=2) {
				if (Random.value > spawnChance || !grid.containsObject(x,y-1)) {
					continue;
				}
				int collectibleType = Random.Range (0, collectibles.Length);
				GenerateCollectible (x, y, collectibleType, grid, tc);
			}
		}
	}

	// Generates collectible of the specified type at the grid location (x,y)
	public Transform GenerateCollectible (int x, int y, int type, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position;
		Transform t = GenerateCollectible (spawnPos.x, spawnPos.y, type);
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}

	// Generates collectible of the specified type at world coordinates (x,y)
	public Transform GenerateCollectible (float x, float y, int type) {
		Vector3 spawnPos = new Vector3 (x,y,0); 
		Transform t = GameObject.Instantiate (collectibles [type], spawnPos, Quaternion.identity) as Transform;
		Collectible col = t.gameObject.GetComponent<Collectible>();
		return t;
	}
}
