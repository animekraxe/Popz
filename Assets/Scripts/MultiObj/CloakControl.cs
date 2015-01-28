using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloakControl : MonoBehaviour {

	// Reference to Player
	//TODO: REPLACE WITH REFERENCE TO GAME MANAGER STATES
	public Player player;

	// Distractor Parameters
	public bool isDistractor = false;

	// Cloak State Variables
	private Material initMaterial;
	private Material revealMaterial;
	private float revealTicker;

	private List<Color> colorSet;

	// Use this for initialization
	void Start () {
		this.renderer.material.color = Color.black;
		initMaterial = this.renderer.material;
		revealMaterial = initMaterial;

		if (!isDistractor) {
			initMaterial = this.renderer.material;
			revealMaterial = new Material (initMaterial);
			revealMaterial.color = Util.randomColorFromSet(colorSet);
		}

		timedReveal (3.5f);
	}
	
	// Update is called once per frame
	void Update () {
		//if (!isDistractor) {
			updateCloak();
		//}
	}

	private void updateCloak () {
		if (revealTicker > 0) {
			revealTicker -= Time.deltaTime;
			renderer.material = revealMaterial;
		}
		else {
			renderer.material = initMaterial;
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

	void OnMouseDown () {
		// Verify in radius
		var selectionRadius = player.GetComponentInChildren<SphereCollider> ();
		var radius = selectionRadius.gameObject.transform.localScale.x / 2.0f;
		var dist = transform.position - selectionRadius.gameObject.transform.position;
		Debug.Log ("Radius: " + radius);
		Debug.Log ("Distance: " + dist.magnitude);

		if (dist.magnitude > radius) {
			Debug.Log("Not in selection radius");
			return;
		}

		//TODO: MOVE SCORING AND VERIFICATION TO GAME MANAGER OR NEW SCRIPT
		if (player.currentColor () == revealMaterial.color) {
			player.AddToScore(100, !isDistractor);
		} else {
			Debug.Log ("WRONG");
			player.AddToScore (-100, !isDistractor);
		}
		gameObject.SetActive(false);
	}
}
