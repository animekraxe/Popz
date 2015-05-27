using UnityEngine;
using System.Collections;

public class GroundGenerator : MonoBehaviour {
		
	public Transform groundPiece;

	public Transform ground1wide;
	public Transform ground2wide;
	public Transform ground3wide;
	
	public Transform ceiling1wide;
	public Transform ceiling2wide;
	public Transform ceiling3wide;

	// Use this for initialization
	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
	}

	public Transform GenerateGround (int x, int y, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position; 
		Transform t = GenerateGround (spawnPos.x, spawnPos.y);
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}

	public Transform GenerateGround (float x, float y) {
		Vector3 spawnPos = new Vector3 (x,y,0); 
		Transform t = GameObject.Instantiate (groundPiece, spawnPos, Quaternion.identity) as Transform;
		return t;
	}
	
	public void GenerateGrounds (Grid grid, TerrainChunk tc, int y = 0, 
	                             bool ceiling = false, int level = -1) {
		// Generate ground and potholes
		for (int i = 0; i < grid.numCellsX; ++i) {
			if (grid.containsObject(i, y)) {
				continue;
			}
			
			// Generate random width ground pieces varying from 1-3
			int cap, roll;
			do {
				cap = Mathf.Min (4, grid.numCellsX - i + 1);
				roll = Random.Range (1, cap);
			} while (!GenerateWideGround(i, y, roll, grid, tc, ceiling));
		}
	}

	public Transform GenerateWideGround (int x, int y, int wide, Grid grid, TerrainChunk tc, bool ceiling = false) {
		// Check grid spaces aren't currently occupied
		for (int i = x; i < x + wide; ++i) {
			if (grid.containsObject(i, y)) {
				return null;
			}
		}

		// Position piece by averaging the grid space positions
		Vector3 spawnPos = new Vector3 (0, 0, 0);
		for (int i = x; i < x + wide; ++i) {
			spawnPos += grid.GridToWorld(i, y);
		}
		spawnPos /= wide;
		spawnPos += tc.transform.position;
		spawnPos.z = 0;
		//Vector3 spawnPos = grid.GridToWorld (x, y) + tc.transform.position;

		// Create piece
		Transform piece;
		if (ceiling) {
			piece = wide == 3 ? ceiling3wide : (wide == 2 ? ceiling2wide : ceiling1wide);
			float topY = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0)).y;
			spawnPos.y = topY - (grid.cellSizeY / 2.0f) + 0.2f;
		} else {
			piece = wide == 3 ? ground3wide : (wide == 2 ? ground2wide : ground1wide);
		}
		Transform t = GameObject.Instantiate (piece, spawnPos, Quaternion.identity) as Transform;

		// Mark grid spaces
		for (int i = x; i < x + wide; ++i) {
			grid.MarkGrid(i, y);
		}

		return t;
	}
}
