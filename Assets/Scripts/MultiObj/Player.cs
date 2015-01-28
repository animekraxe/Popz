using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

	public GameObject collector;

	// TODO: MOVE SCORING TO GAME MANAGER NOT PLAYER
	private int score;
	public int numCloakedObtained;

	// Player has this collector - Bool or Int? Can have multiple?
	// TODO: DECIDE IF BIN SYSTEM OR JUST USE COLORS
	private Bin rBin = new Bin();
	private Bin bBin = new Bin(); 
	private Bin gBin = new Bin();
	private Bin mBin = new Bin();
	private Bin yBin = new Bin();

	// Collection Bins State
	private List<Bin> collectionBins = new List<Bin> ();

	// Use this for initialization
	void Start () {
		score = 0;
		//AddToScore (0);

		rBin.construct (Color.red);
		bBin.construct (Color.blue);
		gBin.construct (Color.green);
		mBin.construct (Color.magenta);
		yBin.construct (Color.yellow);
	}
	
	// Update is called once per frame
	void Update () {
		updateBinSwitch ();
		updateCollector ();
	}

	public void regenCollectors (int num) {
		List<Bin> bins = new List<Bin> ();
		bins.Add (rBin);
		bins.Add (bBin);
		bins.Add (gBin);
		bins.Add (mBin);
		bins.Add (yBin);

		collectionBins.Clear ();
		var shuffled = bins.OrderBy (item => Random.value).ToList ();
		for (int i = 0; i < num; ++i) {
			shuffled[i].count += 1;
			collectionBins.Add (shuffled[i]);
		}
	}

	public void setCollectors (List<Color> colorSet) {
		collectionBins.Clear ();

		for (int i = 0; i < colorSet.Count; ++i) {
			if (colorSet[i] == Color.red) collectionBins.Add(rBin);
			if (colorSet[i] == Color.blue) collectionBins.Add(bBin);
			if (colorSet[i] == Color.green) collectionBins.Add(gBin);
			if (colorSet[i] == Color.magenta) collectionBins.Add(mBin);
			if (colorSet[i] == Color.yellow) collectionBins.Add(yBin);
		}
	}

	void updateBinSwitch () {
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			collectionBins.Add (collectionBins[0]);
			collectionBins.RemoveAt (0);
		}
	}

	void updateCollector () {
		var renderer = collector.GetComponentInChildren<Renderer> ();
		renderer.material.color = collectionBins[0].color;
	}

	public Color currentColor () {
		return collectionBins[0].color;
	}

	public void AddToScore (int num, bool isCloakedObj) {
		score += num;
		Debug.Log ("Score: " + score);

		if (isCloakedObj) {
			++numCloakedObtained;
		}
	}	

	public int NumCloakedObtained () {
		return numCloakedObtained;
	}

	public int GetScore () {
		return score;
	}
}

//			Debug.Log ("Bin Order: ");
//			for (int i = 0; i < collectionBins.Count; ++i) {
//				var name = "";
//				var color = collectionBins[i].color;
//				if (color == Color.red) name = "red";
//				else if (color == Color.blue) name = "blue";
//				else if (color == Color.green) name = "green";
//				else if (color == Color.magenta) name = "magenta";
//				else if (color == Color.yellow) name = "yellow";
//				Debug.Log (i + ". " + name);
//			}
