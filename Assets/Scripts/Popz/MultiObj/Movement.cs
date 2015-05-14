using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	// Waypoint System Parameters 
	// TODO: CHANGE TO SIMPLE RECTANGLE OBJECT
	public GameObject field;
	public WayPoint wayPointReference;
	private WayPoint topBound;
	private WayPoint bottomBound;
	private WayPoint leftBound;
	private WayPoint rightBound;
	private WayPoint endPoint;

	// Movement Parameters
	public Vector3 target;
	public float pushSpeed = 0.0f;

	private float speed = 3.0f;
	private float moveTicker;
	private float moveWaitTime = 2f;
	private int direction;

	// Animation Flag
	private bool endAnimationPlay;

	// Use this for initialization
	void Start () {
		FieldInfo info = Util.getFieldInfo (field);

		var bottomLeftCorner = new Vector3 (info.lowerX, info.lowerY, 0);
		var upperRightCorner = new Vector3 (info.upperX, info.upperY, 0);

		var blank = wayPointReference;
		topBound = Instantiate (blank, upperRightCorner, transform.rotation) as WayPoint;
		bottomBound = Instantiate (blank, bottomLeftCorner, transform.rotation) as WayPoint;
		leftBound = Instantiate (blank, bottomLeftCorner, transform.rotation) as WayPoint;
		rightBound = Instantiate (blank, upperRightCorner, transform.rotation) as WayPoint;

		endAnimationPlay = false;
	}
	
	// Update is called once per frame
	void Update () {
		var bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		var upperRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1, 0));

		topBound.transform.position = upperRightCorner;
		bottomBound.transform.position = bottomLeftCorner;
		leftBound.transform.position = bottomLeftCorner;
		rightBound.transform.position = upperRightCorner;

		// Gentle velocity forward
		transform.position += new Vector3 (pushSpeed * Time.deltaTime, 0.0f, 0.0f);

		if (!endAnimationPlay) {
			// Update animations
			moveTicker += Time.deltaTime;
			moveUpdate();
			changeTarget();

			// Check bounds and zero out Z axis
			Vector3 zeroZ = new Vector3(transform.position.x, transform.position.y, 0);
			transform.position = zeroZ;
			checkInBounds();
		}
		else {
			target = endPoint.transform.position;
			moveUpdate();
		}
	}

	private void moveUpdate () {
		if (GetComponent<CloakControl> ().isRevealed ()) {
			return;
		}

		float step = speed * Time.deltaTime;
		transform.Translate(Vector3.forward * step);
		Vector3 targetDir = target - transform.position;
		targetDir.x *= direction;
		targetDir.y *= direction;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	private void changeTarget () {
		if (moveTicker >= moveWaitTime) {
			float xMin = leftBound.transform.position.x;
			float xMax = rightBound.transform.position.x;
			float yMax = topBound.transform.position.y;
			float yMin = bottomBound.transform.position.y;
			
			Vector3 newTarget = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);
			target = newTarget;
			moveTicker = 0;
			direction = 1;
		}
	}

	private void checkInBounds () {
		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
		float yMax = topBound.transform.position.y;
		float yMin = bottomBound.transform.position.y;

		/*
		if (transform.position.x < xMin || transform.position.x > xMax
		    || transform.position.y < yMin) {
			transform.position = new Vector3(xMin + 1, yMax - 1, 0);
			moveTicker = moveWaitTime;
			changeTarget();
			direction *= -1;
			//moveTicker = 0;
		}
		*/

		// Turn around before you hit the walls
		if (transform.position.x < xMin + 1 || transform.position.x > xMax - 1
		    || transform.position.y < yMin + 2 || transform.position.y > yMax - 1) {
			//transform.position = new Vector3(xMin + 1, yMax - 1, 0);
			moveTicker = moveWaitTime;
			changeTarget();
			transform.LookAt(target);
			//direction *= -1;
			//moveTicker = 0;
		}

		else if (transform.position.y > yMax) {
			transform.position = new Vector3(transform.position.x, yMax - 1, 0);
			direction *= -1;
			moveTicker = 0;
		}

	}

	void OnCollisionEnter(Collision collision) {
		direction *= -1;
		moveTicker = 0;
	}
}
