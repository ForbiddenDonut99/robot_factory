using UnityEngine;
using System.Collections;

public class ColdShower : MonoBehaviour {

	Vector3 record = new Vector3(0,0,0);

	void Start (){
		StartCoroutine (coldShower());
	}

	IEnumerator coldShower(){
		for(;;){
			if (transform.position == record){
				gameObject.GetComponent<TrialPatrol>().state = "MoveToNode";
				gameObject.GetComponent<TrialPatrol>().targetNode = gameObject.GetComponent<TrialPatrol>().lastNode;
			}
			record = transform.position;
			yield return new WaitForSeconds(0.2f);
		}
	}
}