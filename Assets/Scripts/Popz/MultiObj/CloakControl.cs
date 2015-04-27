using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloakControl : MonoBehaviour {

	// Reference to Player
	//TODO: REPLACE WITH REFERENCE TO GAME MANAGER STATES
	public MultiObjPlayer player;

	// Distractor Parameters
	public bool is_distractor = false;

	// Cloak State Variables
	private Material initMaterial;
	private Material revealMaterial;
	private float revealTicker;

	private List<Color> colorSet;
	public AudioClip success;
	public AudioClip fail;

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().material.color = Color.black;
		initMaterial = this.GetComponent<Renderer>().material;
		revealMaterial = initMaterial;

		if (!is_distractor) {
			initMaterial = this.GetComponent<Renderer>().material;
			revealMaterial = new Material (initMaterial);
			revealMaterial.color = Util.randomColorFromSet(colorSet);
		}

		timedReveal (3.5f);
	}
	
	// Update is called once per frame
	void Update () {
		updateCloak();
	}

	private void updateCloak () {
		if (revealTicker > 0) {
			revealTicker -= Time.deltaTime;
			GetComponent<Renderer>().material = revealMaterial;
		}
		else {
			GetComponent<Renderer>().material = initMaterial;
		}
	}

	public void timedReveal (float time) {
		revealTicker = time;
	}

	public bool isCloaked () {
		return revealTicker <= 0;
	}

	public bool isRevealed () {
		return !isCloaked ();	
	}

	public void setColorSet (List<Color> cset) {
		colorSet = cset;
	}

	public bool isDistractor() {
		return is_distractor;
	}

	void OnMouseDown () {
		// Verify in radius
		var selectionRadius = player.GetComponentInChildren<SphereCollider> ();
		var radius = selectionRadius.gameObject.transform.localScale.x / 2.0f;

		var object2D = new Vector2 (transform.position.x, transform.position.y);
		var player2D = new Vector2 (selectionRadius.transform.position.x,
		                            selectionRadius.transform.position.y);

		//var dist = transform.position - selectionRadius.gameObject.transform.position;
		var dist = object2D - player2D;
		Debug.Log ("Radius: " + radius);
		Debug.Log ("Distance: " + dist.magnitude);

		if (dist.magnitude > radius) {
			Debug.Log("Not in selection radius");
			return;
		}
	}

	public void validate () {
		//TODO: MOVE SCORING AND VERIFICATION TO GAME MANAGER OR NEW SCRIPT
		var is_correct = player.currentColor () == revealMaterial.color;
		if (is_correct) {
			player.AddToScore(100, !isDistractor());
			AudioSource.PlayClipAtPoint(success, this.transform.position);
		} else {
			player.AddToScore (-100, !isDistractor());
			AudioSource.PlayClipAtPoint(fail, this.transform.position);
		}
	}
}
