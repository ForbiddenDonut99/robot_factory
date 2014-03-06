using UnityEngine;
using System.Collections;

public class Sense : MonoBehaviour {

	public static NodeData[] nearbyNodes (GameObject self, float distance){
		//creates a list of NodeData for all nodes within a given radius.
		GameObject[] allNodes;
		allNodes = GameObject.FindGameObjectsWithTag("Node");
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
			NodeData[] resultList;
			resultList = new NodeData[resultSize];
			for(int i = 0; i<resultSize; i++){
				resultList[i] = new NodeData();
			}
			for(int i = 0; i<resultSize; i++){
				resultList[i].node = tempList[i];
				resultList[i].isSuper = tempList[i].GetComponent<NodeScript>().isSuper;
			}
			return resultList;
		}
		else return null;
	}
	public static GameObject[] superNodes (NodeData[] nodes){
		//Extracts eligible super node game objects from a node data list.
		GameObject[] tempList;
		int resultSize = 0;
		int addIndex = 0;
		tempList = new GameObject[nodes.Length];
		foreach(NodeData node in nodes){
			if (node.isSuper){
				tempList[addIndex] = node.node;
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
