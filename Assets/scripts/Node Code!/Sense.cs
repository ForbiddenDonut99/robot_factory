using UnityEngine;
using System.Collections;

public class Sense : MonoBehaviour {

	public static GameObject[] nearbyNodes (GameObject self, float distance){
		//creates a list of NodeData for all nodes within a given radius.
		GameObject[] allNodes;
		allNodes = GameObject.FindGameObjectsWithTag("Node");
		if (allNodes == null || allNodes.Length == 0 || allNodes[0] == null){
			Debug.Log ("poop");
			return null;
		}
		int addIndex = 0;
		int resultSize = 0;
		GameObject[] tempList;
		tempList = new GameObject[allNodes.Length];
		foreach (GameObject node in allNodes){
			if(node != null && self != null && node.GetComponent<NodeScript>() != null){
				if(Utility.isInRange(self, node, distance)&& !node.GetComponent<NodeScript>().isOff){
					//only adds nodes you aren't standing on, and that are within range
					resultSize ++;
					tempList[addIndex] = node;
					addIndex ++;
				}
			}
		}
		if (resultSize != 0){
			GameObject[] resultList;
			resultList = new GameObject[resultSize];
			for(int i = 0; i<resultSize; i++){
				resultList[i] = tempList[i];
			}
			return resultList;
		}
		else return null;
	}
	public static GameObject[] exitNodes (GameObject[] nodes){
		//Extracts eligible super node game objects from a node data list.
		if(nodes == null)return null;
		GameObject[] tempList;
		int resultSize = 0;
		int addIndex = 0;
		tempList = new GameObject[nodes.Length];
		foreach(GameObject node in nodes){
			if (node.GetComponent<NodeScript>().isSuper){
				tempList[addIndex] = node;
				addIndex++;
				resultSize++;
			}
		}
		if(resultSize != 0){
			GameObject[] resultList;
			resultList = new GameObject[resultSize];
			for(int i = 0; i<resultSize;i++){
				resultList[i] = tempList[i];
			}
			return resultList;
		}
		else return null;
	}

	public static GameObject[] resetNodes (GameObject[] nodes){
		//Extracts eligible super node game objects from a node data list.
		if(nodes == null)return null;
		GameObject[] tempList;
		int resultSize = 0;
		int addIndex = 0;
		tempList = new GameObject[nodes.Length];
		foreach(GameObject node in nodes){
			if (node.GetComponent<NodeScript>().canReset){
				tempList[addIndex] = node;
				addIndex++;
				resultSize++;
			}
		}
		if(resultSize != 0){
			GameObject[] resultList;
			resultList = new GameObject[resultSize];
			for(int i = 0; i<resultSize;i++){
				resultList[i] = tempList[i];
			}
			return resultList;
		}
		else return null;
	}

	public static bool player (GameObject self, GameObject player){
		bool result = false;
		RaycastHit hit = new RaycastHit();
		CharacterController controller = player.GetComponent<CharacterController>();
		float detectSpeed = (controller.velocity.x)+(controller.velocity.y)+(controller.velocity.z);
		//Debug.Log (detectSpeed);

		//Basic, line of sight from far away.
		if(Physics.Raycast(self.transform.position, player.transform.position - self.transform.position, out hit, 20f)){ 
		   if(Vector3.Angle (player.transform.position - self.transform.position, self.transform.forward) < 30f){
				if (hit.transform.tag == "Player"){
				result = true;
				}
			}
		}
		//Based on being close and moving.
		if(Physics.Raycast(self.transform.position, player.transform.position - self.transform.position, out hit, 7.5f)){
			if (hit.transform.tag == "Player" && (detectSpeed >= 0.2f || detectSpeed <= -0.2f)) result = true;
		}
		return result;
	}
}
