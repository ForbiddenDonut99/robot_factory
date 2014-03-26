using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for list

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
	GameObject powerupCompass;
	GameObject powerupSpring;
	GameObject guard;

	List<GameObject> powerupList = new List<GameObject>();
	ArrayList availablePowerUpTypes = new ArrayList();
	float[] powerupTypeChance = new float[5];
	
	bool flashLightGenerated = false;
	bool compassGenerated = false;
	
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
	GameObject machinePreFab;
	GameObject shelfPreFab;
	GameObject node;
	public float furnitureChance = 45f;
	public float machineRoomChance = 20f;
	
	// Use this for initialization
	void Start () {
		// load prefabs
		room = (GameObject)Resources.Load("Room");
		doorBlocker = (GameObject)Resources.Load("DoorBlocker");
		endBlocker = (GameObject)Resources.Load("EndBlocker");
		powerupWheels = (GameObject)Resources.Load("Powerup-Wheels");
		powerupFlashlight = (GameObject)Resources.Load("Powerup-Flashlight");
		powerupStungun = (GameObject)Resources.Load("Powerup-Stungun");
		powerupCompass = (GameObject)Resources.Load("Powerup-Compass");
		powerupSpring = (GameObject)Resources.Load("Powerup-Spring");
		guard = (GameObject)Resources.Load("Guard");
		
		player = (GameObject)Resources.Load("PlayerRobot");
		conveyorBelt = (GameObject)Resources.Load("ConveyorBelt");
		escapePad = (GameObject)Resources.Load("EscapePad");
		
		cubiclePreFab = (GameObject)Resources.Load("Interior_Cubicle");
		machinePreFab = (GameObject)Resources.Load("Interior_Machine");
		shelfPreFab = (GameObject)Resources.Load("Interior_Shelf");
		node = (GameObject)Resources.Load("Node");

		// powerup chance setting
		availablePowerUpTypes.Add(PowerUp.POWERUPTYPECOMPASS);
		availablePowerUpTypes.Add(PowerUp.POWERUPTYPESCOPE);
		availablePowerUpTypes.Add(PowerUp.POWERUPTYPESPRING);
		availablePowerUpTypes.Add(PowerUp.POWERUPTYPESTUNGUN);
		availablePowerUpTypes.Add(PowerUp.POWERUPTYPEWHEEL);

		powerupTypeChance[PowerUp.POWERUPTYPECOMPASS] = 1f;
		powerupTypeChance[PowerUp.POWERUPTYPESCOPE] = 1f;
		powerupTypeChance[PowerUp.POWERUPTYPESPRING] = 1f;
		powerupTypeChance[PowerUp.POWERUPTYPESTUNGUN] = 1f;
		powerupTypeChance[PowerUp.POWERUPTYPEWHEEL] = 1f;

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
						Instantiate(conveyorBelt, new Vector3(-12f+i*roomWidth,-1f,j*roomWidth), Quaternion.Euler(new Vector3(-90f,0f,0f)));
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

	public void PowerUpSwap(int maxedPowerupType){
		availablePowerUpTypes.Remove(maxedPowerupType);
		List<GameObject> toAdd = new List<GameObject>();
		List<GameObject> toRemove = new List<GameObject>();
		foreach(GameObject power in powerupList){
			if(power != null && power.transform.GetComponent<PowerUp>().PowerUpType == maxedPowerupType){
				toAdd.Add(GeneratePowerUp(power.transform.position));
				toRemove.Add(power);
			}
		}
		powerupList.AddRange(toAdd);
		foreach(GameObject power in toRemove){
			Destroy(power);
		}
	}

	public void removePowerup(GameObject powerup){
		powerupList.Remove(powerup);
	}
	
	GameObject GeneratePowerUp(Vector3 position){
		int powerType = 0;

		float totalChance = 0f;
		foreach(int p in availablePowerUpTypes){
			totalChance += powerupTypeChance[p];
		}

		float rnd = Random.Range(0f,totalChance);
		foreach(int p in availablePowerUpTypes){
			if(rnd <= powerupTypeChance[p]){
				powerType = p;
				break;
			} else{
				rnd -= powerupTypeChance[p];
			}
		}

		position = new Vector3(position.x, position.y, position.z);
		GameObject powerup = null;
		switch(powerType){
		case PowerUp.POWERUPTYPEWHEEL:
			powerup = (GameObject)Instantiate(powerupWheels, new Vector3(position.x, position.y, position.z), Quaternion.Euler(new Vector3(-90f,0f,0f)));
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			powerup.GetComponent<PowerUp>().PowerUpValue = (float)Random.Range(2,4);
			break;
		case PowerUp.POWERUPTYPESTUNGUN:
			powerup = (GameObject)Instantiate(powerupStungun, new Vector3(position.x, position.y + 0.05f, position.z), Quaternion.identity);
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			powerup.GetComponent<PowerUp>().PowerUpValue = (float)Random.Range(1,4);
			break;
		case PowerUp.POWERUPTYPESCOPE:
			flashLightGenerated = true;
			powerup = (GameObject)Instantiate(powerupFlashlight, new Vector3(position.x, position.y + 0.15f, position.z), Quaternion.identity);
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			break;
		case PowerUp.POWERUPTYPECOMPASS:
			compassGenerated = true;
			powerup = (GameObject)Instantiate(powerupCompass, new Vector3(position.x, position.y + 0.15f, position.z), Quaternion.Euler(new Vector3(-90f,0f,0f)));
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			break;
		case PowerUp.POWERUPTYPESPRING:
			powerup = (GameObject)Instantiate(powerupSpring, new Vector3(position.x, position.y + 0.15f, position.z), Quaternion.Euler(new Vector3(-90f,0f,0f)));
			powerup.GetComponent<PowerUp>().PowerUpType = powerType;
			break;
		}
		return powerup;
	}

	void GenerateRoomStuff(int roomType, Vector3 roomCenter){
		if (roomType == 0){
			/* decide interior type
			 * 0: office
			 * 1: machine room
			 */ 
			int interiorType = 0;
			if (Random.Range(0f,100f) <= machineRoomChance){
				interiorType = 1;
			}

			// room location vars
			float relX = roomCenter.x;
			float relY = roomCenter.y;
			float relZ = roomCenter.z;
			int rotation;

			// Double for loop to make a grid of nodes or furniture.
			for(float x = (relX-12);x<=(relX+12);x+=4){
				for(float z = (relZ-12);z<=(relZ+12);z+=4){
					rotation = (Random.Range (0,4)*90);
					if(x != relX && z != relZ){
						if (Random.Range(0f,100f) <= furnitureChance){
							switch(interiorType){
								case 0:{
									if (Random.Range(0f,100f) <= 90f){
										Instantiate(cubiclePreFab, new Vector3(x,relY,z), Quaternion.Euler(new Vector3(270f, rotation, 0)));
									} else{
										Instantiate(shelfPreFab, new Vector3(x,relY,z), Quaternion.Euler(new Vector3(270f, rotation, 0)));
									}
									break;
								}
								case 1:{
									Instantiate(machinePreFab, new Vector3(x,relY,z), Quaternion.Euler(new Vector3(270f, rotation, 0)));
									break;
								}
							}
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
			powerupList.Add(GeneratePowerUp(new Vector3(28f*xMultiplier+roomCenter.x,0f,28f*zMultiplier+roomCenter.z)));
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