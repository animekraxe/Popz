using UnityEngine;
using System.Collections;

public struct Point
{
	public int x;
	public int y;

	public Point(int px, int py)
	{
		x = px;
		y = py;
	}
}

public class Grid : MonoBehaviour {

	public int numCellsX, numCellsY;
	public float cellSize;
	public Transform[,] grid;

	public Vector3 cameraPos;
	public Vector3 playerPos;

	// Use this for initialization
	void Start () {
		grid = new Transform[numCellsX, numCellsY];
		cameraPos = Camera.main.ScreenToWorldPoint (new Vector3 (0f, Camera.main.pixelHeight, 0f));
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerPos = player.position + new Vector3 (0f, player.localScale.y / 2f, 0f);
		cellSize = (cameraPos.y - playerPos.y) / ((float)numCellsY);
	}


	public Point WorldToGrid (Vector3 worldPos) {
		//TODO
		return new Point (1, 1);
	}

	public Vector3 GridToWorld (Point gridPos) {
		//TODO
		return Vector3.zero;
	}

}
