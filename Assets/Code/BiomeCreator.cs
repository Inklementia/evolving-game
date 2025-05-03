using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public class BiomeCreator : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Grid grid;
        [SerializeField] private Tile tileBlack;
        [SerializeField] private Tile tileHot;
        [SerializeField] private Tile tileCold;
        [SerializeField] private Tile tileGreen;

        private BiomeHeart _currentBiomeMode = BiomeHeart.Hot;
        private Dictionary<Vector3Int, BiomeTileData> biomeTiles = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentBiomeMode = BiomeHeart.Cold;
                Debug.LogWarning("Placing ICEBERG");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentBiomeMode = BiomeHeart.Hot;
                Debug.LogWarning("Placing VOLCANO");
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = grid.WorldToCell(worldPos);
                cellPos.z = 0;
                Debug.Log(cellPos);

                if (tilemap.GetTile(cellPos) != tileBlack) return;

                CreateBiomeArea(cellPos, 2);
            }
        }

        private void CreateBiomeArea(Vector3Int center, int radius)
        {
            int baseTemp = _currentBiomeMode == BiomeHeart.Hot ? 3 : -3;
            Tile centerTile = _currentBiomeMode == BiomeHeart.Hot ? tileHot : tileCold;

            biomeTiles[center] = new BiomeTileData(center, baseTemp, _currentBiomeMode, centerTile);
            tilemap.SetTile(center, centerTile);
            
          
              var fullArea = GetFlatTopHexRange(center, 2);
              foreach (var cell in fullArea)
              {
                  tilemap.SetTile(cell, tileHot);
              }
          
        }

        private static readonly Vector3Int[] EvenRowOffsets = new[]
        {
            new Vector3Int(+1,  0, 0),  // E
            new Vector3Int( 0, -1, 0),  // SE
            new Vector3Int(-1, -1, 0),  // SW
            new Vector3Int(-1,  0, 0),  // W
            new Vector3Int(-1, +1, 0),  // NW
            new Vector3Int( 0, +1, 0)   // NE
        };

        private static readonly Vector3Int[] OddRowOffsets = new[]
        {
            new Vector3Int(+1,  0, 0),  // E
            new Vector3Int(+1, -1, 0),  // SE
            new Vector3Int( 0, -1, 0),  // SW
            new Vector3Int(-1,  0, 0),  // W
            new Vector3Int( 0, +1, 0),  // NW
            new Vector3Int(+1, +1, 0)   // NE
        };
        
        private List<Vector3Int> GetFlatTopHexRing(Vector3Int center, int radius)
        {
            List<Vector3Int> ring = new();

            if (radius == 0)
            {
                ring.Add(center);
                return ring;
            }

            // Go to starting tile (direction 4 repeated radius times)
            Vector3Int current = center;
            for (int i = 0; i < radius; i++)
            {
                current = StepInDirection(current, 4); // Start NW corner
            }

            // Walk around 6 directions
            for (int dir = 0; dir < 6; dir++)
            {
                for (int i = 0; i < radius; i++)
                {
                    ring.Add(current);
                    current = StepInDirection(current, dir);
                }
            }

            return ring;
        }

        private Vector3Int StepInDirection(Vector3Int from, int direction)
        {
            bool isOdd = from.y % 2 != 0;
            Vector3Int[] offsets = isOdd ? OddRowOffsets : EvenRowOffsets;
            return from + offsets[direction];
        }
        private List<Vector3Int> GetFlatTopHexRange(Vector3Int center, int radius)
        {
            List<Vector3Int> area = new();
            area.Add(center);

            Queue<Vector3Int> frontier = new();
            HashSet<Vector3Int> visited = new() { center };

            frontier.Enqueue(center);

            for (int i = 0; i < radius; i++)
            {
                int layerCount = frontier.Count;

                for (int j = 0; j < layerCount; j++)
                {
                    var current = frontier.Dequeue();
                    bool isOdd = current.y % 2 != 0;
                    var offsets = isOdd ? OddRowOffsets : EvenRowOffsets;

                    foreach (var offset in offsets)
                    {
                        var neighbor = current + offset;
                        if (visited.Add(neighbor))
                        {
                            area.Add(neighbor);
                            frontier.Enqueue(neighbor);
                        }
                    }
                }
            }

            return area;
        }
    }
}
