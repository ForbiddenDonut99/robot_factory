using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject StartGame;
	public GameObject Options;
	public GameObject Exit;
	public Material whiteText;
	public Material yellowText;
	public Material orangeText;
	
	void Update () {
		// reset color
		StartGame.renderer.material = whiteText;
		Options.renderer.material = whiteText;
		Exit.renderer.material = whiteText;
		
		// mouse controls
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit = new RaycastHit();
		if(Physics.Raycast(mouseRay, out rayHit, 100f)){
			GameObject option = rayHit.transform.gameObject;
			if(option.tag == "Option"){
				if(Input.GetMouseButtonDown(0)){
					// clicked on option
					if(option == StartGame){
						StartGame.renderer.material = orangeText;
						Application.LoadLevel("GameScene");
					} else if(option == Options){
						Options.renderer.material = yellowText;
						Application.LoadLevel("OptionsMenu");
					} else if(option == Exit){
						Exit.renderer.material = yellowText;
						Application.Quit();
					}
				} else{
					// highlight the option
					if(option == StartGame){
						StartGame.renderer.material = yellowText;
					} else if(option == Options){
						Options.renderer.material = yellowText;
					} else if(option == Exit){
						Exit.renderer.material = yellowText;
					}
				}
			}
		}
	}
}
