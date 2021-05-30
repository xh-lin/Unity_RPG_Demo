using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Optimization for first level, disable non-neighbor rooms
    public GameObject room1;
    public GameObject hallway1;
    public GameObject room2;
    public GameObject hallway2;
    public GameObject room3;
    public GameObject hallway3;
    public GameObject room4;
    public GameObject hallway4;
    public GameObject room5;

    // gateway detects which way does player go
    public GatewayListener gateway1to1;  // between room1 and hallway1
    public GatewayListener gateway1to2;  // between hallway1 and room2
    public GatewayListener gateway2to2;
    public GatewayListener gateway2to3;
    public GatewayListener gateway3to3;
    public GatewayListener gateway3to4;
    public GatewayListener gateway4to4;
    public GatewayListener gateway4to5;

    enum PlayerLocation { 
        room1 , hallway1 , 
        room2 , hallway2 , 
        room3 , hallway3 , 
        room4 , hallway4 , 
        room5 }

    private PlayerLocation playerLocation;
    private PlayerLocation prevLocaton;
    private GameObject[] map;
    private GatewayListener[] gateways;
    

    private void Start()
    {
        playerLocation = PlayerLocation.room1;

        map = new GameObject[] {    // length = 9
        room1 , hallway1 ,
        room2 , hallway2 ,
        room3 , hallway3 ,
        room4 , hallway4 ,
        room5 };

        gateways = new GatewayListener[] {  // length = 8
            gateway1to1 , gateway1to2 ,
            gateway2to2 , gateway2to3 ,
            gateway3to3 , gateway3to4 ,
            gateway4to4 , gateway4to5 };

        for (int i = 2; i < 9; i++) // disable all rooms that are not neighbor to room1 at start
            map[i].SetActive(false);
    }

    private void Update()
    {
        prevLocaton = playerLocation;
        PlayerLocationUpdate();
        if (prevLocaton != playerLocation) ToggleRoomUpdate();
        
    }

    /*
     *  if in room 1 (idx0)      Check:  gateway1to1 (idx0)
     *  if in hallway1 (idx1)    Check:  gateway1to1 , gateway1to2   (idx0, 1)
     *  if in room2 (idx2)       Check:  gateway1to2 , gateway2to2   (idx1, 2)
     *  ...
     *  if in room5 (idx8)       Check:  gateway4to5 (idx7)
     */
    void PlayerLocationUpdate()
    {
        int nextGateway = (int)playerLocation;
        int prevGateway = nextGateway - 1;
        if (nextGateway < gateways.Length && gateways[nextGateway].isPlayerPassed) 
            playerLocation += 1;
        else if (prevGateway >= 0 && !gateways[prevGateway].isPlayerPassed) 
            playerLocation -= 1;
    }

    void ToggleRoomUpdate()
    {
        if (prevLocaton == playerLocation) {
            Debug.LogError("Some things wrong with location update");
        }
        else if (playerLocation > prevLocaton) {    // player went forward
            Debug.Log("player went forward from " + prevLocaton + " to " + playerLocation);
            int roomToEnable = (int)playerLocation + 1;
            int roomToDisable = (int)prevLocaton - 1;
            print(roomToEnable + ", " + roomToDisable);
            if (roomToEnable >= 0 && roomToEnable < map.Length) 
                map[roomToEnable].SetActive(true);
            if (roomToDisable >= 0 && roomToDisable < map.Length) 
                map[roomToDisable].SetActive(false);
        }
        else if (playerLocation < prevLocaton) {    // player went backward
            Debug.Log("player went backward from " + prevLocaton + " to " + playerLocation);
            int roomToEnable = (int)playerLocation - 1;
            int roomToDisable = (int)prevLocaton + 1;
            if (roomToEnable >= 0 && roomToEnable < map.Length) 
                map[roomToEnable].SetActive(true);
            if (roomToDisable >= 0 && roomToDisable < map.Length) 
                map[roomToDisable].SetActive(false);
        }
    }
}
