using UnityEngine;
using System.Collections;

public class ColdShower : MonoBehaviour {

	Vector3 record = new Vector3(0,0,0);

	void Start (){
		StartCoroutine (coldShower());
	}
	/*void Update(){
		if (transform.position == record){
			gameObject.GetComponent<TrialPatrol>().state = "MoveToNode";
			gameObject.GetComponent<TrialPatrol>().targetNode = gameObject.GetComponent<TrialPatrol>().lastNode;
			Debug.Log ("No Homo, bro.");
		}
		StartCoroutine (delayTracking());
	}*/

	IEnumerator coldShower(){
		for(;;){
			if (transform.position == record){
				gameObject.GetComponent<TrialPatrol>().state = "MoveToNode";
				gameObject.GetComponent<TrialPatrol>().targetNode = gameObject.GetComponent<TrialPatrol>().lastNode;
				Debug.Log ("No Homo, bro.");
			}
			record = transform.position;
			Debug.Log ("Position Updated!");
			yield return new WaitForSeconds(1.5f);
		}
	}

	/*IEnumerator delayTracking(){
		yield return new WaitForSeconds(2);
		record = transform.position;
		Debug.Log ("waited");
		yield return new WaitForSeconds(2);
	}*/

	/*void OnTriggerEnter(Collider other){
		if (other.tag == "Guard"){
			other.GetComponent<TrialPatrol>().targetNode = other.GetComponent<TrialPatrol>().lastNode;
			other.GetComponent<TrialPatrol>().state = "FindNode";
			Debug.Log ("No homo, bro.");
		}
	}*/

}