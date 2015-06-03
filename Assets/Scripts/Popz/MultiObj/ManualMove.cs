using UnityEngine;
using System.Collections;

public class ManualMove : MonoBehaviour {


	Vector3 currentMousePos;
	public Vector3 endMousePos;
	Vector3 direction;
	public bool isSwiped = false;

	float speed;
	public int bughealth = 50;
	public int depleteBy = 11;
	public float depleteSeconds = 1.0f;

//	public GameObject PlantObject;



//	private Vector3 flickStart;
//	private Vector3 flickEnd;
//	private Vector3 flickDirection;
//	private float flickSpeed = 2.0f;
//	private float flickTimer;
//
//	void OnMouseDown() {
//		flickStart = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//	}
//
//	void OnMouseExit() {
//		flickEnd = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//		flickDirection = (flickEnd - flickStart).Normalize ();
//		flickTimer = 10.if;
//	}
//
//	void UpdateForceOverTime() {
//		if (flickTimer <= 0.0f) {
//			return;
//		}
//
//		flickTimer -= Time.deltaTime;
//		transform.position += flickDirection * flickSpeed * Time.deltaTime;
//	}

//	void OnMouseDown(){
//		Debug.Log ("mouse down");
//		currentMousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
//
////		Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
////		currentMousePos = Camera.main.ScreenToWorldPoint (mousePos);
//	}
//
//	void OnMouseDrag(){
//		Debug.Log ("Old X is: " + currentMousePos.x);
//		Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
////		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
////		Debug.Log ("New X is: " + objPosition.x);
//		Debug.Log ("New X is: " + mousePosition.x);
//
//	}
//
//	void OnMouseUpAsButton(){
//		Debug.Log ("mouse lifted");
//		endMousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
//		isSwiped = true;
//	}

//
//
//	void OnMouseDrag(){
//		Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
//		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
//
//		transform.position = objPosition;
//	}

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
////		if (Input.touches [0].phase == TouchPhase.Moved) {
////			direction = Input.touches [0].deltaPosition.normalized;
////			speed = Input.touches [0].deltaPosition.magnitude / Input.touches [0].deltaTime;
////		}
//	}

//	void Depleting(){
//		bughealth -= depleteBy;
//		if (bughealth <= 0) {
////			PlantObject.GetComponent<PlantInfo>().alive = false;
//			bughealth = 0;
//			Destroy(this.gameObject);
//			CancelInvoke ("Depleting");
//		}
//	}
//
//	void OnCollisionEnter(Collision other){
//		if (other.collider.tag == "Plant") {
//			InvokeRepeating ("Depleting", depleteSeconds, depleteSeconds);
//		}
//
//	}
//
//	void OnCollisionExit(Collision other){
//		if (other.collider.tag == "Plant"){
//			CancelInvoke ("Depleting");
//		}
//	}


}
