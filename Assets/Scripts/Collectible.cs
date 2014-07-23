using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collectible : MonoBehaviour {

	public Color color; 
	public Shape shape; 
	public bool selectable = true;

	//public AudioClip success;
	//public AudioClip fail;
	
	private Pattern patternManager;
	
	void Start () {
		renderer.material.color = color;
		patternManager = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
	}

	void OnMouseDown () {
		if (!selectable) { return; }
		Collectible current = patternManager.pattern.Peek ();
		if (current.color == color && current.shape == shape) {
			Destroy (gameObject);
			Collectible c = patternManager.pattern.Dequeue ();
			patternManager.foundPattern.Enqueue (c);
			//AudioSource.PlayClipAtPoint(success, transform.position);
		} 
		else {
			patternManager.failedPattern = true;
			//AudioSource.PlayClipAtPoint(fail, transform.position);
		}
	}

	// If collectible is off screen, delete it
	void OnBecameInvisible () {
		if (selectable) {
			Destroy (gameObject);
		}
	}

}
