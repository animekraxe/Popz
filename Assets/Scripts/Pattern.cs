using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Shape { Sphere, Cube, Capsule }; // Possible shapes of collectibles
public enum Order { Forward, Backward, Custom }; // Possible orderings of collectibles

public class Pattern : MonoBehaviour {

	public Transform[] collectibles; // The collectibles that have the possibility of appearing in the pattern
	public Queue<Collectible> pattern; // Collectibles in the pattern that they player has not clicked on yet
	public Queue<Collectible> foundPattern; // Collectibles in the pattern that the player has clicked on already

	public int patternLength = 1; // Number of collectibles in the pattern
	private bool display = false; // True only when a new pattern is generated
	public bool failedPattern = false; // True when player clicks on the incorrect collectible
	public int hints = 3;

	void Start () {
		if (collectibles == null) {
			Debug.Log ("Collectibles array is empty.");
		}
		pattern = new Queue<Collectible> ();
		foundPattern = new Queue<Collectible> ();
	}

	void Update () {
		if (pattern.Count == 0) {
			patternLength++;
			GeneratePattern(patternLength);
		}
		else if (failedPattern) {
			if (patternLength > 1) {
				patternLength--;
			}
			GeneratePattern(patternLength);
			failedPattern = false;
		}

		// Displays pattern when "d" is pressed or when a new pattern has just been generated
		if (Input.GetKeyDown ("d") && hints > 0 && !display) {
			StartCoroutine(RevealPattern(((float)patternLength) * 0.6f, true));
		}

		if (display) {
			DisplayPattern();
		}
		else {
			HidePattern();
		}
	}

	// Displays the pattern to the player
	private void DisplayPattern () {
		Collectible current = pattern.Peek ();
		current.renderer.material.color = Color.white;
		foreach (var c in pattern) {
			c.gameObject.renderer.enabled = true;
		}
		foreach (var c in foundPattern) {
			c.renderer.material.color = c.color;
			c.gameObject.renderer.enabled = true;
		}

	}

	// Hides the pattern from the player
	private void HidePattern () {
		foreach (var c in pattern) {
			c.gameObject.renderer.enabled = false;
		}
		foreach (var c in foundPattern) {
			c.gameObject.renderer.enabled = false;
		}
	}

	// Reveals the pattern to the player for the specified amount of time
	IEnumerator RevealPattern(float displayTime, bool hint) {
		if (hint) {
			hints--;
		}
		display = true;
		yield return new WaitForSeconds(displayTime);  
		display = false;
	}

	// Creates a pattern of the specified length
	private void GeneratePattern (int length) {
		if (display) {
			display = false;
			StopCoroutine ("RevealPattern");
		}
		foreach (var c in pattern) {
			Destroy(c.gameObject);
		}
		foreach (var c in foundPattern) {
			Destroy(c.gameObject);
		}
		pattern.Clear ();
		foundPattern.Clear();
		Vector3 startPos = transform.position;
		Vector3 offset = new Vector3 (1f, 0f, 0f);

		for (int i = 0; i < length; i++) {
			int randNum = Random.Range (0, collectibles.Length);
			Transform t = GameObject.Instantiate (collectibles [randNum], startPos, Quaternion.identity) as Transform;
			t.parent = this.gameObject.transform;
			Collectible c = t.gameObject.GetComponent<Collectible>();
			c.selectable = false;
			c.gameObject.layer = 8;
			pattern.Enqueue(c);
			startPos = startPos + offset;
		}

		StartCoroutine(RevealPattern(((float)length) * 0.6f, false));
	}
}