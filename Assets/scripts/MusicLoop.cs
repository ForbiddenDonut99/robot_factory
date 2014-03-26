using UnityEngine;
using System.Collections;

public class MusicLoop : MonoBehaviour {

	void Start () {
		audio.loop = true;
		audio.volume = OptionsMenu.sfx;
		audio.Play();
	}

	void Update(){
		audio.volume = OptionsMenu.sfx;
	}
}
