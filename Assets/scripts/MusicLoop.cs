using UnityEngine;
using System.Collections;

public class MusicLoop : MonoBehaviour {

	public AudioClip music;

	void Start () {
		audio.loop = true;
		audio.PlayOneShot(music, OptionsMenu.sfx);
	}
}
