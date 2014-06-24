using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float walkVel = 1.0f;	// velocity to apply to make the character move
	private float walkMax = 2.0f;	// max velocity to restrict character from moving too fast
	private float jumpVel = 5.0f;	// amount of velocity to apply to make the character jump
	public bool isMovingRight, isMovingLeft, isJumping;	// flags to interface w/ input controls

	private CameraRotationController camRotCtrl;	// variable to interface with CameraRotationController script

	void Start () {
	
		// init variables
		GameObject spinCtrl = GameObject.FindGameObjectWithTag("SpinController");
		camRotCtrl = spinCtrl.GetComponent<CameraRotationController>();
		isMovingRight = false;
		isMovingLeft = false;
		isJumping = false;
		
		// restrict the character from moving in the z-direction
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
	}
	
	void Update () {
	
		//===== Input Interface =====//
	
		if( Input.GetKey(KeyCode.D) ) {
			isMovingRight = true;
		}
		else {
			isMovingRight = false;
		}

		if( Input.GetKey(KeyCode.A) ) {
			isMovingLeft = true;
		}
		else {
			isMovingLeft = false;
		}

		if( Input.GetKeyDown(KeyCode.Space) ) {
			isJumping = true;
		}
		else {
			isJumping = false;
		}
	}

	void FixedUpdate() {
	
		// when the rigidbody is kinematic, it won't move, so exit FixedUpdate()
		if( rigidbody.isKinematic ) return;

		// otherwise, perform movements if input was given
		if( isMovingRight ) {
			moveRight();
		}

		if( isMovingLeft ) {
			moveLeft();
		}

		if( isJumping ) {
			rigidbody.velocity += new Vector3(0, jumpVel, 0);
		}
	}

	void moveRight() {
		switch(camRotCtrl.view) {	// movement is determined by the current view
			case CameraRotationController.VIEW.FRONT:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;	// lock z-movement
				if( rigidbody.velocity.x >= walkMax ) return;
				rigidbody.velocity += new Vector3(walkVel, 0, 0);	// move in +x-direction
				break;
			case CameraRotationController.VIEW.RIGHT:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionX;	// lock x-movement
				if( rigidbody.velocity.z >= walkMax ) return;
				rigidbody.velocity += new Vector3(0, 0, walkVel);	// move in +z-direction
				break;
			case CameraRotationController.VIEW.BACK:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;	// lock z-movement
				if( rigidbody.velocity.x <= -walkMax ) return;
				rigidbody.velocity += new Vector3(-walkVel, 0, 0);	// move in -x-direction
				break;
			case CameraRotationController.VIEW.LEFT:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionX;	// lock x-movement
				if( rigidbody.velocity.z <= -walkMax ) return;
				rigidbody.velocity += new Vector3(0, 0, -walkVel);	// move in -z-direction
				break;
		}
	}

	void moveLeft() {	// same as moveRight() just in the opposite direction
		switch(camRotCtrl.view) {
		case CameraRotationController.VIEW.FRONT:
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
			if( rigidbody.velocity.x <= -walkMax ) return;
			rigidbody.velocity += new Vector3(-walkVel, 0, 0);
			break;
		case CameraRotationController.VIEW.RIGHT:
			rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
			if( rigidbody.velocity.z <= -walkMax ) return;
			rigidbody.velocity += new Vector3(0, 0, -walkVel);
			break;
		case CameraRotationController.VIEW.BACK:
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
			if( rigidbody.velocity.x >= walkMax ) return;
			rigidbody.velocity += new Vector3(walkVel, 0, 0);
			break;
		case CameraRotationController.VIEW.LEFT:
			rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
			if( rigidbody.velocity.z >= walkMax ) return;
			rigidbody.velocity += new Vector3(0, 0, walkVel);
			break;
		}
	}

	void OnCollisionEnter(Collision col) {	// when colliding with a platform, set the platformID to that platform's ID
		if( col.gameObject.tag == "Platform" ) {
			camRotCtrl.platformID = col.gameObject.GetComponent<PlatformID>().ID;
		}
	}

}
