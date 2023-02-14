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
    // Number of total rooms between each boss.
    public const int NUMROOMS = 4; 
    // public const int NUMCHAPTERS = 8; 

    // reference to GameObject with Grid component at root
    [SerializeField] private Grid grid; 

    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject ch0;
    [SerializeField] private List<GameObject> ch1;

    [SerializeField] private GameObject[] enemies; // available enemies
    [SerializeField] private GameObject coin;

    //private HashSet<int> usedRooms; // list of available rooms. 
                            // ints correspond to their index in ch1
                            // removing one marks current index as "used". (no room repeats)
    
    private Vector2 lastEndRoomPos;
    private int sinceLastBoss; 
    //private List<Vector2> roomEndPositions; // or make it used room names? 

    // to keep the hierarchy clean
    // we child GameObjects (enemies, coins, destructibles) in this Transform
    private Transform enemiesContainer;
    private Transform ftpsContainer;

    void Awake() {
        _instance = this;
        enemiesContainer = new GameObject("EnemiesContainer").transform;
        ftpsContainer = new GameObject("FtpsContainer").transform;
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
        lastEndRoomPos = Vector2.zero;

        GameObject startRoomInstance = PlaceRoom(startRoom, new Vector2(0f, 0f));

        if (chapter == 0) 
        {
            // delete StartRoom endzone
            GameObject.Destroy(startRoomInstance.transform.GetChild(1).gameObject);

            // if prologue, place tutorial
            PlaceRoom(ch0, lastEndRoomPos);
        }

        GenerateRooms();

        //usedRooms = new HashSet<int>();
    }

    private void GenerateRooms()
    {
        for (int i = 0; i < NUMROOMS; i++)
        {
            int randy = Random.Range(0, ch1.Count);

            GameObject randRoom = ch1[randy];

            PlaceRoom(randRoom, lastEndRoomPos);
        }

        PlaceRoom(bossRoom, lastEndRoomPos);
    }

    // check if boss died
    private void OnBossDied()
    {
        GenerateRooms();
    }

    private void OnPlayerDeath()
    {   
        Transform[] allRooms = GameObject.Find("Grid").transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allRooms.Length; i++)
        {
            if (String.Equals(allRooms[i].name, "Grid"))
            {
                continue;
            }
            Debug.Log("CM: destroying " + allRooms[i].name);
            GameObject.Destroy(allRooms[i].gameObject);
        }

        Transform[] ftpsContainer = GameObject.Find("FtpsContainer").transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < ftpsContainer.Length; i++)
        {
            if (String.Equals(ftpsContainer[i].name, "FtpsContainer"))
            {
                continue;
            }
            Debug.Log("CM: destroying " + ftpsContainer[i].name);
            GameObject.Destroy(ftpsContainer[i].gameObject);
        }
    }

    // Modifies
    // sinceLastBoss - increments for every normal room generated
    //
    // TODO call this OnExitReached
    private void OnEndzoneReached()
    {


        if (sinceLastBoss == 3)
        {
            // boss room
            sinceLastBoss = 0;
            
            PlaceRoom(bossRoom, lastEndRoomPos);

        } else {
            sinceLastBoss++;

            // normal room
            int randy = Random.Range(0, ch1.Count);
            //while (usedRooms.Contains(randy))
            //    randy = Random.Range(0, ch1.Count);
            //usedRooms.Add(randy);

            Debug.Log("OnEndzone (exit) Reached");
            GameObject randRoom = ch1[randy];

            PlaceRoom(randRoom, lastEndRoomPos);
        }
    }

    // Initializes rooms for the prologue tutorial.
    void initPrologue()
    {
        // GameObject startRoomInstance = Instantiate(startRoom, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        // startRoomInstance.transform.SetParent(grid.transform);
        PlaceRoom(startRoom, new Vector2(0f, 0f));

        PlaceRoom(ch0, lastEndRoomPos);
    }

    // Modifies
    // lastEndRoomPos - sets to the placed room's exit position
    //
    // Parameters
    // - roomToInstance: room prefab to instance into game.
    // - position: global position at which to place room
    // 
    // Instances and places the given room at a given location.
    GameObject PlaceRoom(GameObject roomToInstance, Vector2 position)
    {
        GameObject roomInstance = Instantiate(roomToInstance, position, Quaternion.identity) as GameObject;

        roomInstance.transform.SetParent(grid.transform);
        Debug.Log(roomToInstance.name + "placed at: " + lastEndRoomPos);

        // Handle fallthrough platforms
        // thanks man https://forum.unity.com/threads/how-to-get-gameobjects-that-inside-instantiated-prefabs.841522/
        // problem: parent tags override child tags
        // solution: DUH just extract the ftp's out of the room, reparent them into new container

        // find all children whose names are "FallthroughPlatform" in the room
        // and set their parent to the FallthroughPlatformsContainer
        string ftp = "FallthroughPlatform";
        Transform[] ftPlatforms = roomInstance.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < ftPlatforms.Length; i++)
        {
            if (ftPlatforms[i].name.Contains(ftp))
            {
                ftPlatforms[i].SetParent(ftpsContainer);
            }
        }

        lastEndRoomPos = HandleRoomInfo(roomInstance);
        Debug.Log("its end position: " + lastEndRoomPos);

        return roomInstance;
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

            // Debug.Log(t.name + " at " + tileLocalPos.ToString());

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
                    endPosition = lastEndRoomPos + new Vector2(tileLocalPos.x, tileLocalPos.y);

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
        PlayerBehavior.OnPlayerDeath += OnPlayerDeath;
        EnemyBehavior.BossDied += OnBossDied;
    }
    private void OnDisable()
    {
        //GameManager.OnReset -= OnReset;
        EndzoneScript.EndzoneReached -= OnEndzoneReached;
        PlayerBehavior.OnPlayerDeath -= OnPlayerDeath;
        EnemyBehavior.BossDied -= OnBossDied;
    }
}
