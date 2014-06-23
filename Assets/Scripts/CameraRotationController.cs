using UnityEngine;
using System.Collections;

public class CameraRotationController : MonoBehaviour {

	private float camRotationDuration = 1.0f;
	private float platformTransformDuration = 0.5f;

	private enum ROTATION { NONE = 0, RIGHT = 1, LEFT = 2 };
	private enum ROTATION_STATE { TRANSFORM = 0, ROTATE = 1, PROJECT = 2, DONE = 3 };
	private enum SUB_STATE { SETUP = 0, PERFORM = 1, COMPLETE = 2 };
	private ROTATION rotating;
	private ROTATION_STATE rState;
	private SUB_STATE subState;

	private bool doneRotation;

	public enum VIEW { FRONT = 0, RIGHT = 1, BACK = 2, LEFT = 3 };
	public VIEW view;

	private float t;
	public float oldRotation;
	public float newRotation;

	private GameObject[] platforms;
	private Vector3[] platformPos;
	private Vector3[] oldPlatformPos;
	private Vector3[] newPlatformPos;

	private GameObject player;
	private Vector3 newPlayerPos;
	public int platformID;

	// Use this for initialization
	void Start () {
		initGameObjects();
		initVars();
		projectPlatformsOnto2D();
		setPlatformsIn2D();
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown("right") && doneRotation ) {
			doneRotation = false;
			rotating = ROTATION.RIGHT;
			rState = ROTATION_STATE.TRANSFORM;
			subState = SUB_STATE.SETUP;
		}
		else if( Input.GetKeyDown("left") && doneRotation ) {
			doneRotation = false;
			rotating = ROTATION.LEFT;
			rState = ROTATION_STATE.TRANSFORM;
			subState = SUB_STATE.SETUP;
		}

