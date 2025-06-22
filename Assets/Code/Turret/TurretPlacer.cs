using System.Collections.Generic;
using Code.Scripts.Object_Pooler;
using ToolBox.Tags;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

namespace Code
{
    public class TurretPlacer : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tag turretTag;
        [SerializeField] private TileBase greenTile;

        [Header("Hotkeys")]
        [SerializeField] private KeyCode enablePlacementKey = KeyCode.Alpha3;
        [SerializeField] private int mouseButton = 1;              // 0 = LMB, 1 = RMB

        private readonly HashSet<Vector3Int> occupiedTiles = new();
        private bool canPlaceTurret;

        private void Update()
        {
            if (Input.GetKeyDown(enablePlacementKey))
            {
                canPlaceTurret = true;
                Debug.Log("<color=lime>Turret-placement mode ON</color>");
            }

            if (canPlaceTurret && Input.GetMouseButtonDown(mouseButton))
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0f;                                     3
                Vector3Int cell   = tilemap.WorldToCell(worldPos);

                TryPlaceTurret(cell);
            }
        }

        private void TryPlaceTurret(Vector3Int cell)
        {
            // уже занято?
            if (occupiedTiles.Contains(cell))
            {
                Debug.LogWarning($"Tile {cell} already has a turret");
                return;
            }

            // не зелёная клетка?
            if (tilemap.GetTile(cell) != greenTile)
            {
                Debug.LogWarning($"Tile {cell} is not buildable (needs greenTile)");
                return;
            }
            

            // ставим башню
            Vector3 spawnPos = tilemap.GetCellCenterWorld(cell);
            ObjectPooler.Instance.SpawnFromPool(turretTag, spawnPos, Quaternion.identity);
            occupiedTiles.Add(cell);
            canPlaceTurret = false;                                 // выходим из режима
        }
    }
}
