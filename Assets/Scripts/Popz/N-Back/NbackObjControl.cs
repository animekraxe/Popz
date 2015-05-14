using UnityEngine;
using System.Collections;

public class NbackObjControl : MonoBehaviour {
	
	private Color revealColor;
	private Player player;
	private bool isCorrect = false;

	public AudioClip success;
	public AudioClip fail;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		GetComponent<Renderer>().material.color = Color.black;
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
		var sphereCollider = player.gameObject.GetComponentInChildren<SphereCollider> ();
		var radius = sphereCollider.gameObject.transform.localScale.x / 2.0f;
		//radius += 2.0f;
		var diameter = radius * 2; // As soon as last object has passed sphere, new object will reveal.
		
		if (Util.getDistance2D (sphereCollider.gameObject, this.gameObject) < diameter) {
			GetComponent<Renderer>().material.color = revealColor;
		}
	}

	public void MarkCorrect() {
		isCorrect = true;
	}

	public void MarkColor(Color color) {
		revealColor = color;
	}
}
