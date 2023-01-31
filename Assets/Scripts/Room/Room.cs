using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // public class Connection
    // {
    //     Vector2 point; // Global position
    //     Room room; // pointer to the room being connected

    //     public Connection (Vector2 p, Room rm)
    //     {
    //         point = p;
    //         room = rm;
    //     }
    // }


    /*
    Positions are defined as local position.
    These positions are based on the bottom left corner of the object you're referring to. 
    For a 1 tile wide, 2 tiles tall space, (0, 0) is the position.
    */
    
    GameObject room;

    [SerializeField] public Vector2 entrance; // Global position
    [SerializeField] public Vector2 exit;
    Room entranceRoom; // pointer to the room being connected
    Room exitRoom;

    [SerializeField] public Vector2[] enemyPositions;
    [SerializeField] public Vector2[] destructiblePositions;
    [SerializeField] public Vector2[] doorPositions;
    Vector2[] cutscenePositions;


    // void connect(Room other)
    // {

    // }
}
