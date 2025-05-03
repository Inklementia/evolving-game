using System;
using UnityEngine;

namespace Code
{
    public struct CubeCoordinate
    {
        private int x;
        private int y;
        private int z;

        public CubeCoordinate(int x, int y, int z)
        {
            if (x + y + z != 0)
                throw new ArgumentException("Invalid cube coordinates: x + y + z must equal 0");

            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int DistanceTo(CubeCoordinate other)
        {
            return (Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y) + Mathf.Abs(z - other.z)) / 2;
        }
    }
}