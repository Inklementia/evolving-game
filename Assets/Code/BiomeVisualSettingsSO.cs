using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    [CreateAssetMenu(menuName = "Biome/Biome Visual Settings")]
    public class BiomeVisualSettingsSO : ScriptableObject
    {
        public TileBase tileBlack;
        public TileBase tileHot;
        public TileBase tileCold;
        public TileBase tileGreen;
        public TileBase tileVolcano;
        public TileBase tileIceberg;

        public TileBase GetVisualForTemperature(int temp)
        {
            if (Mathf.Abs(temp) <= PowerHolder.Instance.CurrentHabitatValue) return tileGreen;
            return temp > 0 ? tileHot : tileCold;
        }

        public TileBase GetCenterTile(BiomeHeart heart)
        {
            return heart switch
            {
                BiomeHeart.Volcano => tileVolcano,
                BiomeHeart.Iceberg => tileIceberg,
                _ => tileGreen
            };
        }
    }
}
    