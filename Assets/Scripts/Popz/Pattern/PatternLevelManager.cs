using UnityEngine;
using System.Collections;

public class PatternLevelManager : MonoBehaviour {

	public int numLives = 3; // Number of mistakes player can make before pattern length is decremented
	public int numHints = 3;
	public int numRounds = 5; // Number of rounds player must complete before pattern length is incremented
	public int patternLength = 2; // Number of collectibles in the pattern
	private int livesPerCollection;
	private int roundsPerCollection;

	public int maxPatternLength;
	private Pattern pattern;
	private Player player;

	// Use this for initialization
	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		maxPatternLength = grid.numCellsX - 1;
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		roundsPerCollection = numRounds;
		livesPerCollection = numLives;
	}
	
	// Update is called once per frame
	void Update () {
		if (pattern.patternCount == 0) {
			if (numRounds > 1) {
				numRounds--;
			}
			else {
				numLives = livesPerCollection;
				numRounds = roundsPerCollection;
				if (patternLength < maxPatternLength) {
					patternLength++;
				}
			}
			pattern.GeneratePattern(patternLength);
		}

		// Skips and generates new pattern is "s" is pressed
		if (Input.GetKeyDown ("s")) {
			pattern.GeneratePattern(patternLength);
		}
		if (Input.GetKeyDown ("h") && numHints > 0 && !pattern.display) {
			pattern.RevealPattern();
			numHints--;
		}
	}

	public void FailedPattern () {
		if (numLives > 1) {
			numLives--;
			pattern.RevealPattern ();
		}
		else {
			numRounds = roundsPerCollection;
			numLives = livesPerCollection;
			if (patternLength > 2) {
				patternLength--;
			}
			pattern.GeneratePattern(patternLength);
		}
	}
}
