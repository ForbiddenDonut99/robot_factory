using UnityEngine;
using System.Collections;

public class BuildingGen : MonoBehaviour {

	/* ROOM TYPES:
	 * -3: Row Move Room that's also a starting room
	 * -2: Exit Room
	 * -1: Starting Room
	 * 0: None
	 * 1: Normal Room. Connects to rooms in the same row, and type 2,3 rooms in the previous row
	 * 2: Row Move Room. Connects to next row
	 */

	//general
	GameObject room;
	GameObject doorBlocker;
	GameObject endBlocker;
	GameObject powerupWheels;
	GameObject powerupFlashlight;
	GameObject powerupStungun;
	GameObject guard;

	bool flashLightGenerated = false;

	// starting room
	GameObject player;
	GameObject conveyorBelt;

	// exit room
	GameObject escapePad;

	float roomWidth = 68f; // size of the rooms
	public float powerupChance = 25f;
	public float guardChance = 80f;

	// within the room
	GameObject cubiclePreFab;
	GameObject node;
	public float cubicleChance = 28.57f;

	// Use this for initialization
	void Start () {
		// load prefabs
		room = (GameObject)Resources.Load("Room");
		doorBlocker = (GameObject)Resources.Load("DoorBlocker");
		endBlocker = (GameObject)Resources.Load("EndBlocker");
		powerupWheels = (GameObject)Resources.Load("Powerup-Wheels");
		powerupFlashlight = (GameObject)Resources.Load("Powerup-Flashlight");
		powerupStungun = (GameObject)Resources.Load("Powerup-Stungun");
		guard = (GameObject)Resources.Load("Guard");

		player = (GameObject)Resources.Load("PlayerRobot");
		conveyorBelt = (GameObject)Resources.Load("ConveyorBelt");
		escapePad = (GameObject)Resources.Load("EscapePad");

		cubiclePreFab = (GameObject)Resources.Load("Cubicle");
		node = (GameObject)Resources.Load("Node");

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
					roomTypeArray[row,col] = -3;
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
					if(i == 0 || (roomTypeArray[i-1,j] != 2 && roomTypeArray[i-1,j] != -3)){
						BlockOffWall(new Vector3(i*roomWidth,0f,j*roomWidth), -1, 0, roomType);
					} else if(roomType >= 0){
						// create the nodes connecting to the next room
						GenerateConnectingNodes(new Vector3(i*roomWidth,0f,j*roomWidth), -1, 0);
					}

					if(roomType != 2 && roomType != -3){
						BlockOffWall(new Vector3(i*roomWidth,0f,j*roomWidth), 1, 0, roomType);
					} else if(roomType >= 0){
						// create the nodes connecting to the next room
						GenerateConnectingNodes(new Vector3(i*roomWidth,0f,j*roomWidth), 1, 0);
					}

					if(j == 0 || roomTypeArray[i,j-1] == 0){
						BlockOffWall(new Vector3(i*roomWidth,0f,j*roomWidth), 0, -1, roomType);
					} else if(roomType >= 0){
						// create the nodes connecting to the next room
						GenerateConnectingNodes(new Vector3(i*roomWidth,0f,j*roomWidth), 0, -1);
					}

					if(j == 3 || roomTypeArray[i,j+1] == 0){
						BlockOffWall(new Vector3(i*roomWidth,0f,j*roomWidth), 0, 1, roomType);
					} else if(roomType >= 0){
						// create the nodes connecting to the next room
						GenerateConnectingNodes(new Vector3(i*roomWidth,0f,j*roomWidth), 0, 1);
					}



					if (roomType == -1 || roomType == -3){
						// generate player
						Instantiate(player, new Vector3(-12f+i*roomWidth,3f,j*roomWidth), Quaternion.Euler(new Vector3(0f,90f,0f)));
						Instantiate(conveyorBelt, new Vector3(-12f+i*roomWidth,0f,j*roomWidth), Quaternion.Euler(new Vector3(-90f,0f,0f)));
					} else if (roomType == -2){
						// generate escape pad
						Instantiate(escapePad, new Vector3(0f+i*roomWidth,0.2f,0f+j*roomWidth), Quaternion.identity);
					} else {
						// generate guards not on the first level and not next to starting room
						if((roomType == 1 || roomType == 2) && i != 0 && roomTypeArray[i-1,j] != -1 && roomTypeArray[i-1,j] != 3){
							if (Random.Range(0f,100f) <= guardChance){
								Instantiate(guard, new Vector3(i*roomWidth,3f,j*roomWidth), Quaternion.identity);
							}
						}
						if(roomType == 1 || roomType == 2){
							GenerateRoomStuff(0, new Vector3(i*roomWidth,0f,j*roomWidth));
						}
					}
				}
			}
		}
	}

	void GeneratePowerUp(Vector3 position){
		int powerType;
		float rnd = Random.Range(0f,100f);
		if (rnd < 50){
			powerType = 0;
		} else if (rnd < 60){
			powerType = 1;
		} else{
			if (!flashLightGenerated && rnd < 90){
				powerType = 1;
			} else {
				powerType = 2;
			}
		}
		position = new Vector3(position.x + Random.Range(-2f,2f), position.y, position.z + Random.Range(-2f,2f));
		GameObject powerup;
		switch(powerType){
		case 0:
			powerup = (GameObject)Instantiate(powerupWheels, new Vector3(position.x, position.y, position.z), Quaternion.Euler(new Vector3(-90f,0f,0f)));
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			powerup.GetComponent<PowerUp>().PowerUpValue = (float)Random.Range(2,5);
			break;
		case 1:
			powerup = (GameObject)Instantiate(powerupFlashlight, new Vector3(position.x, position.y + 0.15f, position.z), Quaternion.identity);
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			flashLightGenerated = true;
			powerup.GetComponent<PowerUp>().PowerUpValue = 1f;
			break;
		case 2:
			powerup = (GameObject)Instantiate(powerupStungun, new Vector3(position.x, position.y + 0.05f, position.z), Quaternion.identity);
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			powerup.GetComponent<PowerUp>().PowerUpValue = (float)Random.Range(1,4);
			break;
		}
	}

	void GenerateRoomStuff(int roomType, Vector3 roomCenter){
		if (roomType == 0){
			// office
			float relX = roomCenter.x;
			float relY = roomCenter.y;
			float relZ = roomCenter.z;
			int rotation;
			//Office is super simple. Double for loop to make a grid of nodes or cubicles.
			for(float x = (relX-12);x<=(relX+12);x+=4){
				for(float z = (relZ-12);z<=(relZ+12);z+=4){
					rotation = (Random.Range (0,4)*90);
					if(x != relX && z != relZ){
						if (Random.Range(0,101) <= cubicleChance){
							Instantiate(cubiclePreFab, new Vector3(x,relY,z), Quaternion.Euler(new Vector3(270f, rotation, 0)));
						} else{
							Instantiate(node, new Vector3(x,(relY+0.5f),z), Quaternion.identity);
						}
					} else{
						// set step reset at the exits
						if (x != relX - 12 && x != relX + 12 && z != relZ - 12 && z != relZ + 12){
							// make a node cross at the middle
							GameObject supernode = (GameObject)Instantiate(node, new Vector3(x,(relY+0.5f),z), Quaternion.identity);
							supernode.GetComponent<NodeScript>().isSuper = true;
						}
					}
				}
			}
		}
	}

	void BlockOffWall(Vector3 roomCenter, int xMultiplier, int zMultiplier, int roomType){
		// choose to block off the door and generate a powerup or block off entire wall
		if (Random.Range(0f,100f) <= powerupChance){
			Instantiate(endBlocker, new Vector3(32.5f*xMultiplier+roomCenter.x,4f,32.5f*zMultiplier+roomCenter.z), Quaternion.Euler(new Vector3(0f,90f*zMultiplier,0f)));
			GeneratePowerUp(new Vector3(28f*xMultiplier+roomCenter.x,0f,28f*zMultiplier+roomCenter.z));
		} else{
			Instantiate(doorBlocker, new Vector3(15f*xMultiplier+roomCenter.x,5f,15f*zMultiplier+roomCenter.z), Quaternion.Euler(new Vector3(0f,90f*zMultiplier,0f)));
		}
		if (roomType >= 0){
			Instantiate(node, new Vector3(12f*xMultiplier+roomCenter.x,0.5f,12f*zMultiplier+roomCenter.z), Quaternion.identity);
		}
	}
	
	void GenerateConnectingNodes(Vector3 roomCenter, int xMultiplier, int zMultiplier){
		// create the nodes connecting to the next room
		for(int n = 0; n < 4; n++){
			Instantiate(node, new Vector3(roomCenter.x + (20+n*4f) * xMultiplier,(0.5f),roomCenter.z + (20+n*4f) * zMultiplier), Quaternion.identity);
		}
		GameObject resetnode;
		resetnode = (GameObject)Instantiate(node, new Vector3(roomCenter.x + 12f * xMultiplier,0.5f,roomCenter.z + 12f * zMultiplier), Quaternion.identity);
		resetnode.GetComponent<NodeScript>().canReset = true;
		resetnode.GetComponent<NodeScript>().isSuper = true;
		resetnode = (GameObject)Instantiate(node, new Vector3(roomCenter.x + 16f * xMultiplier,0.5f,roomCenter.z + 16f * zMultiplier), Quaternion.identity);
		resetnode.GetComponent<NodeScript>().canReset = true;
		resetnode.GetComponent<NodeScript>().isSuper = true;
	}
}
