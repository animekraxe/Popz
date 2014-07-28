using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {

	private bool cloaked;
	private Material initMaterial;
	private Mesh initMesh;

	private GameObject cloakObjectRef;
	private float cloakTicker;
	private float uncloakWaitTime = 2;

	private Vector3 target;
	private int currentTarget;
	private float speed = 5;

	private float moveTicker;
	private float moveWaitTime = 1f;

	// Use this for initialization
	void Start () {
		moveTicker = 0;
		cloakTicker = uncloakWaitTime;
		cloaked = true;

		initMaterial = this.renderer.material;
		initMesh = this.GetComponent<MeshFilter>().mesh;

		target = new Vector3(0,0,0);
		cloakObjectRef = GameObject.FindGameObjectWithTag("cloakObj");
		currentTarget = 5;
	}
	
	// Update is called once per frame
	void Update () {
		moveTicker += Time.deltaTime;
		cloakTicker += Time.deltaTime;

		moveUpdate();
		changeTarget();
		setCloak();
		checkReveal();
	}

	private void moveUpdate () {
		float step = speed * Time.deltaTime;
//		transform.position = Vector3.MoveTowards(transform.position, transform.forward, step);
		transform.Translate(Vector3.forward * step);
		Vector3 targetDir = target - transform.position;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	private void setCloak () {
		if (cloakTicker >= uncloakWaitTime)
			cloaked = true;

		if (cloaked) {
			this.GetComponent<MeshFilter>().mesh = cloakObjectRef.GetComponent<MeshFilter>().mesh;
			renderer.material = cloakObjectRef.renderer.material;
		}
		else {
			this.GetComponent<MeshFilter>().mesh = initMesh;
			renderer.material = initMaterial;
		}
	}

	private void changeTarget () {
		if (moveTicker >= moveWaitTime || transform.position == target) {
			float xMin = GameObject.FindGameObjectWithTag("bPoint1").transform.position.x;
			float xMax = GameObject.FindGameObjectWithTag("bPoint2").transform.position.x;
			float yMax = GameObject.FindGameObjectWithTag("bPoint3").transform.position.y;
			float yMin = GameObject.FindGameObjectWithTag("bPoint4").transform.position.y;

			Vector3 newTarget = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);

			target = newTarget;
			moveTicker = 0;
		}
	}

	private void checkReveal () {
		if (Input.anyKeyDown) {
			cloaked = false;
			cloakTicker = 0;
		}
	}

	void OnTriggerEnter(Collider other) {
		moveTicker = 0;
	}
}
