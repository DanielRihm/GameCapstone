using UnityEngine;

namespace LCPS.SlipForge.Util
{
    public static class Vector3Extensions
    {
        public static Vector3 InvertX(this Vector3 vector)
        {
            vector.x *= -1;
            return vector;
        }

        public static Vector3 InvertY(this Vector3 vector)
        {
            vector.y *= -1;
            return vector;
        }

        public static Vector3 InvertZ(this Vector3 vector)
        {
            vector.z *= -1;
            return vector;
        }

        public static Vector3 InvertAxes(this Vector3 vector)
        {
            vector.x *= -1;
            vector.y *= -1;
            vector.z *= -1;
            return vector;
        }

        public static Vector3 Copy(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
    }
}

