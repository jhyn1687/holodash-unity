using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    // using the singleton pattern for the ChapterManager
    private static ChapterManager _instance;
    public static ChapterManager Instance {
        get {
            if (_instance == null) {
                Debug.Log("Chapter Manager is null");
            }
            return _instance;
        }
    }
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
    [SerializeField] private GameObject coin;

    List<GameObject>[] chapterRooms;


    // to keep the hierarchy clean
    // we child GameObjects (enemies, coins, destructibles) in this Transform
    private Transform enemiesContainer;

    void Awake() {
        _instance = this;
        enemiesContainer = new GameObject("Enemies").transform;
    }

    void Start() {

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
        GameObject tutorialInstance = Instantiate(ch0, startRoomExit, Quaternion.identity) as GameObject;
        tutorialInstance.transform.SetParent(grid.transform);
        
        // spawnEnemies(placeRoom(ch0, startRoomExit));
    
        testHandleRoom(tutorialInstance);
    }

    // Will initialize all rooms, connecting them properly.
    // Each room is set up with appropriate enemies, destructibles, etc. one at a time
    void placeRooms(int chapter)
    {
        placeRoom(startRoom, new Vector2(0f, 0f));
        Vector2 lastExit = startRoom.GetComponent<Room>().exit;
        // cloning the chapter so that reset doesn't delete the original rooms.
        List<GameObject> ch1_clone = new List<GameObject>(ch1);
        for (int i = 0; i < NUMROOMS; i++)
        {
            int randy = Random.Range (0, ch1_clone.Count);
            GameObject randRoom = ch1_clone[randy];
            ch1_clone.RemoveAt(randy);
            Vector2 rrEntrance = randRoom.GetComponent<Room>().entrance;
            Vector2 rrExit = randRoom.GetComponent<Room>().exit;

            // calculate entrance and exit connection position
            Vector2 entrancePos = lastExit - rrEntrance;
            Vector2 exitPos = (lastExit + rrExit) - rrEntrance;

            GameObject randRoomInstance = placeRoom(randRoom, entrancePos);
            testHandleRoom(randRoomInstance);
            //spawnEnemies(randRoomInstance);

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

    void testHandleRoom(GameObject room)
    {
        Tilemap tilemap;

        if (enemiesContainer == null)
            enemiesContainer = new GameObject("Enemies").transform;

        // List<Vector3> tileWorldLocations;
        // tileWorldLocations = new List<Vector3>();
        // Debug.Log(room.name);

        // get Tilemap of RoomInfoPositions
        // Grid > Tutorial > RoomInfo > RoomInfoPositions
        tilemap = room.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int tileLocalPos = new Vector3Int(pos.x, pos.y, pos.z);
            // TODO https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.html
            TileBase t = tilemap.GetTile(tileLocalPos);
            if (t == null)
                continue;
            //Debug.Log("curr tile");
            //Debug.Log(t);
            Debug.Log(t.name + " at " + tileLocalPos.ToString());

            // Vector3 place = tilemap.CellToWorld(tileLocalPos);
            // if (tilemap.HasTile(tileLocalPos))
            // {
            //     tileWorldLocations.Add(place);
            // }

            switch (t.name)
            {
                case "coin_0":
                    // instantiate at position
                    GameObject c = InstanceSpawnable(coin, tileLocalPos, room);
                    c.transform.SetParent(enemiesContainer);

                    break;
                case "enemy":
                    // choose random enemy from the enemies array
                    GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length - 1)];

                    GameObject enemyInstance = InstanceSpawnable(enemyPrefab, tileLocalPos, room);

                    enemyInstance.transform.SetParent(enemiesContainer);

                    break;
            }
        }

        // delete RoomPositionsInfo tilemap
        GameObject.Destroy(room.transform.GetChild(0).GetChild(0).gameObject);
        GameObject.Destroy(room.transform.GetChild(0).gameObject);

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

    // Instances and places the specified prefab at the given position in the given room.
    GameObject InstanceSpawnable(GameObject prefab, Vector3 localPosition, GameObject room)
    {
        // calculate position with offset of room, the spawnable's local position, and a mid-tile offset
        Vector3 finalPosition = room.transform.position + localPosition + new Vector3(0.5f, 0.5f, 0f);

        // instantiate at position
        return Instantiate(prefab, finalPosition, Quaternion.identity) as GameObject;
    }


    void ClearLevel() {
        foreach (Transform child in grid.transform) {
            GameObject.Destroy(child.gameObject);
        }
        if (enemiesContainer != null) { 
            foreach (Transform enemy in enemiesContainer) {
                GameObject.Destroy(enemy.gameObject);
            }
        }
    }
}
