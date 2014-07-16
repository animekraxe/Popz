using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public float speed = 1f;

	void Update () {
		transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
	}



}
