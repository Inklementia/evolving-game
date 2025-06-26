using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public class BiomeTileData
    {
        public Vector3Int Position { get; }
        public int Temperature { get; private set; }
        public BiomeHeart Heart { get; private set; }
        public BiomeTile Type { get; private set; }
        public TileBase Visual { get; set; }  // Assigned externally

        public bool IsSource => Heart != BiomeHeart.None;

        public BiomeTileData(Vector3Int position, int initialTemperature, BiomeHeart heart)
        {
            Position = position;
            Temperature = initialTemperature;
            Type = DetermineBiomeTile(initialTemperature);
            Heart = ValidateHeart(Type, heart);
        }

        public void AddTemperature(int delta)
        {
            Temperature += delta;
            Type = DetermineBiomeTile(Temperature);
            Heart = IsSource ? Heart : ValidateHeart(Type, BiomeHeart.None);
        }
        
        public void SetTemperature(int delta)
        {
            Temperature = delta;
            Type = DetermineBiomeTile(Temperature);
            Heart = IsSource ? Heart : ValidateHeart(Type, BiomeHeart.None);
        }

        public void ResetTemperature()
        {
            Temperature = 0;
        }

        private BiomeTile DetermineBiomeTile(int temp)
        {
            if (Mathf.Abs(temp) <= PowerHolder.Instance.CurrentHabitatValue) return BiomeTile.Green;
            return temp > PowerHolder.Instance.CurrentHabitatValue ? BiomeTile.Hot : BiomeTile.Cold;
        }

        private BiomeHeart ValidateHeart(BiomeTile tile, BiomeHeart heart)
        {
            return tile switch
            {
                BiomeTile.Hot => heart == BiomeHeart.Volcano ? BiomeHeart.Volcano : BiomeHeart.None,
                BiomeTile.Cold => heart == BiomeHeart.Iceberg ? BiomeHeart.Iceberg : BiomeHeart.None,
                _ => BiomeHeart.None,
            };
        }
    }
}