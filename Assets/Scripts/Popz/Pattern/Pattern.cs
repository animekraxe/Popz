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


	private List<Collectible> collectionSet;
	private List<Collectible> correctPattern;

	public Collectible current { 
		//get { return pattern.Peek (); }  
		get { return correctPattern[0]; }
	}

	public int patternCount {
		//get { return pattern.Count; }
		get { return correctPattern.Count; }
	}

	public int length {
		//get { return pattern.Count + foundPattern.Count; }
		get { return pattern.Count; }
	}

	private CollectibleGenerator collectibleGen; 
	private Vector3 offset;
	private Vector3 offsetY;

	void Awake () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		offset = new Vector3(grid.cellSizeX, 0f, 0f);
		collectibleGen = GameObject.FindGameObjectWithTag ("CollectibleGen").GetComponent<CollectibleGenerator> ();
		pattern = new Queue<Collectible> ();
		foundPattern = new Queue<Collectible> ();
		highlight = GameObject.FindGameObjectWithTag ("Highlight").transform;
		highlight.GetComponent<Renderer>().material.color = Color.grey;

		// Testing new matching mechanisms
		collectionSet = new List<Collectible> ();
		correctPattern = new List<Collectible> ();
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

		//Testing new matching
		highlight.GetComponent<Renderer>().enabled = false;
		//highlight.renderer.enabled = true;


		foreach (var c in pattern) {
			c.gameObject.GetComponent<Renderer>().enabled = true;
		}
		foreach (var c in foundPattern) {
			c.gameObject.GetComponent<Renderer>().enabled = true;
		}

	}

	// Hides the pattern from the player
	private void HidePattern () {
		// Testing new matching
		foreach (var c in collectionSet) {
			c.gameObject.GetComponent<Renderer>().enabled = false;
		}

		hid = true;
		highlight.GetComponent<Renderer>().enabled = false;
		foreach (var c in pattern) {
			c.gameObject.GetComponent<Renderer>().enabled = false;
		}
		foreach (var c in foundPattern) {
			c.gameObject.GetComponent<Renderer>().enabled = false;
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

		// Testing new matching
		foreach (var c in correctPattern) {
			Destroy (c.gameObject);
		}
		correctPattern.Clear ();

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
		//Testing new matching
		correctPattern.RemoveAt (0);

		//Collectible c = pattern.Dequeue ();
		//foundPattern.Enqueue (c);
	}

	// Creates a nonselectable collectible of the specified type and at the specified position
	private Collectible CreatePatternCollectible (int type, Vector3 pos) {
		Transform t = collectibleGen.GenerateCollectible (pos.x, pos.y, type);
		t.parent = this.gameObject.transform;
		Collectible col = t.gameObject.GetComponent<Collectible>();
		col.selectable = false;
		col.gameObject.layer = 9;
		col.type = type;
		col.gameObject.GetComponent<Renderer>().enabled = false;
		return col;
	}

	private int collectionType = -1;

	private int previous = -1;
	// Creates a pattern of the specified lengthz
	public void GeneratePattern (int length) {
		DestroyPattern ();

		// Generate forward/backward matching activity
		ChooseRandomCollectionMethod ();

		// This sets startPos to corner or center...
		//Vector3 startPos = transform.position;

		Vector3 startPos = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
		Debug.Log ("Start Pos: " + startPos);
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

		// Testing new matching
		foreach (var c in pattern) {
			if (collectionType == 0) {
				correctPattern.Add (c);
			} else if (collectionType == 1) {
				correctPattern.Insert (0, c);
			}
		}

		RevealPattern ();
	}

	void ChooseRandomCollectionMethod () {
		collectionType = Random.Range (0, 2);
	}

	void OnGUI () {
		string displayString = "";
		if (collectionType == 0) {
			displayString = "Forward";
		} else if (collectionType == 1) {
			displayString = "Backward";
		}

		// Displays pattern when a hint is used or when a new pattern has just been generated
		if (display) {
			GUI.Button(new Rect(Camera.main.pixelWidth / 2 - 50, Camera.main.pixelHeight / 2 - 50, 100, 30), displayString);
		}
		else if (!hid) {
		}
	}
}