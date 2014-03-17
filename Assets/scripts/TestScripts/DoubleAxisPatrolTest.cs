using UnityEngine;
using System.Collections;

public class DoubleAxisPatrolTest : MonoBehaviour {

	public float cubicleChance;
	public bool spawnSupers =false;
	float relX;
	float relY;
	float relZ;
	int rotation;
	GameObject cubiclePreFab;
	GameObject node;
	GameObject guard;
	// Use this for initialization
	void Start () {
		relX = transform.position.x;
		relY = transform.position.y;
		relZ = transform.position.z;
		cubiclePreFab = (GameObject)Resources.Load("Cubicle");
		node = (GameObject)Resources.Load("Node"); 
		guard = (GameObject)Resources.Load("Guard");
		//Office is super simple. Double for loop to make a grid of nodes or cubicles.
		for(float x = (relX-12);x<=(relX+12);x+=4){
			for(float z = (relZ-12);z<=(relZ+12);z+=4){
				rotation = (Random.Range (0,4)*90);
				if(x != relX && z != relZ){
					if (Random.Range(0,101) <= cubicleChance){
						Instantiate(cubiclePreFab, new Vector3(x,relY,z), Quaternion.Euler(new Vector3(270f, rotation, 0)));
					}
					else{
						Instantiate(node, new Vector3(x,(relY+0.5f),z), Quaternion.identity);
					}
				} else{
					GameObject supernode = (GameObject)Instantiate(node, new Vector3(x,(relY+0.5f),z), Quaternion.identity);
					supernode.GetComponent<NodeScript>().isSuper = spawnSupers;
				}
			}
		}
		Instantiate(guard, new Vector3(relX, relY+1.5f, relZ), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
