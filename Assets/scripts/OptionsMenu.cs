using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {
	
	public static bool isFullscreen = false;
	public static int width = 1280;
	public static int height = 1024;
	public static float sfx = 1f;
	string ratio = "(5:4)";
	public GameObject Resolution;
	public GameObject Fullscreen;
	public GameObject Volume;
	public GameObject Back;
	public Material whiteText;
	public Material yellowText;
	public Material orangeText;

	void Update () {

		//Update the lines pertaining to changeable options.
		Resolution.GetComponent<TextMesh>().text = "Resolution      " + width + "*" + height + " " + ratio;
		Volume.GetComponent<TextMesh>().text = "Volume      " + (sfx*10).ToString("#.")+"0%";
		if(isFullscreen){
			Fullscreen.GetComponent<TextMesh>().text = "Fullscreen      [x]";
		}
		else{
			Fullscreen.GetComponent<TextMesh>().text = "Fullscreen      [  ]";
		}

		// reset color
		Resolution.renderer.material = whiteText;
		Fullscreen.renderer.material = whiteText;
		Volume.renderer.material = whiteText;
		Back.renderer.material = whiteText;

		// mouse controls
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit = new RaycastHit();
		if(Physics.Raycast(mouseRay, out rayHit, 100f)){
			GameObject option = rayHit.transform.gameObject;
			if(option.tag == "Option"){
				if(Input.GetMouseButtonDown(0)){
					// clicked on option
					if(option == Resolution){
						Resolution.renderer.material = yellowText;
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
					} else if(option == Fullscreen){
						Fullscreen.renderer.material = yellowText;
						isFullscreen = !isFullscreen;
					} else if(option == Volume){
						Volume.renderer.material = yellowText;
						if(sfx >= 1f){
							sfx = 0f;
						}
						else{
							sfx += 0.1f;
						}
					} else if(option == Back){
						Back.renderer.material = orangeText;
						Screen.SetResolution(width, height, isFullscreen);
						Application.LoadLevel("MainMenu");
					}
				} else{
					// highlight the option
					if(option == Resolution){
						Resolution.renderer.material = yellowText;
					} else if(option == Fullscreen){
						Fullscreen.renderer.material = yellowText;
					} else if(option == Volume){
						Volume.renderer.material = yellowText;
					} else if(option == Back){
						Back.renderer.material = yellowText;
					}
					
				}
			}
		}
	}
}
