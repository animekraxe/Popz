using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NbackGenerator : MonoBehaviour {

	public Transform nbackObject;
	public int nLevel = 2;
	public int rate = 5;

	public bool platforms = true;
	public int navigationDifficulty = 0;

	private int lastGridOffset;
	private List<Color> sequence = new List<Color>();

	void Awake () {
		if (Settings.isSet) {
			platforms = Settings.togglePlatformsNback;
			navigationDifficulty = Settings.nbackNavigationDifficulty;
		}
	}

	// Use this for initialization
	void Start () {
		lastGridOffset = rate;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Entry for prototype platforms generation
	public void GenerateNbackInGridPlatforms(Grid grid, TerrainChunk tc, GroundGenerator ggen) {
		int x = lastGridOffset;
		int topY = grid.numCellsY - 2;
		int midY = topY / 2;
		for ( ; x < grid.numCellsX; x += rate) {
			int roll = Random.Range (0, 4);
			int placement = roll == 0 ? 1 : (roll == 1 ? midY + 1: topY + 1);
			Transform h = GenerateNbackObjectInGrid(x, placement, grid, tc);
		}
		lastGridOffset = x - grid.numCellsX;

		for (int y = 0; y <= grid.numCellsY; ++y) {
			ggen.GenerateGround(y, topY, grid, tc);
			ggen.GenerateGround(y, midY, grid, tc);
			ggen.GenerateGround(y, 0, grid, tc);
		}

		return;
	}

	int generateCount = 0;
	public void GenerateNbackInGrid(Grid grid, TerrainChunk tc, GroundGenerator ggen) {
		int difficulty = navigationDifficulty;
		int scale = 10;

		// Generate beginning so character doesnt fall
		if (generateCount == 0) {
			for (int i = 0; i < 5; ++i) {
				ggen.GenerateGround (i, 0, grid, tc);
				ggen.GenerateGround (i, 7, grid, tc);
			}
		}
		++generateCount;

		// Entry for prototype platforms generation
		if (platforms) {
			GenerateNbackInGridPlatforms(grid, tc, ggen);
			return;
		}

		// Generate Nback collectibles
		int x = lastGridOffset;
		for ( ; x < grid.numCellsX; x += rate) {
			int rand = Random.Range(0, 2);
			int y = rand == 0 ? 1 : 6;
			Transform h = GenerateNbackObjectInGrid(x, y, grid, tc);
		}

		// Generate ground and potholes
		for (int i = 0; i < grid.numCellsX; ++i) {
			// For floor
			// Generate ground (NOT pothole) by scale and difficulty
			if (grid.containsObject(i, 0)) {
				Debug.Log ("Grid Contains at: " + i);
				continue;
			}
			int cap = Mathf.Min (4, grid.numCellsX - i + 1);
			int roll = Random.Range (1, cap);

			while (!ggen.GenerateWideGround(i, 0, roll, grid, tc)) {
				roll = Random.Range (1, 4);
			}

//			int rand = Random.Range(0, 100);
//			if (rand > difficulty * scale) {
//				ggen.GenerateGround (i, 0, grid, tc);
//			}
//
//			// For Ceiling
//			rand = Random.Range(0, 100);
//			if (rand > difficulty * scale) {
//				//ggen.GenerateGround (i, 7, grid, tc);
//			}
		}
		lastGridOffset = x - grid.numCellsX;
	}

	Transform GenerateNbackObjectInGrid(int x, int y, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position; 
		Transform t = GenerateNbackObject (spawnPos.x, spawnPos.y);
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}

	Transform GenerateNbackObject(float x, float y) {
		Vector3 spawnPos = new Vector3 (x, y, 0); 
		Debug.Log ("Generating at: " + spawnPos);
		Transform t = GameObject.Instantiate (nbackObject, spawnPos, Quaternion.identity) as Transform;
		NbackObjControl ctrl = t.gameObject.GetComponent<NbackObjControl> ();

		// Update color sequence
		Color color = Util.randomColor ();
		sequence.Add (color);
		ctrl.MarkColor (color);
		if (Util.checkNbackMatch(sequence, nLevel)) {
			ctrl.MarkCorrect();
		}

		return t;
	}
}
