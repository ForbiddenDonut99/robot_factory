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
		if (target == null)return true;
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
		int resetOptions;
		int superOptions;
		GameObject[] nodes;
		nodes = Sense.nearbyNodes(self, distance);
		if (nodes == null)return null;
		GameObject[] resets;
		resets = Sense.resetNodes(nodes);
		GameObject[] exits;
		exits = Sense.exitNodes(nodes);
		int options = nodes.Length;
		if (resets != null){
			resetOptions = exits.Length;
		}
		else resetOptions = 0;
		if (exits != null){
			superOptions = exits.Length;
		}
		else superOptions = 0;
		float superWeight = counter/10; //10 is a magic number right now. This is the amount of steps to take within a room.
		GameObject result = lastNode;

		if(lastNode.GetComponent<NodeScript>().isSuper && superOptions == 1 && !lastNode.GetComponent<NodeScript>().isOff && Random.value < superWeight && exits != null){
			Debug.Log("Returned to "+lastNode+" because we wanted to escape, but also dead end.");
			return lastNode; //if we wanna get out, but there's only one option, use this.
		}


		else if(options == 1 && !lastNode.GetComponent<NodeScript>().isOff){
			Debug.Log ("Returned to " + lastNode+ " because we hit a dead end.");
			return lastNode; //if there's only one option, we're at a dead end, and we need to go back.
		}

		else if(Random.value < superWeight && resets != null){//try to find a reset node instead
			while(!selected){
				int choice = Random.Range (0,superOptions);
				if(resets[choice] != lastNode){ // Makes sure the returned node isn't where we just came from.
					selected = true;			//Tries over and over until it comes up with one.
					result = resets[choice];
				}
				else selected = false;
			}
			Debug.Log ("We chose " + result+ " because we wanted a SuperNode.");
			return result;
		}

		else if(Random.value < superWeight && exits != null){//try to find an exit node instead
			while(!selected){
				int choice = Random.Range (0,superOptions);
				if(exits[choice] != lastNode){ // Makes sure the returned node isn't where we just came from.
					selected = true;			//Tries over and over until it comes up with one.
					result = exits[choice];
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
