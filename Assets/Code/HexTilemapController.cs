
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public partial class HexTilemapController : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile tileBlack;
        [SerializeField] private Tile tileHot;
        [SerializeField] private Tile tileCold;
        [SerializeField] private Tile tileGreen;

        private BiomeHeart _currentBiomeMode;
        private Dictionary<Vector3Int, BiomeTileData> biomeTiles = new();

        [SerializeField] private bool drawRingDebug = true;
        [SerializeField] private int debugRadius = 2;
        [SerializeField] private Vector3Int debugCenter = new(0, 4, 0);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _currentBiomeMode = BiomeHeart.Cold;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                _currentBiomeMode = BiomeHeart.Hot;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = tilemap.WorldToCell(worldPos);
                cellPos.z = 0;

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

            for (int r = 1; r <= radius; r++)
            {
                var ring = GetHexRing(center, r);
                foreach (var pos in ring)
                {
                    int influence = baseTemp > 0 ? baseTemp - r : baseTemp + r;
                    if (biomeTiles.TryGetValue(pos, out var existing))
                    {
                        existing.UpdateTemperature(influence, GetTileForTemperature);
                        biomeTiles[pos] = existing;
                    }
                    else
                    {
                        biomeTiles[pos] = new BiomeTileData(pos, influence, BiomeHeart.None, GetTileForTemperature(influence));
                    }
                    tilemap.SetTile(pos, biomeTiles[pos].tileVisual);
                }
            }
        }

        private Tile GetTileForTemperature(int temp)
        {
            if (temp > 0) return tileHot;
            if (temp < 0) return tileCold;
            if (temp == 0) return tileGreen;
            return tileBlack;
        }

        private Vector3Int OffsetToCube(Vector3Int offset)
        {
            int x = offset.x;
            int z = offset.y - (offset.x - (offset.x & 1)) / 2;
            int y = -x - z;
            return new Vector3Int(x, y, z);
        }

        private Vector3Int CubeToOffset(Vector3Int cube)
        {
            int col = cube.x;
            int row = cube.z + (cube.x - (cube.x & 1)) / 2;
            return new Vector3Int(col, row, 0);
        }

        private static readonly Vector3Int[] cubeDirections = new Vector3Int[]
        {
            new(1, -1, 0),
            new(1, 0, -1),
            new(0, 1, -1),
            new(-1, 1, 0),
            new(-1, 0, 1),
            new(0, -1, 1)
        };

        private Vector3Int CubeNeighbor(Vector3Int cube, int dir)
        {
            var d = cubeDirections[dir];
            return new Vector3Int(cube.x + d.x, cube.y + d.y, cube.z + d.z);
        }

        private List<Vector3Int> GetHexRing(Vector3Int centerOffset, int radius)
        {
            List<Vector3Int> results = new();
            if (radius == 0)
            {
                results.Add(centerOffset);
                return results;
            }

            Vector3Int centerCube = OffsetToCube(centerOffset);
            Vector3Int cube = centerCube;
            for (int i = 0; i < radius; i++)
                cube = CubeNeighbor(cube, 5); // NE

            for (int dir = 0; dir < 6; dir++)
            {
                for (int i = 0; i < radius; i++)
                {
                    results.Add(CubeToOffset(cube));
                    cube = CubeNeighbor(cube, dir);
                }
            }

            return results;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawRingDebug || tilemap == null) return;

            Gizmos.color = Color.red;
            Vector3 centerWorld = tilemap.GetCellCenterWorld(debugCenter);
            Gizmos.DrawSphere(centerWorld, 0.1f);

            List<Vector3Int> ring = GetHexRing(debugCenter, debugRadius);
            foreach (var cell in ring)
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(worldPos, Vector3.one * 0.85f);
            }
        }
    }
}
