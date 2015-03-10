using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {

	public TerrainChunk terrainChunk;
	public Grid grid;
	private PlatformGenerator platformGen;
	private CollectibleGenerator collectibleGen;
	private GroundGenerator groundGen;
	private NbackGenerator nbackGen;

	// Used to determine current game modes
	private PopzGameManager gameMngr;

	// Use this for initialization
	void Start () {
		gameMngr = FindObjectOfType (typeof(PopzGameManager)) as PopzGameManager;

		// Set up references to object generators and grid
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		platformGen = GameObject.FindGameObjectWithTag ("PlatformGen").GetComponent<PlatformGenerator> ();
		collectibleGen = GameObject.FindGameObjectWithTag ("CollectibleGen").GetComponent<CollectibleGenerator> ();
		groundGen = GameObject.FindGameObjectWithTag ("GroundGen").GetComponent<GroundGenerator> ();
		nbackGen = FindObjectOfType (typeof(NbackGenerator)) as NbackGenerator;

		// Position generator and box collider 
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		BoxCollider2D boxCol = gameObject.GetComponent<BoxCollider2D> ();
		float boxHeight = topRight.y - bottomLeft.y;
		float boxWidth = 1f;
		boxCol.size = new Vector2 (boxWidth, boxHeight);
		boxCol.center = new Vector2 (-boxWidth, boxHeight / 2f);
		transform.position = new Vector3(bottomLeft.x, bottomLeft.y, 0f);

		BoxCollider2D chunkBoxCol = terrainChunk.GetComponent<BoxCollider2D> ();
		float chunkBoxWidth = 2f * ((float) grid.numCellsX) * grid.cellSizeX;
		float chunkBoxHeight = topRight.y - bottomLeft.y;
		chunkBoxCol.size = new Vector2 (chunkBoxWidth, chunkBoxHeight);
		chunkBoxCol.center = new Vector2 (chunkBoxWidth/2f, chunkBoxHeight/2f);
		chunkBoxCol.isTrigger = true;

		GenerateTerrain (transform.position);
	}
	
	public void GenerateTerrainRelevantTo (Vector3 pos) {
		Vector3 spawnPos = pos + new Vector3 (((float)grid.numCellsX) * grid.cellSizeX,0f,0f);
		GenerateTerrain (spawnPos);
	}

	void GenerateTerrain (Vector3 spawnPos) {
		TerrainChunk tc = GameObject.Instantiate (terrainChunk, spawnPos, Quaternion.identity) as TerrainChunk;
		grid.ClearGrid ();

		// Generate ground
		groundGen.GenerateGrounds(grid, tc);

		// Generate For Pattern
		if (gameMngr.Modes ().Contains (GameModes.Pattern)) {
			platformGen.GeneratePlatforms (grid, tc);
			collectibleGen.GenerateCollectibles (grid, tc);
		}

		// Generate For Nback
		if (gameMngr.Modes ().Contains (GameModes.Nback)) {
			nbackGen.GenerateNbackInGrid (grid, tc);
		}
	}
}
