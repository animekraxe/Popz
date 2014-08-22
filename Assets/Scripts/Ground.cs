using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 spriteExtents = GetComponent<SpriteRenderer> ().sprite.bounds.extents;
		Vector3 middleBottom = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth/2, 0f, 0f));
		transform.position = new Vector3 (middleBottom.x, middleBottom.y + spriteExtents.y * transform.localScale.y, 0f);
	}

}
