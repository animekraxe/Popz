using UnityEngine;
using System.Collections;

public class RotateModifier : MonoBehaviour {

	private CameraRotationController camRotCtrl;

	private ParticleSystem ps;
	private float initER, initSS;
	
	private Transform modCube;
	
	private bool collided;
	
	private float collisionAnimLength = 1.0f;
	private float collisionAnimTimer;
	
	private bool endRotationAnim;
	
	void Start () {
		camRotCtrl = GameObject.FindGameObjectWithTag("SpinController").GetComponent<CameraRotationController>();
		
		ps = this.gameObject.particleSystem;
		
		modCube = this.transform.parent.FindChild("ModifierCube");
		
		collided = false;
		collisionAnimTimer = 0.0f;
		
		endRotationAnim = false;
	}
	
	void Update() {
		
		if( collided ) {
			collisionAnimTimer += Time.deltaTime;
			if( collisionAnimTimer >= collisionAnimLength ) {
				endRotationAnim = true;
				ps.emissionRate = initER;
				ps.startSpeed = initSS;
				collided = false;
				collisionAnimTimer = 0.0f;
			}
			else {
				modCube.transform.Rotate(new Vector3(-120.0f * Time.deltaTime, -120.0f * Time.deltaTime, -120.0f * Time.deltaTime));
				ps.emissionRate = initER*3;
				ps.startSpeed = initSS*3;
			}
		}
		
		if( endRotationAnim ) {
			if( modCube.transform.rotation.eulerAngles.magnitude < 0.1f ) {
				modCube.transform.rotation = Quaternion.identity;
				endRotationAnim = false;
			}
			else {
				modCube.transform.rotation = Quaternion.Lerp(modCube.transform.rotation, Quaternion.identity, 0.05f);
			}
		}
		
	}
	
	void OnTriggerEnter(Collider col) {
		
		if( col.gameObject.tag == "Player" ) {
			
			initER = ps.emissionRate;
			initSS = ps.startSpeed;
			collided = true;

			camRotCtrl.performRotateRight();
		}
	}
}
