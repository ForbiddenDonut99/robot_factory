using UnityEngine;
using System.Collections;

public class BuildingGen : MonoBehaviour {

	public Transform room;
	public Transform doorBlock;

	// Use this for initialization
	void Start () {
		// mark places to generate a room as 1
		int[,] roomTypeArray = new int[4,4];
		int row = 0;
		int col = Random.Range(0,4);
		int rooms;
		while(row < 4){
			roomTypeArray[row,col] = 1;
			int rnd = Random.Range(0,3);
			switch(rnd){
			case 0:
				// go left
				if(col > 0){
					col--;
				}
				break;
			case 1:
				// go right
				if(col < 3){
					col++;
				}
				break;
			case 2:
				// go down
				row++;
				break;
			}
		}

		for(int i = 0; i < 4; i++){
			for(int j = 0; j < 4; j++){
//				Debug.Log(roomTypeArray[i,j]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
