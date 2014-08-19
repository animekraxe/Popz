using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public float speed = 0.0007f;
	private float pos = 0f;

	// Update is called once per frame
	void Update () {
		pos -= speed;
		if (pos < 0f) {
			pos += 1.0f;
		}
		renderer.material.mainTextureOffset = new Vector2 (pos, 0);
	}
}
