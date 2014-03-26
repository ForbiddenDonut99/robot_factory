using UnityEngine;
using System.Collections;

public class PersistentBeat : MonoBehaviour {

	private static PersistentBeat instance = null;
	public static PersistentBeat Instance{
		get{return instance;}
	}

	void Awake(){
		if(instance != null && instance != this){
			Destroy(this.gameObject);
			return;
		}
		else{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
