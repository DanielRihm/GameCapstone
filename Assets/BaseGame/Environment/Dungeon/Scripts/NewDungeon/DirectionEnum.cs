using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class DirectionEnum
    {
        public enum Direction
        {
            North, East, South, West, Null
        }

        public Direction Opposite(Direction direction)
        {
            if (direction == Direction.North)
            {
                return Direction.South;
            }
            else if (direction == Direction.East)
            {
                return Direction.West;
            }
            else if (direction == Direction.South)
            {
                return Direction.North;
            }
            else
            {
                return Direction.East;
            }
        }

        public Quaternion QuaternionFacing(Direction direction)
        {
            if (direction == Direction.North)
            {
                return Quaternion.identity;
            }
            else if (direction == Direction.East)
            {
                return Quaternion.Euler(0, 60, 0);
            }
            else if (direction == Direction.South)
            {
                return Quaternion.identity;
            }
            else
            {
                return Quaternion.Euler(0, 300, 0);
            }
        }
    }
}
