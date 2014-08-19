using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public int numCellsX, numCellsY; // Number of cells in the screen
	public bool GeneratePlatforms; // True when platforms will be generated
	public bool GenerateCollectibles;
	 
	public float cellSizeX, cellSizeY; // Cell size in horizontal and vertical directions
	private bool[,] grid; // grid[x,y] is true is the grid cell (x,y) contains an object
	private Vector3 offset;

	// Calculates cell sizes and sets up positions
	void Start () {
		
		PlatformGenerator pg = GameObject.FindGameObjectWithTag ("Platforms").GetComponent<PlatformGenerator> ();

		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		Vector3 topLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, Camera.main.pixelHeight, 0f));
		offset = topRight - topLeft;

		grid = new bool[numCellsX + pg.maxCellLength - 1, numCellsY];
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		Vector3 playerPos = player.position + new Vector3 (0f, player.localScale.y / 2f, 0f);
		cellSizeY = (topRight.y - playerPos.y) / ((float)numCellsY);
		cellSizeX = (topRight.x - topLeft.x) / ((float)numCellsX);

		Vector3 temp = (topRight - new Vector3(0f, ((float) numCellsY) * cellSizeY, 0f)) + offset;
		temp.z = 0f;
		transform.position = temp;
		GeneratePlatforms = true;

	}

	// Moves the grid by offset once the camera reaches the grid
	void Update () {
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		if (transform.position.x <= topRight.x) {
			transform.position += offset;
			ClearGrid ();
			GeneratePlatforms = true;
		}
	}
	
	private void ClearGrid () {
		for (int x = 0; x < numCellsX; x++) {
			for (int y = 0; y < numCellsY; y++) {
				grid[x,y] = false;
			}
		}
		for (int x = numCellsX + 1; x < grid.GetLength(0); x++) {
			for (int y = 0; y < numCellsY; y++) {
				grid[x - numCellsX - 1, y] = grid[x,y];
				grid[x,y] = false;
			}
		}
	}

	// Called when an object is placed in grid cell (x,y)
	public void MarkGrid (int x, int y) {
		grid[x, y] = true;
	}

	// Returns the world position of the specified grid cell
	public Vector3 GridToWorld (int x, int y) {
		Vector3 pos = transform.position;
		pos.x -= ((float) numCellsX - x - 1) * cellSizeX - cellSizeX/2f;
		pos.y += ((float) y) * cellSizeY + cellSizeY/2f;
		return pos;
	}

	// Is true if grid cell (x,y) contains an object 
	public bool containsObject (int x, int y) {
		return grid[x,y];
	}
}
