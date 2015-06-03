using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlantInfo : MonoBehaviour {

	//Plant info is a component of the plant
	//plant health slowly decreases until down to zero. which would be game over.
	public int health = 100;
	public int depleteBy = 2;
	public float depleteSeconds = 1.0f;
	public Slider healthSlider;
//	public bool alive = true;

	// Use this for initialization
	void Start () {
//		var bugColor = GameObject.FindGameObjectWithTag ("Bug");
		this.GetComponent<Renderer>().material.color = Util.randomColor ();
		Debug.Log (health);
		InvokeRepeating ("Depleting", depleteSeconds, depleteSeconds);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (health);
		healthSlider.value = health;

	}

	void Depleting(){
		if((health - depleteBy) < 100)
			health -= depleteBy;
		if (health <= 0) {
			health = 0;
			CancelInvoke ("Depleting");
		}
	}

	//TODO = COMPARE COLORS
	void OnCollisionEnter(Collision other){
//		Debug.Log ("Entered trigger");
		if (other.collider.tag == "Bug") {
			var otherBug = other.collider.gameObject;
			if (otherBug.GetComponent<CloakControl>().getRevealColor() == this.GetComponent<Renderer>().material.color){
//				Physics.IgnoreCollision( other.collider, this.collider);
//				this.depleteBy = -8;
				this.health += 25;
			}
			else{
//				this.depleteBy = 8;
				this.health -= 25;
			}
		}
	}
//		this.depleteBy = 2;
	void OnCollisionStay(Collision other){
		//		Debug.Log ("Exited Trigger");
		if (other.collider.tag == "Bug")
			this.depleteBy = -8;
	}


	void OnCollisionExit(Collision other){
//		Debug.Log ("Exited Trigger");
		if (other.collider.tag == "Bug")
			this.depleteBy = 2;
	}

}
