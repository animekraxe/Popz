using UnityEngine;
using System.Collections;

public class ContainerButtons : MonoBehaviour {

	private int selection;
//	public Texture btnTexture;

	private int toolbarWidth = 300;
	private int toolbarHeight = 50;
	
	private string[] toolbarStrings = new string[] {"RED", "BLUE", "GREEN"};

	// Use this for initialization
	void Start () {
		selection = -1;
		toolbarStrings = new string[] {"RED", "BLUE", "GREEN"};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		selection = GUI.Toolbar(new Rect(Screen.width/2 - toolbarWidth/2, 
		                                Screen.height - toolbarHeight, toolbarWidth, toolbarHeight), 
		                        		selection, toolbarStrings);
	}

	public int getSelection () {
		return selection;
	}

}
