using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;


/**
ChapterManager
Manages logic in room selection and enemy/destructible placements.


Kyle Santos
1-29-2023
Holodash Team

TODO:
 - [] TODO: you can put all chapter room arrays in another array,
            with indices 0=prologue 1=ch1, 7=ch7, etc
 - [] Make placeRooms() recursive, in preparation for Branch Rooms 


*/
public class ChapterManager : MonoBehaviour
{
    // amount of total possible randomly generated rooms for this chapter
    public const int NUMROOMS = 5; 
    public const int NUMCHAPTERS = 8; 

    // reference to GameObject with Grid component at root
    [SerializeField] private Grid grid; 

    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject endRoom;
    [SerializeField] private GameObject ch0;
    [SerializeField] private List<GameObject> ch1;

    [SerializeField] private GameObject[] enemies; // available enemies

    List<GameObject>[] chapterRooms;

    // to keep the hierarchy clean
    // we child GameObjects (enemies, coins, destructibles) in this Transform
    private Transform enemiesContainer;

    void Start() {
        enemiesContainer = new GameObject("Enemies").transform;
    }
    /**
    Initializes all rooms for the current chapter.    

    @Parameters
    - chapter: denotes the current chapter that is being played
        0: Prologue
        1: Chapter 1
        2: Chapter 2
            etc.
    */
    public void initChapter(int chapter)
    {
        ClearLevel();
        if (chapter == 0) // if prologue
        {  
            initPrologue();
            return;
        }

        placeRooms(chapter);
    }

    // Initializes rooms for the prologue tutorial.
    void initPrologue()
    {
        // GameObject startRoomInstance = Instantiate(startRoom, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        // startRoomInstance.transform.SetParent(grid.transform);
        placeRoom(startRoom, new Vector2(0f, 0f));

        Vector2 startRoomExit = startRoom.GetComponent<Room>().exit;
        // GameObject tutorialInstance = Instantiate(ch0, startRoomExit, Quaternion.identity) as GameObject;
        // tutorialInstance.transform.SetParent(grid.transform);
        spawnEnemies(placeRoom(ch0, startRoomExit));
    }

    // Will initialize all rooms, connecting them properly.
    // Each room is set up with appropriate enemies, destructibles, etc. one at a time
    void placeRooms(int chapter)
    {
        placeRoom(startRoom, new Vector2(0f, 0f));
        Vector2 lastExit = startRoom.GetComponent<Room>().exit;
        for (int i = 0; i < NUMROOMS; i++)
        {
            int randy = Random.Range (0, ch1.Count-1);
            GameObject randRoom = ch1[randy];
            ch1.RemoveAt(randy);
            Vector2 rrEntrance = randRoom.GetComponent<Room>().entrance;
            Vector2 rrExit = randRoom.GetComponent<Room>().exit;

            // calculate entrance and exit connection position
            Vector2 entrancePos = lastExit - rrEntrance;
            Vector2 exitPos = (lastExit + rrExit) - rrEntrance;

            GameObject randRoomInstance = placeRoom(randRoom, entrancePos);
            spawnEnemies(randRoomInstance);

            lastExit = exitPos;
        }

        placeRoom(endRoom, lastExit - endRoom.GetComponent<Room>().entrance);
    }


    // Parameters
    // - roomToInstance: room prefab to instance into game.
    // - position: global position at which to place room
    // 
    // Instances and places the given room at a given location.
    GameObject placeRoom(GameObject roomToInstance, Vector2 position)
    {
        GameObject roomInstance = Instantiate(roomToInstance, position, Quaternion.identity) as GameObject;
        roomInstance.transform.SetParent(grid.transform);
        return roomInstance;
    }

    // @Requires: room must be an instance, NOT a prefab
    // spawns enemies in room instance
    void spawnEnemies(GameObject room)
    {
        if (enemiesContainer == null)
            enemiesContainer = new GameObject("Enemies").transform;

        // get enemy positions from room.enemyPositions
        Vector2[] enemyPositions = room.GetComponent<Room>().enemyPositions;

        for (int i = 0; i < enemyPositions.Length; i++)
        {
            // choose random enemy from the enemies array
            GameObject enemyPrefab = enemies[Random.Range (0, enemies.Length-1)];

            // calculate placement position relative to room
            // (+1 to y for some reason)
            Vector3 pos = room.transform.position + new Vector3(enemyPositions[i].x, enemyPositions[i].y + 1, 0);

            // instantiate at position
            GameObject enemyInstance = Instantiate(enemyPrefab, pos, Quaternion.identity) as GameObject;

            // place in enemies container
            enemyInstance.transform.SetParent(enemiesContainer);
        }
    }

    void ClearLevel() {
        foreach (Transform child in grid.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach(Transform enemy in enemiesContainer) {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
