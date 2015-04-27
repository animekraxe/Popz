using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool jumpEnabled = true;
	public float jumpingSpeed = 5f;
	public float runningSpeed = 1.7f;

	public bool canRun = true;
	public bool canJump = true;

	// For testing purposes
	public bool canDoubleJump = true;

	private float screenBottom;
	private PatternLevelManager levelManager;
	// Use this for initialization
	void Start() {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<PatternLevelManager> ();
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
		Vector3 pos = bottomLeft + grid.GridToWorld (1, 2);
		transform.position = pos;
		screenBottom = bottomLeft.y - 10f;
	}
	
	// Update is called once per frame
	void Update () {
		if (jumpEnabled && canJump && Input.GetKeyDown ("space")){
			Jump ();
		}
		if (canRun) {
			transform.Translate(new Vector3(runningSpeed * Time.deltaTime, 0f, 0f));
		}
		UpdateTouch ();
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Ground")) {
			foreach (ContactPoint2D cp in col.contacts) {
				if (cp.normal.y == 1) {
					canJump = true;
				}
			}
		}
		else if (col.gameObject.tag.Equals("Hill")) {
			foreach (ContactPoint2D cp in col.contacts) {
				if (cp.normal.x == -1) {
					canRun = false;
				}
				else if (cp.normal.y == 1) {
					canJump = true;
				}
			}
		}
	}
	
	void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Ground")) {
			if (!canDoubleJump) {
				canJump = false;
			}
		}
		else if (col.gameObject.tag.Equals("Hill")) {
			foreach (ContactPoint2D cp in col.contacts) {
				if (cp.normal.x == -1) {
					canRun = true;
				}
				else if (cp.normal.y == 1) {
					canJump = false;
				}
			}
		}
	}

	public bool IsRunning { get { return canRun; } } 

	void UpdateTouch () {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Jump ();
			}
		}
	}

	void Jump () {
		//rigidbody2D.velocity += new Vector2(0, jumpingSpeed);
		if (GetComponent<Rigidbody2D>().velocity.y <= 0) {
			GetComponent<Rigidbody2D>().AddForce (Vector2.up * jumpingSpeed);
		}
	}

	void OnSwipeUp () {
		Jump ();
	}
}
