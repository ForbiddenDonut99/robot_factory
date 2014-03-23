using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public static bool fullscreen = true;
	public static int width = 1280;
	public static int height = 1024;
	public static float sfx = 1f;
	int state = 0;
	string ratio = "(5:4)";
	public GameObject Line1;
	public GameObject Line2;
	public GameObject Line3;
	public GameObject Line4;
	public GameObject resolution;
	public GameObject Fullscreen;
	public GameObject Volume;
	public Material whiteText;
	public Material yellowText;

	void Update () {

		//Move the selector.
		if(Input.GetKeyDown(KeyCode.DownArrow)&& state < 3){
			state++;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow) && state > 0){
			state--;
		}

		//Update the lines pertaining to changeable options.
		resolution.GetComponent<TextMesh>().text = width + "*" + height + "  " + ratio;
		Volume.GetComponent<TextMesh>().text = (sfx*10).ToString("#.")+"0%";
		if(fullscreen){
			Fullscreen.GetComponent<TextMesh>().text = "[x]";
		}
		else{
			Fullscreen.GetComponent<TextMesh>().text = "[ ]";
		}

		//Shit happens when you hit da buttons.
		if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)){
			switch(state){
			case 0:
				switch(width){
				case 1920:
					width = 640;
					height = 480;
					ratio = "(4:3)";
					break;
				case 640:
					width = 800;
					height = 600;
					ratio = "(4:3)";
					break;
				case 800:
					width = 1024;
					height = 768;
					ratio = "(4:3)";
					break;
				case 1024:
					width = 1280;
					height = 1024;
					ratio = "(5:4)";
					break;
				case 1280:
					width = 1440;
					height = 900;
					ratio = "(16:10)";
					break;
				case 1440:
					width = 1680;
					height = 1050;
					ratio = "(16:10)";
					break;
				case 1680:
					width = 1600;
					height = 900;
					ratio = "(16:9)";
					break;
				case 1600:
					width = 1920;
					height = 1080;
					ratio = "(16:9)";
					break;
				}
				break;

			case 1:
				if(fullscreen){
					fullscreen = false;
				}
				else{
					fullscreen = true;
				}
				break;
				
			case 2:
				if(sfx >= 1f){
					sfx = 0f;
				}
				else{
					sfx += 0.1f;
				}
				break;
				
			case 3:
				Screen.SetResolution(width, height, fullscreen);
				Application.LoadLevel("MainMenu");
				break;
			}
		}

		//Switch the text color to indicate selection.
		switch(state){
		case 0:
			Line1.renderer.material = yellowText;
			resolution.renderer.material = yellowText;
			Line2.renderer.material = whiteText;
			Fullscreen.renderer.material = whiteText;
			Line3.renderer.material = whiteText;
			Volume.renderer.material = whiteText;
			Line4.renderer.material = whiteText;
			break;
		case 1:
			Line1.renderer.material = whiteText;
			resolution.renderer.material = whiteText;
			Line2.renderer.material = yellowText;
			Fullscreen.renderer.material = yellowText;
			Line3.renderer.material = whiteText;
			Volume.renderer.material = whiteText;
			Line4.renderer.material = whiteText;
			break;
		case 2:
			Line1.renderer.material = whiteText;
			resolution.renderer.material = whiteText;
			Line2.renderer.material = whiteText;
			Fullscreen.renderer.material = whiteText;
			Line3.renderer.material = yellowText;
			Volume.renderer.material = yellowText;
			Line4.renderer.material = whiteText;
			break;
		case 3:
			Line1.renderer.material = whiteText;
			resolution.renderer.material = whiteText;
			Line2.renderer.material = whiteText;
			Fullscreen.renderer.material = whiteText;
			Line3.renderer.material = whiteText;
			Volume.renderer.material = whiteText;
			Line4.renderer.material = yellowText;
			break;
		}
	}
	
}
