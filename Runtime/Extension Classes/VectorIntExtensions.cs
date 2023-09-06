using System;
using UnityEngine;

namespace crass
{
    public static class VectorIntExtensions
    {
        public static Vector2Int DivideBy(this Vector2Int dividend, float divisor)
        {
            return new Vector2Int((int)(dividend.x / divisor), (int)(dividend.y / divisor));
        }

        public static Vector3Int DivideBy(this Vector3Int dividend, float divisor)
        {
            return new Vector3Int(
                (int)(dividend.x / divisor),
                (int)(dividend.y / divisor),
                (int)(dividend.z / divisor)
            );
        }

        public static int ManhattanDistanceTo(this Vector3Int self, Vector3Int other)
        {
            return Math.Abs(self.x - other.x)
                + Math.Abs(self.y - other.y)
                + Math.Abs(self.z - other.z);
        }

        public static int ManhattanDistanceTo(this Vector2Int self, Vector2Int other)
        {
            return Math.Abs(self.x - other.x) + Math.Abs(self.y - other.y);
        }
    }
}
