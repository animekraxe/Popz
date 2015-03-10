using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pattern : MonoBehaviour {

	public bool display = false; // Displays pattern when true
	private Queue<Collectible> pattern; // Collectibles in the pattern that they player has not clicked on yet
	private Queue<Collectible> foundPattern; // Collectibles in the pattern that the player has clicked on already
	private Collectible currentHighlighted; // Collectible that is currently outlined
	private Transform highlight;
	private int resistance = 2;
	private bool hid = true;
	private Player player;

	public Collectible current { 
		get { return pattern.Peek (); }  
	}

	public int patternCount {
		get { return pattern.Count; }
	}

	public int length {
		get { return pattern.Count + foundPattern.Count; }
	}

	private CollectibleGenerator collectibleGen;
	public Vector3 offset;

	void Awake () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		offset = new Vector3(grid.cellSizeX, 0f, 0f);
		collectibleGen = GameObject.FindGameObjectWithTag ("CollectibleGen").GetComponent<CollectibleGenerator> ();
		pattern = new Queue<Collectible> ();
		foundPattern = new Queue<Collectible> ();
		highlight = GameObject.FindGameObjectWithTag ("Highlight").transform;
		highlight.renderer.material.color = Color.grey;
	}

	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		transform.position = bottomLeft + grid.GridToWorld (0, 0);

	}

	void Update () {
		// Displays pattern when a hint is used or when a new pattern has just been generated
		if (display) {
			DisplayPattern();
		}
		else if (!hid) {
			HidePattern();
		}
	}

	// Displays the pattern to the player
	private void DisplayPattern () {
		hid = false;
		if (currentHighlighted == null || pattern.Peek ().GetInstanceID () != currentHighlighted.GetInstanceID ()) {
			currentHighlighted = pattern.Peek ();
			highlight.position = currentHighlighted.transform.position;
		}
		highlight.renderer.enabled = true;
		foreach (var c in pattern) {
			c.gameObject.renderer.enabled = true;
		}
		foreach (var c in foundPattern) {
			c.gameObject.renderer.enabled = true;
		}

	}

	// Hides the pattern from the player
	private void HidePattern () {
		hid = true;
		highlight.renderer.enabled = false;
		foreach (var c in pattern) {
			c.gameObject.renderer.enabled = false;
		}
		foreach (var c in foundPattern) {
			c.gameObject.renderer.enabled = false;
		}
	}

	// Reveals the pattern to the player for the specified amount of time
	IEnumerator RevealPattern(float displayTime) {
		display = true;
		yield return new WaitForSeconds(displayTime);  
		display = false;
	}

	// Destroys the current pattern
	private void DestroyPattern () {
		if (display) {
			StopCoroutine ("RevealPattern");
			display = false;
		}

		foreach (var c in pattern) {
			Destroy(c.gameObject);
		}
		foreach (var c in foundPattern) {
			Destroy(c.gameObject);
		}
		pattern.Clear ();
		foundPattern.Clear();
	}

	public void RevealPattern () {
		StartCoroutine("RevealPattern",((float)length) * 0.65f);
	}

	// Called when the player clicks the correct collectible
	public void foundCollectible () {
		Collectible c = pattern.Dequeue ();
		foundPattern.Enqueue (c);
	}

	// Creates a nonselectable collectible of the specified type and at the specified position
	private Collectible CreatePatternCollectible (int type, Vector3 pos) {
		Transform t = collectibleGen.GenerateCollectible (pos.x, pos.y, type);
		t.parent = this.gameObject.transform;
		Collectible col = t.gameObject.GetComponent<Collectible>();
		col.selectable = false;
		col.gameObject.layer = 9;
		col.type = type;
		col.gameObject.renderer.enabled = false;
		return col;
	}
	
	private int previous = -1;
	// Creates a pattern of the specified lengthz
	public void GeneratePattern (int length) {
		DestroyPattern ();
		Vector3 startPos = transform.position;
		for (int i = 0; i < length; i++) {
			int randNum = Random.Range (0, collectibleGen.collectibles.Length);
			for (int j = 0; j < resistance; j++) {
				if (previous == randNum) {
					randNum = Random.Range (0, collectibleGen.collectibles.Length);
				}
				else {
					break;
				}
			}
			previous = randNum;
			
			pattern.Enqueue(CreatePatternCollectible(randNum, startPos));
			startPos = startPos + offset;
		}
		RevealPattern ();
	}


}