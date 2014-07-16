using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Shape { Sphere, Cube, Capsule };

public class Pattern : MonoBehaviour {

	public Transform[] collectibles;
	public Queue<Collectible> pattern;
	public Queue<Collectible> foundPattern;

	public int patternLength = 2;
	public float displayTime = 0.8f;

	private bool display = false;
	
	void Start () {
		if (collectibles == null) {
			Debug.Log ("Collectibles array is empty.");
		}
		pattern = new Queue<Collectible> ();
		foundPattern = new Queue<Collectible> ();
	}

	void Update () {
		displayTime = ((float)patternLength) * 0.6f;


		if (pattern.Count == 0) {
			foreach (var c in foundPattern) {
				Destroy(c.gameObject);
			}
			foundPattern.Clear();
			patternLength++;
			GeneratePattern();
		}

		if (Input.GetKey ("d") || display) {
			DisplayPattern();
		}
		else {
			HidePattern();
		}


	}

	private void DisplayPattern () {
		foreach (var c in pattern) {
			c.gameObject.renderer.enabled = true;
		}
		foreach (var c in foundPattern) {
			c.gameObject.renderer.enabled = true;
		}

	}

	private void HidePattern () {
		foreach (var c in pattern) {
			c.gameObject.renderer.enabled = false;
		}
		foreach (var c in foundPattern) {
			c.gameObject.renderer.enabled = false;
		}
	}

	IEnumerator RevealPattern(float displayTime) {
		display = true;
		yield return new WaitForSeconds(displayTime);
		display = false;
	}

	private void GeneratePattern () {
		pattern.Clear ();
		Vector3 startPos = transform.position;
		Vector3 offset = new Vector3 (1f, 0f, 0f);
		for (int i = 0; i < patternLength; i++) {
			int randNum = Random.Range (0, collectibles.Length);
			Transform t = GameObject.Instantiate (collectibles [randNum], startPos, Quaternion.identity) as Transform;
			t.parent = this.gameObject.transform;
			pattern.Enqueue(t.gameObject.GetComponent<Collectible>());
			startPos = startPos + offset;
		}

		foreach (var c in pattern) {
			c.gameObject.GetComponent<Collectible>().selectable = false;
			c.gameObject.layer = 8;
		}
		StartCoroutine(RevealPattern(displayTime));
	}
}