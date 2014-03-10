using UnityEngine;
using System.Collections;

public class OfficeGen : MonoBehaviour {

	public Transform cubiclePreFab;
	public GameObject node;
	public float cubicleChance = 28.57f;
	//Since the prefab procedure gives us a wonky location, this allows us to compensate.

	void Start () {
		float relX = gameObject.transform.position.x;
		float relY = gameObject.transform.position.y;
		float relZ = gameObject.transform.position.z;
		int rotation;
		//Office is super simple. Double for loop to make a grid of nodes or cubicles.
		for(float x = (relX-12);x<=(relX+12);x+=4){
			for(float z = (relZ-12);z<=(relZ+12);z+=4){
				rotation = (Random.Range (0,4)*90);
				if((x != relX)){
					if (Random.Range(0,101) <= cubicleChance){
						Instantiate(cubiclePreFab, new Vector3(x,relY,z), Quaternion.Euler(new Vector3(270f, rotation, 0)));
					}
					else{
						Instantiate(node, new Vector3(x,(relY+0.5f),z), Quaternion.identity);
					}
				}

			}
		}
	}
	
}
