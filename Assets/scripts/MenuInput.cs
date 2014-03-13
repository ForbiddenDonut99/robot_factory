using UnityEngine;
using System.Collections;

public class MenuInput : MonoBehaviour {
	
	public string MainScene;
	public string OptionsMenu;
	public GameObject Line1;
	public GameObject Line2;
	public GameObject Line3;
	public GameObject Line4;
	public Material whiteText;
	public Material yellowText;
	int state = 0;
	
	void Start () {
		
	}
	
	void Update () {
		
		//Move the selector.
		if(Input.GetKeyDown(KeyCode.DownArrow)&& state < 3){
			state++;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow) && state > 1){
			state--;
		}
		
		//Do something once you select an option.
		if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)){
			switch(state){
			case 1:
				Application.LoadLevel(MainScene);
				break;
				
			case 2:
				Application.LoadLevel (OptionsMenu);
				break;
				
			case 3:
				Application.Quit();
				break;
			}
		}
		
		//Switch the text color to indicate selection.
		switch(state){
		case 0:
			Line1.renderer.material = yellowText;
			Line2.renderer.material = whiteText;
			Line3.renderer.material = whiteText;
			Line4.renderer.material = whiteText;
			break;
		case 1:
			Line1.renderer.material = whiteText;
			Line2.renderer.material = yellowText;
			Line3.renderer.material = whiteText;
			Line4.renderer.material = whiteText;
			break;
		case 2:
			Line1.renderer.material = whiteText;
			Line2.renderer.material = whiteText;
			Line3.renderer.material = yellowText;
			Line4.renderer.material = whiteText;
			break;
		case 3:
			Line1.renderer.material = whiteText;
			Line2.renderer.material = whiteText;
			Line3.renderer.material = whiteText;
			Line4.renderer.material = yellowText;
			break;
		}
	}
}