		if(!doneRotation) {
			if( rotating == ROTATION.NONE ) {
				doneRotation = true;
			}
			else {
				rotationController();
			}
		}
		else {
			rotating = ROTATION.NONE;
		}
		
	}

	void initGameObjects() {
		player = GameObject.FindGameObjectWithTag("Player");
		
		platforms = GameObject.FindGameObjectsWithTag("Platform");
	}

	void initVars() {
		rotating = ROTATION.NONE;
		view = VIEW.FRONT;
		
		platformPos = new Vector3[platforms.Length];
		oldPlatformPos = new Vector3[platforms.Length];
		newPlatformPos = new Vector3[platforms.Length];
		int i = 0;
		foreach(GameObject platform in platforms) {
			platformPos[i] = platform.transform.position;
			platform.GetComponent<PlatformID>().ID = i;
			++i;
		}
	}

	void rotationController() {
		switch(rState) {
			case ROTATION_STATE.TRANSFORM:
				switch(subState) {
					case SUB_STATE.SETUP:
						pausePlayer();
						transformPlatformsBackTo3D();
						t = 0.0f;
						subState = SUB_STATE.PERFORM;
						break;
					case SUB_STATE.PERFORM:
						if( interpolatePlatforms() ) {
							subState = SUB_STATE.COMPLETE;
						}
						break;
					case SUB_STATE.COMPLETE:
						rState = ROTATION_STATE.ROTATE;
						subState = SUB_STATE.SETUP;
						break;
				}
				break;
			case ROTATION_STATE.ROTATE:
				switch(subState) {
					case SUB_STATE.SETUP:
						t = 0.0f;
						oldRotation = this.transform.eulerAngles.y;
						setNewRotation();
						subState = SUB_STATE.PERFORM;
						break;
					case SUB_STATE.PERFORM:
						if( interpolateCamera() ) {
							subState = SUB_STATE.COMPLETE;
						}
						break;
					case SUB_STATE.COMPLETE:
						setNewView();
						rState = ROTATION_STATE.PROJECT;
						subState = SUB_STATE.SETUP;
						break;
				}
				break;
			case ROTATION_STATE.PROJECT:
				switch(subState) {
					case SUB_STATE.SETUP:
						t = 0.0f;
						projectPlatformsOnto2D();
						subState = SUB_STATE.PERFORM;
						break;
					case SUB_STATE.PERFORM:
						if( interpolatePlatforms() ) {
							subState = SUB_STATE.COMPLETE;
						}
						break;
					case SUB_STATE.COMPLETE:
						rState = ROTATION_STATE.DONE;
						break;
				}
				break;
			case ROTATION_STATE.DONE:
				resumePlayer();
				doneRotation = true;
				break;
		}
	}

	void projectPlatformsOnto2D() {
		float newX, newZ;
		int i;
		switch(view) {
			case VIEW.FRONT:
			case VIEW.BACK:
				newZ = 0.0f;
				i = 0;
				foreach(Vector3 pos in platformPos) {
					oldPlatformPos[i] = pos;
					newPlatformPos[i] = pos;
					newPlatformPos[i].z = newZ;
					++i;
				}
				break;
			case VIEW.RIGHT:
			case VIEW.LEFT:
				newX = 0.0f;
				i = 0;
				foreach(Vector3 pos in platformPos) {
					oldPlatformPos[i] = pos;
					newPlatformPos[i] = pos;
					newPlatformPos[i].x = newX;
					++i;
				}
				break;
		}
	}

	void transformPlatformsBackTo3D() {
		int i = 0;
		foreach( Vector3 pos in platformPos) {
			oldPlatformPos[i] = newPlatformPos[i];
			newPlatformPos[i] = pos;
			++i;
		}
	}

	bool interpolatePlatforms() {
		t += Time.deltaTime / platformTransformDuration;
		int i = 0;
		if( t >= 1.0f ) {
			foreach(GameObject platform in platforms) {
				platform.transform.position = newPlatformPos[i];
				++i;
			}
			return true;
		}
		else {
			foreach(GameObject platform in platforms) {
				Vector3 newPos = oldPlatformPos[i] * (1.0f-t) + newPlatformPos[i] * t;
				platform.transform.position = newPos;
				++i;
			}
			return false;
		}
	}

	bool interpolateCamera() {
		t += Time.deltaTime / camRotationDuration;
		if( t >= 1.0f ) {
			Vector3 newAngle = this.transform.eulerAngles;
			newAngle.y = newRotation;
			this.transform.eulerAngles = newAngle;
			return true;
		}
		else {
			if( oldRotation == 0.0f && newRotation == 270.0f ) {
				Vector3 newAngle = this.transform.eulerAngles;
				if( t == 0 )
					newAngle.y = oldRotation;
				else
					newAngle.y = 360.0f * (1.0f-t) + newRotation * t;
				this.transform.eulerAngles = newAngle;
				return false;
			}
			else if( oldRotation == 270.0f && newRotation == 0.0f ) {
				Vector3 newAngle = this.transform.eulerAngles;
				newAngle.y = oldRotation * (1.0f-t) + 360.0f * t;
				this.transform.eulerAngles = newAngle;
				return false;
			}
			else {
				Vector3 newAngle = this.transform.eulerAngles;
				newAngle.y = oldRotation * (1.0f-t) + newRotation * t;
				this.transform.eulerAngles = newAngle;
				return false;
			}
		}
	}

	void setNewRotation() {
		switch(view) {
			case VIEW.FRONT:
				if( rotating == ROTATION.RIGHT )
					newRotation = 270.0f;
				else
					newRotation = 90.0f;
				break;
			case VIEW.RIGHT:
				if( rotating == ROTATION.RIGHT )
					newRotation = 180.0f;
				else
					newRotation = 0.0f;
				break;
			case VIEW.BACK:
				if( rotating == ROTATION.RIGHT )
					newRotation = 90.0f;
				else
					newRotation = 270.0f;
				break;
			case VIEW.LEFT:
				if( rotating == ROTATION.RIGHT )
					newRotation = 0.0f;
				else
					newRotation = 180.0f;
				break;
		}
	}

	void setNewView() {
		switch(view) {
			case VIEW.FRONT:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.RIGHT;
				else
					view = VIEW.LEFT;
				break;
			case VIEW.RIGHT:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.BACK;
				else
					view = VIEW.FRONT;
				break;
			case VIEW.BACK:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.LEFT;
				else
					view = VIEW.RIGHT;
				break;
			case VIEW.LEFT:
				if( rotating == ROTATION.RIGHT )
					view = VIEW.FRONT;
				else
					view = VIEW.BACK;
				break;
			}
	}

	void setPlatformsIn2D() {
		float newZ = 0.0f;
		Vector3 newPos = player.transform.position;
		newPos.z = newZ;
		player.transform.position = newPos;
		foreach(GameObject platform in platforms) {
			newPos = platform.transform.position;
			newPos.z = newZ;
			platform.transform.position = newPos;
		}
	}

	void pausePlayer() {
		player.rigidbody.constraints = RigidbodyConstraints.None;
		player.rigidbody.isKinematic = true;

		// Rotate with camera (works all the time, but does not achieve the desired mechanic we're going for)
		//player.transform.parent = this.transform;

		// Stay with the last platform landed on (the desired mechanic but has some issues when playing free-range mode)
		player.transform.parent = platforms[platformID].transform;
	}

	void resumePlayer() {
		newPlayerPos = player.transform.position;
		if( view == VIEW.LEFT || view == VIEW.RIGHT ) {
			newPlayerPos.x = 0.0f;
		}
		else {
			newPlayerPos.z = 0.0f;
		}
		player.transform.position = newPlayerPos;
		player.transform.parent = null;
		player.rigidbody.isKinematic = false;
	}

}
