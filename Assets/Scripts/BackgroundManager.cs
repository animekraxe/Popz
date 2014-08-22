using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour {

	public float speed = 1f;

	private Queue<Background> backgrounds;
	private Vector3 end;

	// Use this for initialization
	void Start () {
		backgrounds = new Queue<Background> ();
		GameObject[] bgs = GameObject.FindGameObjectsWithTag ("Background");
		Vector3 start = Camera.main.ScreenToWorldPoint (new Vector3 (0f, Camera.main.pixelHeight, 0f));
		start.z = 0f;

		Transform last = bgs [bgs.Length - 1].transform; 
		start.y -= last.GetComponent<SpriteRenderer> ().sprite.bounds.extents.y * last.transform.localScale.y;

		foreach (var b in bgs) {
			Vector3 spriteExtents = b.GetComponent<SpriteRenderer> ().sprite.bounds.extents;
			start += new Vector3 (spriteExtents.x * b.transform.localScale.x, 0f, 0f);;
			b.transform.position = start;
			start += new Vector3 (spriteExtents.x * b.transform.localScale.x, 0f, 0f);
			backgrounds.Enqueue(b.GetComponent <Background> ());
		}
		end = last.position + new Vector3 (last.GetComponent<SpriteRenderer> ().sprite.bounds.extents.x, 0f, 0f);

	}
	
	// Update is called once per frame
	void Update () {
		if (backgrounds.Peek ().isOffscreen) {
			Background offscreen = backgrounds.Dequeue();
			Vector3 spriteExtents = offscreen.GetComponent<SpriteRenderer> ().sprite.bounds.extents;
			end += new Vector3(spriteExtents.x * offscreen.transform.localScale.x, 0f, 0f);
			offscreen.transform.position = end;
			end += new Vector3(spriteExtents.x * offscreen.transform.localScale.x, 0f, 0f);
			offscreen.isOffscreen = false;
			backgrounds.Enqueue(offscreen);
		}

		Vector3 offset = new Vector3 (speed * Time.deltaTime, 0f, 0f);
		transform.Translate(offset);
		end += offset;
	}

	 
}
