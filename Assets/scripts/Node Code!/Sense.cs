using UnityEngine;
using System.Collections;

public class Sense : MonoBehaviour {

	public static GameObject[] nearbyNodes (GameObject self, float distance){
		//creates a list of NodeData for all nodes within a given radius.
		GameObject[] allNodes;
		allNodes = GameObject.FindGameObjectsWithTag("Node");
		if (allNodes == null || allNodes[0] == null){
			Debug.Log ("poop");
			return null;
		}
		int addIndex = 0;
		int resultSize = 0;
		GameObject[] tempList;
		tempList = new GameObject[allNodes.Length];
		foreach (GameObject node in allNodes){
			if(Utility.isInRange(self, node, distance)&& !node.GetComponent<NodeScript>().isOff){
				//only adds nodes you aren't standing on, and that are within range
				resultSize ++;
				tempList[addIndex] = node;
				addIndex ++;
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
}
