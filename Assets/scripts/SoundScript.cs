﻿using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {
	public AudioClip sound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		audio.PlayOneShot (sound, OptionsMenu.sfx);
	}
}
