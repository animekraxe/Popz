using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Bounds bounds = GetComponent<SpriteRenderer> ().sprite.bounds;
		Vector3 middleBottom = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth/2, 0f, 0f));
		transform.position = new Vector3 (middleBottom.x, middleBottom.y + bounds.extents.y * transform.localScale.y, -9f);
	}

}
