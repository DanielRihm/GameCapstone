using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCPS.SlipForge;


public class DungeonFloor : MonoBehaviour
{
    public Vector2 Location;

    public Vector2 NorthPortal, EastPortal, SouthPortal, WestPortal;
    public Vector2 SpawnPoint;

    public Vector3 GetLocationOf(DirectionEnum.Direction direction)
    {
        if (direction == DirectionEnum.Direction.North)
        {
            return new Vector3(NorthPortal.x, 0, NorthPortal.y);
        } 
        else if (direction == DirectionEnum.Direction.East)
        {
            return new Vector3(EastPortal.x, 0, EastPortal.y);
        }
        else if (direction == DirectionEnum.Direction.South)
        {
            return new Vector3(SouthPortal.x, 0, SouthPortal.y);
        }
        else
        {
            return new Vector3(WestPortal.x, 0, WestPortal.y);
        }
    }
}
