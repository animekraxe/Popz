using UnityEngine;
using System.Collections;

public class GridSize : MonoBehaviour {

	public int numCellsX;
	public int numCellsY;
	private Grid grid;

	void Awake () {
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent <Grid> ();
		numCellsY = grid.numCellsY;
		ResizeSprite ();
	}

	void Start () {

	}


	private void ResizeSprite () {
		Vector3 spriteSize = this.gameObject.GetComponent <SpriteRenderer> ().sprite.bounds.size;
		float scaleY = (numCellsY * grid.cellSizeY) / spriteSize.y;
		Vector3 newScale = new Vector3 (scaleY, scaleY, this.gameObject.transform.localScale.z);
		this.gameObject.transform.localScale = newScale;
	}
}
