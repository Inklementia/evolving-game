using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class HexGridHelper
    {
        private readonly Vector3Int[] evenRowOffsets = {
            new(+1, 0), new(0, -1), new(-1, -1), new(-1, 0), new(-1, +1), new(0, +1)
        };

        private readonly Vector3Int[] oddRowOffsets = {
            new(+1, 0), new(+1, -1), new(0, -1), new(-1, 0), new(0, +1), new(+1, +1)
        };

        public List<Vector3Int> GetRing(Vector3Int center, int radius)
        {
            List<Vector3Int> result = new();
            if (radius == 0) { result.Add(center); return result; }

            var current = center;
            for (int i = 0; i < radius; i++)
                current = Step(current, 4); // NW

            for (int dir = 0; dir < 6; dir++)
            for (int i = 0; i < radius; i++)
            {
                result.Add(current);
                current = Step(current, dir);
            }

            return result;
        }

        public Vector3Int Step(Vector3Int from, int dir)
        {
            var offsets = (from.y % 2 == 0) ? evenRowOffsets : oddRowOffsets;
            return from + offsets[dir];
        }
    }
}