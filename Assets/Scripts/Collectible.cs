using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other) {
		if (!other.gameObject.tag.Equals ("Player")) { return; }



		Destroy (gameObject);
		
	}
}
