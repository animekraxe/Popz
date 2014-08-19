using UnityEngine;
using System.Collections;

public class GoodCreatureSelect : MonoBehaviour {

	public ContainerButtons selection;
	public int selectionID;
	private int hasBeenSelected;

	// Use this for initialization
	void Start () {
		hasBeenSelected = 0;
	}
	
	// Update is called once per frame
	void Update () {
		checkSelectFlags();
	}

	void checkSelectFlags () {
		hasBeenSelected = selection.getSelection();
	}

	void OnMouseDown () {
		if(hasBeenSelected == selectionID) {
			Destroy(gameObject);
		}
	}
}
