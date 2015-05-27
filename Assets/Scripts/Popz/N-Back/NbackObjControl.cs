using UnityEngine;
using System.Collections;

// Mineral types
public enum Mineral {
	Yellow,
	Green,
	Red,
	Blue,
	White,
};

public class NbackObjControl : MonoBehaviour {
	public AudioClip success;
	public AudioClip fail;
	public Mineral type;
	public Sprite cloakSprite;
	private Sprite revealSprite;

	private bool isCorrect = false;

	// The currently revealed object on the screen
	static NbackObjControl currentRevealed = null;

	// Use this for initialization
	void Start () {
		revealSprite = GetComponent<SpriteRenderer> ().sprite;
		GetComponent<SpriteRenderer> ().sprite = cloakSprite;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateReveal ();
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if (isCorrect) {
			AudioSource.PlayClipAtPoint(success, this.transform.position);
		} else {
			AudioSource.PlayClipAtPoint(fail, this.transform.position);
		}
		Destroy (this.gameObject);
	}

	void UpdateReveal() {
		// Reveal when past halfway of the screen
		float halfway = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth / 2, 0)).x;

		if (transform.position.x < halfway) {
			UpdateCurrentRevealed(this);
		}
	}

	void Reveal() {
		GetComponent<SpriteRenderer> ().sprite = revealSprite;
	}

	void Cloak() {
		GetComponent<SpriteRenderer> ().sprite = cloakSprite;
	}

	static void UpdateCurrentRevealed(NbackObjControl next) {
		currentRevealed = next;
		next.Reveal ();
	}

	public void MarkCorrect() {
		isCorrect = true;
	}
}
