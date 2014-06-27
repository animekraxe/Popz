using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectiblesManager : MonoBehaviour {

	public Queue<Collectible> collectibles;

	// Use this for initialization
	void Start () {
		collectibles = new Queue<Collectible> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
