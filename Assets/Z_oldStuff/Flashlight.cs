using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {

	public Light flashlightSpotlight;
	public bool flashlightToggle;
	public bool preventRefire = false;

	void Start() {
		flashlightSpotlight.enabled = false;

		}

	void FixedUpdate() {

		if (Input.GetKey (KeyCode.F) && preventRefire == false) {
				StartCoroutine("FlashlightCoroutine");


				}

		}



	private IEnumerator FlashlightCoroutine() {
		if (flashlightToggle == false) {
					preventRefire = true;
					flashlightSpotlight.enabled = true;
					flashlightToggle = true;
					yield return new WaitForSeconds(1);
					preventRefire = false;
				} else {
					preventRefire = true;
					flashlightSpotlight.enabled = false;
					flashlightToggle = false;
					yield return new WaitForSeconds(1);
					preventRefire = false;
		}

		
	}

		




}
