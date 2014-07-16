using UnityEngine;
using System.Collections;

public class Jumper : MonoBehaviour {

	public float jumpVel = 5.0f;
	private bool onGround = true;
	
	void Update () {
		if (Input.GetKeyDown ("space") && onGround){
			Jump ();
		}
	}

	private void Jump () {
		//Debug.Log("Jump");
		rigidbody.velocity += new Vector3(0, jumpVel, 0);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag.Equals ("Ground")) {
			//Debug.Log("On ground");
			onGround = true;
		}
	}
	
	void OnCollisionExit (Collision col) {
		if (col.gameObject.tag.Equals ("Ground")) {
			//Debug.Log("Off ground");
			onGround = false;
		}
	}
}
