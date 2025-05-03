using System;
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
        public TileBase Visual { get; private set; }

        public bool IsSource => Heart != BiomeHeart.None;

        public BiomeTileData(Vector3Int position, int initialTemperature, BiomeHeart heart, TileBase visual)
        {
            Position = position;
            Temperature = initialTemperature;
            Visual = visual;

            Type = DetermineBiomeTile(initialTemperature);
            Heart = ValidateHeart(Type, heart);
        }

        public void AddTemperature(int delta, System.Func<int, TileBase> visualResolver)
        {
            Temperature += delta;
            Type = DetermineBiomeTile(Temperature);
            Heart = IsSource ? Heart : ValidateHeart(Type, BiomeHeart.None);
            Visual = visualResolver?.Invoke(Temperature);
        }

        private static BiomeTile DetermineBiomeTile(int temp)
        {
            if (temp == 0) return BiomeTile.Green;
            return temp > 0 ? BiomeTile.Hot : BiomeTile.Cold;
        }

        private static BiomeHeart ValidateHeart(BiomeTile tile, BiomeHeart heart)
        {
            return tile switch
            {
                BiomeTile.Hot => heart == BiomeHeart.Hot ? BiomeHeart.Hot : BiomeHeart.None,
                BiomeTile.Cold => heart == BiomeHeart.Cold ? BiomeHeart.Cold : BiomeHeart.None,
                _ => BiomeHeart.None,
            };
        }
    }

    }




