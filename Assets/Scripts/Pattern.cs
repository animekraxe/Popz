using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Order { Forward, Backward }; // Possible orderings of collectibles

public class Pattern : MonoBehaviour {

	public Transform[] collectibles; // Possible collectibles in the pattern
	public int patternLength = 1; // Number of collectibles in the pattern
	public bool display = false; // Displays pattern when true
	public bool failedPattern = false; // True when player clicks on the incorrect collectible
	public Order order = Order.Forward;
	public int numLives = 3; // Number of mistakes player can make before pattern length is decremented
	public int numRounds = 5; // Number of rounds player must complete before pattern length is incremented
	public bool transition = false;

	private Queue<Collectible> pattern; // Collectibles in the pattern that they player has not clicked on yet
	private Queue<Collectible> foundPattern; // Collectibles in the pattern that the player has clicked on already
	
	private Collectible currentHighlighted; // Collectible that is currently outlined
	private Transform highlight;

	private int livesPerCollection;
	private int roundsPerCollection;
	private int resistance = 2;
	private bool hid = true;

	public int maxPatternLength;

	public Collectible current { 
		get { return pattern.Peek (); }  
	}

	void Start () {
				
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		bottomLeft.x += 1f;
		GameObject ground = GameObject.FindGameObjectWithTag ("Ground");
		bottomLeft.y += ground.GetComponent<SpriteRenderer> ().sprite.bounds.extents.y * ground.transform.localScale.y;
		bottomLeft.z = 1f;

		transform.position = bottomLeft;

		pattern = new Queue<Collectible> ();
		foundPattern = new Queue<Collectible> ();
		livesPerCollection = numLives;
		roundsPerCollection = numRounds;
	
		highlight = GameObject.FindGameObjectWithTag ("Highlight").transform;
		highlight.renderer.material.color = Color.grey;
		GeneratePattern(patternLength);

		Vector3 spriteSize = collectibles [0].GetComponent<SpriteRenderer> ().sprite.bounds.size;
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		Vector3 topLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, Camera.main.pixelHeight, 0f));
		float length = topRight.x - topLeft.x - 3f;
		maxPatternLength = Mathf.FloorToInt(length / (spriteSize.x * collectibles[0].localScale.x));
	}

	void Update () {

		if (pattern.Count == 0) {
			if (numRounds > 1) {
				numRounds--;
			}
			else {
				numLives = livesPerCollection;
				numRounds = roundsPerCollection;
				if (patternLength < maxPatternLength) {
					patternLength++;
				}
			}
			GeneratePattern(patternLength);
		}
		else if (failedPattern) {
			if (numLives > 1) {
				numLives--;
				RevealPattern ();
			}
			else {
				numRounds = roundsPerCollection;
				numLives = livesPerCollection;
				if (patternLength > 2) {
					patternLength--;
					StartCoroutine("Transition");
				}

			}
			failedPattern = false;	
		}

		// Skips and generates new pattern is "s" is pressed
		if (Input.GetKeyDown ("s")) {
			GeneratePattern(patternLength);
		}

		// Displays pattern when a hint is used or when a new pattern has just been generated
		if (display) {
			DisplayPattern();
		}
		else if (!hid) {
			HidePattern();
		}

		if (Input.GetKeyDown ("q")) {
			StartCoroutine("Transition",3f);
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

	private int previous = -1;

	// Creates a pattern of the specified lengthz
	private void GeneratePattern (int length) {
		DestroyPattern ();
		Vector3 startPos = transform.position;
		Vector3 offset = new Vector3 (1.3f, 0f, 0f);


		switch (order) {
			case Order.Forward:
				for (int i = 0; i < length; i++) {
					int randNum = Random.Range (0, collectibles.Length);
					for (int j = 0; j < resistance; j++) {
						if (previous == randNum) {
							randNum = Random.Range (0, collectibles.Length);
						}
						else {
							break;
						}
					}
					previous = randNum;
					
					pattern.Enqueue(CreatePatternCollectible(randNum, startPos));
					startPos = startPos + offset;
				}
				break;
			case Order.Backward:
				Stack<Collectible> tempStack = new Stack<Collectible>();
				for (int i = 0; i < length; i++) {
					int randNum = Random.Range (0, collectibles.Length);
					for (int j = 0; j < resistance; j++) {
						if (previous == randNum) {
							randNum = Random.Range (0, collectibles.Length);
						}
						else {
							break;
						}
					}
					previous = randNum;
					
					tempStack.Push(CreatePatternCollectible(randNum, startPos));
					startPos = startPos + offset;
				}
				while (tempStack.Count > 0) {
					pattern.Enqueue(tempStack.Pop());
				}
				break;
			default:
				Debug.Log ("Pattern ordering type is not specified.");
				break;
		}
		StartCoroutine("RevealPattern",((float)length) * 0.65f);
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

	// Creates a nonselectable collectible of the specified type and at the specified position
	private Collectible CreatePatternCollectible (int type, Vector3 pos) {
		Transform t = GameObject.Instantiate (collectibles [type], pos, Quaternion.identity) as Transform;
		t.parent = this.gameObject.transform;
		Collectible col = t.gameObject.GetComponent<Collectible>();
		col.selectable = false;
		col.gameObject.layer = 9;
		col.type = type;
		col.gameObject.renderer.enabled = false;
		return col;
	}

	public void RevealPattern () {
		StartCoroutine("RevealPattern",((float)patternLength) * 0.65f);
	}

	// Called when the player clicks the correct collectible
	public void foundCollectible () {
		Collectible c = pattern.Dequeue ();
		foundPattern.Enqueue (c);
	}

	IEnumerator Transition () {
		transition = true;
		Runner camera = Camera.main.GetComponent<Runner> ();
		BackgroundManager bg = GameObject.FindGameObjectWithTag ("Backgrounds").GetComponent <BackgroundManager> ();
		float cameraSpeed = camera.speed;
		float backgroundSpeed = bg.speed;
		camera.speed = cameraSpeed * 40f;
		bg.speed = 10f * backgroundSpeed;
		yield return new WaitForSeconds(0.8f);
		 
		camera.speed = cameraSpeed;
		bg.speed = backgroundSpeed;
		GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ().ResetGridPosition (); 
		transition = false;
		GeneratePattern(patternLength);

	}
}