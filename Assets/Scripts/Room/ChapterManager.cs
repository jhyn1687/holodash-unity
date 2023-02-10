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
 - [] Make PlaceRooms() recursive, in preparation for Branch Rooms 


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

    private HashSet<int> usedRooms; // list of available rooms. 
                            // ints correspond to their index in ch1
                            // removing one marks current index as "used". (no room repeats)
    
    private Vector2 lastEndRoomPos;
    private List<Vector2> roomEndPositions;

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

        Vector2 startRoomExit = PlaceRoom(startRoom, new Vector2(0f, 0f));

        if (chapter == 0) // if prologue
        {
            PlaceRoom(ch0, startRoomExit);
        }

        usedRooms = new HashSet<int>();
    }

    // TODO call this OnExitReached
    private void OnEndzoneReached()
    {

        int randy = Random.Range(0, ch1.Count);
        while (usedRooms.Contains(randy))
            randy = Random.Range(0, ch1.Count);
        usedRooms.Add(randy);

        GameObject randRoom = ch1[randy];

        PlaceRoom(randRoom, lastEndRoomPos);
    }

    // Initializes rooms for the prologue tutorial.
    void initPrologue()
    {
        // GameObject startRoomInstance = Instantiate(startRoom, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        // startRoomInstance.transform.SetParent(grid.transform);
        Vector2 startRoomExit = PlaceRoom(startRoom, new Vector2(0f, 0f));

        PlaceRoom(ch0, startRoomExit);
    }


    // Parameters
    // - roomToInstance: room prefab to instance into game.
    // - position: global position at which to place room
    // 
    // Instances and places the given room at a given location.
    Vector2 PlaceRoom(GameObject roomToInstance, Vector2 position)
    {
        GameObject roomInstance = Instantiate(roomToInstance, position, Quaternion.identity) as GameObject;

        roomInstance.transform.SetParent(grid.transform);
        
        lastEndRoomPos = HandleRoomInfo(roomInstance);
        return lastEndRoomPos;
    }

    // Uses RoomInfoPositions tilemap to get room entrance, exit, position information as well as
    // instance coins, enemies, etc. in their corresponding positions.
    Vector2 HandleRoomInfo(GameObject room)
    {
        Tilemap tilemap;
        Vector2 endPosition = Vector2.zero;

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

            Debug.Log(t.name + " at " + tileLocalPos.ToString());

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
                case "EndMarker":
                    endPosition = new Vector2(tileLocalPos.x, tileLocalPos.y);

                    break;
            }
        }

        // delete RoomPositionsInfo tilemap
        GameObject.Destroy(room.transform.GetChild(0).GetChild(0).gameObject);
        GameObject.Destroy(room.transform.GetChild(0).gameObject);

        return endPosition;
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

    private void OnEnable()
    {
        //GameManager.OnReset += OnReset;
        EndzoneScript.EndzoneReached += OnEndzoneReached;
    }
    private void OnDisable()
    {
        //GameManager.OnReset -= OnReset;
        EndzoneScript.EndzoneReached -= OnEndzoneReached;
    }
}
