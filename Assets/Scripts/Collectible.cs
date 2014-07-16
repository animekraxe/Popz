using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collectible : MonoBehaviour {

	public Color color;
	public Shape shape;
	public bool selectable = true;

	private Queue<Collectible> pattern;
	private Queue<Collectible> foundPattern;


	// Use this for initialization
	void Start () {
		renderer.material.color = color;
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern>().pattern;
		foundPattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern>().foundPattern;
	}

	void OnMouseDown () {
		if (!selectable) { return; }
		Collectible current = pattern.Peek ();
		if (current.color == color && current.shape == shape) {
			Destroy (gameObject);
			Collectible c = pattern.Dequeue();
			foundPattern.Enqueue(c);

		}
	}

	void OnTriggerEnter (Collider obj) {
		if (!obj.gameObject.tag.Equals("Player")) { return; }
		if (!selectable) { return; }
		Collectible current = pattern.Peek ();
		if (current.color == color && current.shape == shape) {
			Destroy (gameObject);
			Collectible c = pattern.Dequeue();
			foundPattern.Enqueue(c);
			
		}
	}

	void OnBecameInvisible () {
		if (selectable) {
			Destroy (gameObject);
		}
	}

}
