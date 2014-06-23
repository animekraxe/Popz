using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float walkVel = 1.0f;
	private float walkMax = 2.0f;
	private float jumpVel = 5.0f;
	public bool isMovingRight, isMovingLeft, isJumping;

	private CameraRotationController camRotCtrl;

	// Use this for initialization
	void Start () {
		GameObject spinCtrl = GameObject.FindGameObjectWithTag("SpinController");
		camRotCtrl = spinCtrl.GetComponent<CameraRotationController>();
		isMovingRight = false;
		isMovingLeft = false;
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
	}
	
	// Update is called once per frame
	void Update () {
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
		if( rigidbody.isKinematic ) return;

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
		rigidbody.constraints = RigidbodyConstraints.None;
		switch(camRotCtrl.view) {
			case CameraRotationController.VIEW.FRONT:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
				if( rigidbody.velocity.x >= walkMax ) return;
				rigidbody.velocity += new Vector3(walkVel, 0, 0);
				break;
			case CameraRotationController.VIEW.RIGHT:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
				if( rigidbody.velocity.z >= walkMax ) return;
				rigidbody.velocity += new Vector3(0, 0, walkVel);
				break;
			case CameraRotationController.VIEW.BACK:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
				if( rigidbody.velocity.x <= -walkMax ) return;
				rigidbody.velocity += new Vector3(-walkVel, 0, 0);
				break;
			case CameraRotationController.VIEW.LEFT:
				rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
				if( rigidbody.velocity.z <= -walkMax ) return;
				rigidbody.velocity += new Vector3(0, 0, -walkVel);
				break;
		}
	}

	void moveLeft() {
		rigidbody.constraints = RigidbodyConstraints.None;
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
}
