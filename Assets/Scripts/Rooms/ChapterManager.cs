using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;


/**
ChapterManager
Manages logic in room, enemy, destructible, item choosing and placement.
*/
public class ChapterManager : MonoBehaviour
{
    // reference to GameObject with Grid component at root
    [SerializeField] public GameObject grid; 

    [SerializeField] public GameObject startRoom;
    [SerializeField] public GameObject ch0;
    [SerializeField] public GameObject[] ch1;

    [SerializeField] public GameObject[] enemies;

    int numRooms; // amount of total possible rooms for this chapter

    List<Room> chosenRooms; // rooms selectRooms() chose

    // to keep the hierarchy clean
    // we child GameObjects (enemies, coins, destructibles) in this Transform
    private Transform enemiesContainer;


    /**
    

    0: Prologue
    1: Chapter 1

    */
    public void initChapter(int chapter)
    {
        if (chapter == 0) // if prologue
        {  
            initPrologue();
            return;
        }
        
        


    }

    void initPrologue()
    {
        GameObject startRoomInstance = Instantiate(startRoom, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        startRoomInstance.transform.SetParent(grid.transform);

        Vector2 startRoomExit = startRoom.GetComponent<Room>().exit;
        GameObject tutorialInstance = Instantiate(ch0, startRoomExit, Quaternion.identity) as GameObject;
        tutorialInstance.transform.SetParent(grid.transform);

        spawnEnemies(tutorialInstance);
    }

    void spawnEnemies(GameObject room)
    {
        enemiesContainer = new GameObject ("Enemies").transform;

        // get enemy positions from room.enemyPositions
        Vector2[] enemyPositions = room.GetComponent<Room>().enemyPositions;

        for (int i = 0; i < enemyPositions.Length; i++)
        {
            // choose random enemy from the enemies array
            GameObject enemyPrefab = enemies[Random.Range (0, enemies.Length-1)];

            // calculate placement position relative to room (idk how to convert local to global yet)
            Vector3 pos = room.transform.position + new Vector3(enemyPositions[i].x, enemyPositions[i].y, 0);

            // instantiate at position
            GameObject enemyInstance = Instantiate(enemyPrefab, pos, Quaternion.identity) as GameObject;

            // place in enemies container
            enemyInstance.transform.SetParent(enemiesContainer);
        }
    }

}
