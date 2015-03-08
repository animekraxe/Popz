using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NbackGenerator : MonoBehaviour {

	public Transform nbackObject;
	public int nLevel = 2;
	public int rate = 5;

	private int lastGridOffset;
	private List<Color> sequence = new List<Color>();
	
	// Use this for initialization
	void Start () {
		lastGridOffset = rate;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GenerateNbackInGrid(Grid grid, TerrainChunk tc) {
		int x = lastGridOffset;
		for ( ; x < grid.numCellsX; x += rate) {
			Transform h = GenerateNbackObjectInGrid(x, 1, grid, tc);
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
