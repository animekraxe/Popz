using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct FieldInfo {
	public float width;
	public float height;

	public float lowerX;
	public float lowerY;
	public float upperX;
	public float upperY;
}

public class Util : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static public Color randomColor () {
		List<Color> bins = new List<Color> ();
		bins.Add (Color.red);
		bins.Add (Color.blue);
		bins.Add (Color.green);
		bins.Add (Color.magenta);
		bins.Add (Color.yellow);

		return bins[Random.Range (0, 5)];
	}

	static public Color randomColorFromSet (List<Color> colorSet) {
		return colorSet [Random.Range (0, colorSet.Count)];
	}

	static public List<Color> genColorSet (int num) {
		List<Color> bins = new List<Color> ();
		bins.Add (Color.red);
		bins.Add (Color.blue);
		bins.Add (Color.green);
		bins.Add (Color.magenta);
		bins.Add (Color.yellow);
		
		return bins.OrderBy (item => Random.value).Take (num).ToList ();
	}

	static public FieldInfo getFieldInfo (GameObject field) {
		FieldInfo ret;
		var fieldRenderer = field.GetComponentInChildren<Renderer> ();
		ret.width = fieldRenderer.bounds.size.x;
		ret.height = fieldRenderer.bounds.size.y;

		ret.lowerX = field.transform.position.x;
		ret.lowerY = field.transform.position.y;
		ret.upperX = ret.lowerX + ret.width;
		ret.upperY = ret.lowerY + ret.height;
		return ret;
	}
}
