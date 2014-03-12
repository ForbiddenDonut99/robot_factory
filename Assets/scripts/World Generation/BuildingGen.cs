using UnityEngine;
using System.Collections;

public class BuildingGen : MonoBehaviour {

	/* ROOM TYPES:
	 * -2: Exit Room
	 * -1: Starting Room
	 * 0: None
	 * 1: Normal Room. Connects to rooms in the same row, and type 2,3 rooms in the previous row
	 * 2: Row Move Room. Connects to next row
	 * 3: Row Move Room that's also a starting room
	 */

	//general
	GameObject room;
	GameObject doorBlocker;
	GameObject endBlocker;
	GameObject powerupCube;
	GameObject guard;

	// starting room
	GameObject player;
	GameObject conveyorBelt;

	// exit room
	GameObject escapePad;

	float roomWidth = 68f; // size of the rooms
	public float powerupChance = 25f;

	// Use this for initialization
	void Start () {
		// load prefabs
		room = (GameObject)Resources.Load("Room");
		doorBlocker = (GameObject)Resources.Load("DoorBlocker");
		endBlocker = (GameObject)Resources.Load("EndBlocker");
		powerupCube = (GameObject)Resources.Load("PowerupCube");
		guard = (GameObject)Resources.Load("Guard");

		player = (GameObject)Resources.Load("PlayerRobot");
		conveyorBelt = (GameObject)Resources.Load("ConveyorBelt");
		escapePad = (GameObject)Resources.Load("EscapePad");

		// mark places to generate a room as 1
		int[,] roomTypeArray = new int[4,4];
		int row = 0;
		int col = Random.Range(0,4);
		int rooms;

		roomTypeArray[row,col] = -1; // player starting room
		while(row < 4){
			if(roomTypeArray[row,col] == 0){
				roomTypeArray[row,col] = 1;
			}
			int rnd = Random.Range(0,5);
			switch(rnd){
			case 0:
			case 1:
				// go left
				if(col > 0){
					col--;
				}
				break;
			case 2:
			case 3:
				// go right
				if(col < 3){
					col++;
				}
				break;
			case 4:
				// go down
				if(roomTypeArray[row,col] == -1){
					roomTypeArray[row,col] = 3;
				} else{
					roomTypeArray[row,col] = 2;
				}
				row++;
				break;
			}
			if (row == 4){
				// exit
				roomTypeArray[row-1,col] = -2;
			}
		}

		for(int i = 0; i < 4; i++){
			for(int j = 0; j < 4; j++){
//				Debug.Log(roomTypeArray[i,j]);
				int roomType = roomTypeArray[i,j];
				if (roomType == 0){
					// do nothing
					continue;
				} else{
					// generate room
					Instantiate(room, new Vector3(i*roomWidth,0f,j*roomWidth), Quaternion.identity);


					// wall off dead ends. 
					// 2 connecting type 1 rooms are also walled off so there's no straight way to the end
					if(i == 0 || (roomTypeArray[i-1,j] != 2 && roomTypeArray[i-1,j] != 3)){
						// 1 in 6 chance of generating a powerup
						if (Random.Range(0f,100f) <= powerupChance){
							Instantiate(endBlocker, new Vector3(-32.5f+i*roomWidth,4f,0f+j*roomWidth), Quaternion.identity);
							GeneratePowerUp(new Vector3(-30f+i*roomWidth,0.5f,0f+j*roomWidth));
						} else{
							Instantiate(doorBlocker, new Vector3(-15f+i*roomWidth,5f,0f+j*roomWidth), Quaternion.identity);
						}
					}
					if(roomType != 2 && roomType != 3){
						if (Random.Range(0f,100f) <= powerupChance){
							Instantiate(endBlocker, new Vector3(32.5f+i*roomWidth,4f,0f+j*roomWidth), Quaternion.identity);
							GeneratePowerUp(new Vector3(30f+i*roomWidth,0.5f,0f+j*roomWidth));
						} else{
							Instantiate(doorBlocker, new Vector3(15f+i*roomWidth,5f,0f+j*roomWidth), Quaternion.identity);
						}
					}
					if(j == 0 || roomTypeArray[i,j-1] == 0){
						if (Random.Range(0f,100f) <= powerupChance){
							Instantiate(endBlocker, new Vector3(0f+i*roomWidth,4f,-32.5f+j*roomWidth), Quaternion.Euler(new Vector3(0f,90f,0f)));
							GeneratePowerUp(new Vector3(0f+i*roomWidth,0.5f,-30f+j*roomWidth));
						} else{
							Instantiate(doorBlocker, new Vector3(0f+i*roomWidth,5f,-15f+j*roomWidth), Quaternion.Euler(new Vector3(0f,90f,0f)));
						}
					}
					if(j == 3 || roomTypeArray[i,j+1] == 0){
						if (Random.Range(0f,100f) <= powerupChance){
							Instantiate(endBlocker, new Vector3(0f+i*roomWidth,4f,32.5f+j*roomWidth), Quaternion.Euler(new Vector3(0f,90f,0f)));
							GeneratePowerUp(new Vector3(0f+i*roomWidth,0.5f,30f+j*roomWidth));
						} else{
							Instantiate(doorBlocker, new Vector3(0f+i*roomWidth,5f,15f+j*roomWidth), Quaternion.Euler(new Vector3(0f,90f,0f)));
						}
					}

					if (roomType == -1 || roomType == 3){
						// generate player
						Instantiate(player, new Vector3(-12f+i*roomWidth,3f,j*roomWidth), Quaternion.Euler(new Vector3(0f,90f,0f)));
						Instantiate(conveyorBelt, new Vector3(-12f+i*roomWidth,0f,j*roomWidth), Quaternion.Euler(new Vector3(-90f,0f,0f)));
					} else if (roomType == -2){
						// generate escape pad
						Instantiate(escapePad, new Vector3(0f+i*roomWidth,0.2f,0f+j*roomWidth), Quaternion.identity);
					} else {
						// generate room stuff/nodes/

						// generate guards not on the first level and not next to starting room
						if(roomType == 1 && i != 0 && roomTypeArray[i-1,j] != -1 && roomTypeArray[i-1,j] != 3){
							Instantiate(guard, new Vector3(i*roomWidth,3f,j*roomWidth), Quaternion.identity);
						}
					}
				}
			}
		}
	}
	void GeneratePowerUp(Vector3 position){
		GameObject powerup = (GameObject)Instantiate(powerupCube, position, Quaternion.identity);
		PowerUp up = powerup.GetComponent<PowerUp>();
		float rnd = Random.Range(0f,100f);
		if (rnd < 40){
			up.PowerUpType = 0;
		} else if (rnd < 60){
			up.PowerUpType = 1;
		} else{
			up.PowerUpType = 2;
		}
		switch(up.PowerUpType){
		case 0:
			up.PowerUpValue = (float)Random.Range(2,6);
			break;
		case 1:
			up.PowerUpValue = 1f;
			break;
		case 2:
			up.PowerUpValue = (float)Random.Range(1,4);
			break;
		}
	}
	//	
	//	// Update is called once per frame
	//	void Update () {
//	
//	}
}
