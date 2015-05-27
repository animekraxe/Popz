using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum ParallaxLayer {
	Background,
	Midground,
	Nearground,
};

public class ParallaxBackground : MonoBehaviour {

	// Images
	public List<Transform> images; // Images in sequence that make up the background

	// Parallax
	public ParallaxLayer layer;
	public float speed; // Note: Bigger speed means slower looking background
	private float offset;

	// Nearground Rotation
	private float nearRotateMin = -45.0f;
	private float nearRotateMax = 25.0f;

	private Transform cam;
	private Vector3 previousCamPos;

	void Awake () {
		// Get Camera Link
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		// Set offset to the size of one image so that there are no seams when arranging
		if (layer == ParallaxLayer.Background) {
			offset = images [0].gameObject.GetComponent<SpriteRenderer> ().bounds.size.x - 0.1f;
		} 
		// Set offset to screen width so layer items don't clutter the screen
		else {
			Vector3 start = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));
			Vector3 end = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, 0, 0));
			offset = end.x - start.x;
		}

		ArrangeImages ();

		// Start Camera Link
		previousCamPos = cam.position;
	}
	
	// Update is called once per frame
	void Update () {
		PollForOffscreenImage ();
		ParallaxMove ();

		// Update Camera link
		previousCamPos = cam.position;
	}

	void ArrangeImages () {
		foreach (Transform img in images) {
			RepositionBottomLeft(img);
		}

		for (int i = 1; i < images.Count; ++i) {
			Transform curr = images[i];
			Transform prev = images[i - 1];

			curr.position = prev.position + new Vector3(offset, 0, 0);
			FloorImage (curr);
			RotateImage (curr);
		}
	}

	void FloorImage (Transform img) {
		// Rotations affect height calculation. Set it to 0 and save it
		Vector3 angles = img.localEulerAngles;
		img.localEulerAngles = Vector3.zero;

		float height = img.gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		Vector3 floor = img.position;
		floor.y = this.transform.position.y + (height / 2.0f);
		img.position = floor;

		// Set original rotation
		img.localEulerAngles = angles;
	}

	void RotateImage (Transform img) {
		// Randomly rotate nearground images between 45 and -45 degrees on Z axis
		if (layer == ParallaxLayer.Nearground) {
			float rand = Random.Range (nearRotateMin, nearRotateMax);
			img.localEulerAngles = new Vector3 (0, 0, rand);
		}
	}

	void RepositionBottomLeft (Transform img) {
		img.position = this.transform.position;
		Vector3 size = img.gameObject.GetComponent<SpriteRenderer> ().bounds.size;
		size.z = 0;
		Vector3 centerOffset = size / 2.0f;
		img.Translate (centerOffset);
	}

	void RepeatImage (Transform img) {
		Transform end = images [images.Count - 1];
		images.Remove (img);
		img.position = end.position + new Vector3 (offset, 0, 0);
		FloorImage (img);
		RotateImage (img);
		img.gameObject.GetComponent<Background> ().isOffscreen = false;
		images.Add (img);
	}

	void PollForOffscreenImage () {
		List<Transform> toAdd = new List<Transform> ();
		foreach (Transform img in images) {
			if (img.gameObject.GetComponent<Background>().isOffscreen) {
				toAdd.Add (img);
			}
		}

		foreach (Transform img in toAdd) {
			RepeatImage(img);
		}
	}

	void ParallaxMove () {
		Vector3 move = cam.position - previousCamPos;
		move.y = 0;
		move.z = 0;
		move.Normalize ();
		move *= speed * Time.deltaTime;

		foreach (Transform image in images) {
			image.transform.position += move;
		}
	}
}
