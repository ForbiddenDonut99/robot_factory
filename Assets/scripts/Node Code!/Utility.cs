using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	public static float getRange(GameObject self, GameObject target){
		/*Compares and returns the distance between a given target and the calling
	 * instance.*/
		Vector3 position = self.transform.position;
		Vector3 difference = target.transform.position-position;
		float distance = difference.sqrMagnitude;
		return distance;
	}
	
	public static bool isInRange(GameObject self, GameObject target, float maxDistance){
		/*Compares the distance between a given target and the calling
	 * instance, returning true if this value is less than the max distance specified.*/
		Vector3 position = self.transform.position;
		Vector3 difference = target.transform.position-position;
		float distance = difference.sqrMagnitude;
		if(distance<maxDistance) return true;
		else return false;
	}
	public static GameObject selectNode(GameObject self, float distance, int counter, GameObject lastNode){
		//If counter is still low, choose a normal node. As counter increases,
		//chance of selecting a super node increases. Also, will try not to
		//go to the last node visited.
		bool selected = false;
		int superOptions;
		GameObject[] nodes;
		nodes = Sense.nearbyNodes(self, distance);
		if (nodes == null)return null;
		GameObject[] supers;
		supers = Sense.superNodes(nodes);
		int options = nodes.Length;
		if (supers != null){
			superOptions = supers.Length;
		}
		else superOptions = 0;

		float superWeight = counter/10; //10 is a magic number right now. This is the amount of steps to take within a room.

		GameObject result = lastNode;
		if(options == 1 && !lastNode.GetComponent<NodeScript>().isOff){
			Debug.Log ("Returned to " + lastNode+ " because we hit a dead end.");
			return lastNode; //if there's only one option, we're at a dead end, and we need to go back.
		}
		else if(Random.value < superWeight && supers != null){//try to find a super node instead
			while(!selected){
				int choice = Random.Range (0,superOptions);
				if(supers[choice] != lastNode){ // Makes sure the returned node isn't where we just came from.
					selected = true;			//Tries over and over until it comes up with one.
					result = supers[choice];
				}
				else selected = false;
			}
			Debug.Log ("We chose " + result+ " because we wanted a SuperNode.");
			return result;
		}
		else if(nodes != null){//Settle for a normal node, possibly a super node.
			while(!selected){
				int choice = Random.Range (0,options);
				if(nodes[choice] != lastNode){
					selected = true;
					result =  nodes[choice];
				}
				else selected = false;
			}
			Debug.Log (result+ " was chosen from "+options+" choices.");
			return result;
		}
		else return null; //If there aren't any nodes in range, return null instead of crashing.


	}
}
