using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public class BiomeCreator : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private BiomeVisualSettingsSO visualSettings;
        [SerializeField] private TemperatureLabeler temperatureLabeler;
        private int maxTemperature;

        private BiomeHeart currentBiomeMode = BiomeHeart.Volcano;
        private Dictionary<Vector3Int, BiomeTileData> biomeTiles = new();
        private HexGridHelper hexService;

        private void Awake()
        {
            hexService = new HexGridHelper(); // or inject via Construct() if DI
            maxTemperature = PowerHolder.Instance.CurrentHeartTemp;
        }

        public void UpdateMaxTemperature()
        {
            maxTemperature = PowerHolder.Instance.CurrentHeartTemp;
            RecreateAllBiomeAreasForCurrentMode();
            
            temperatureLabeler?.UpdateLabels();
        }

        public void SetCurrentBiomeMode(BiomeHeart biomeHeart)
        {
            currentBiomeMode = biomeHeart;
            
        }

        public void UpdateMap()
        {
            UpdateTileVisuals();
            temperatureLabeler?.UpdateLabels();
        }
        private void ClearTemperatures()
        {
            foreach (var tile in biomeTiles.Values)
            {
                tile.ResetTemperature();
            }
        }


        public IReadOnlyDictionary<Vector3Int, BiomeTileData> GetBiomeTiles() => biomeTiles;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) currentBiomeMode = BiomeHeart.Iceberg;
            if (Input.GetKeyDown(KeyCode.Alpha2)) currentBiomeMode = BiomeHeart.Volcano;

            if (Input.GetMouseButtonDown(0))
            {
                var pos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                pos.z = 0;

                if (tilemap.GetTile(pos) != visualSettings.tileBlack) return;

                CreateBiomeArea(pos, maxTemperature - 1);
            }
        }

        private void CreateBiomeArea(Vector3Int center, int radius, bool toAdd = true)
        {
            int sourceTemp = currentBiomeMode == BiomeHeart.Volcano ? maxTemperature : -maxTemperature;
            var centerTile = visualSettings.GetCenterTile(currentBiomeMode);

            biomeTiles[center] = new BiomeTileData(center, sourceTemp, currentBiomeMode);
            tilemap.SetTile(center, centerTile);

            for (int r = 1; r <= radius; r++)
            {
                int ringTemp = sourceTemp > 0 ? sourceTemp - r : sourceTemp + r;

                foreach (var pos in hexService.GetRing(center, r))
                {
                    if (!biomeTiles.ContainsKey(pos))
                        biomeTiles[pos] = new BiomeTileData(pos, 0, BiomeHeart.None);

                    if(toAdd) 
                    {biomeTiles[pos].AddTemperature(ringTemp);}
                    else
                    {
                        biomeTiles[pos].SetTemperature(ringTemp);}
                }
                
            }

            UpdateTileVisuals();
            temperatureLabeler?.UpdateLabels();
        }

        private void UpdateTileVisuals()
        {

            foreach (var (pos, tile) in biomeTiles)
            {
                if (tile.IsSource && Mathf.Abs(tile.Temperature) == maxTemperature) continue;

                tile.Visual = visualSettings.GetVisualForTemperature(tile.Temperature);
                tilemap.SetTile(pos, tile.Visual);
            }
        }
        
        private void RecreateAllBiomeAreasForCurrentMode()
        {
            var centers = new List<Vector3Int>();

            foreach (var (pos, tile) in biomeTiles)
            {
                if (tile.IsSource && tile.Heart == currentBiomeMode)
                {
                    centers.Add(pos);
                }
            }

            foreach (var centerPos in centers)
            {
                CreateBiomeArea(centerPos, maxTemperature - 1, toAdd: false);
            }
        }
    }
}
