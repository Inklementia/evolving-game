using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public enum BiomeHeart
        {
            None,
            Hot,
            Cold
        }

        public enum BiomeTile
        {
            Empty,
            Hot,
            Cold,
            Green
        }

        public class BiomeTileData
        {
            public Vector3Int position;
            public int temperatureValue;

            public BiomeHeart biomeHeart;
            public BiomeTile biomeTile;
            public TileBase tileVisual;

            public bool IsSource => biomeHeart != BiomeHeart.None;

            public BiomeTileData(Vector3Int pos, int temp, BiomeHeart heart, TileBase visual)
            {
                position = pos;
                temperatureValue = temp;
                tileVisual = visual;

                biomeTile = DetermineBiomeTile(temp);
                biomeHeart = ValidateHeart(biomeTile, heart);
            }

            public void UpdateTemperature(int delta, System.Func<int, TileBase> visualResolver)
            {
                temperatureValue += delta;

                biomeTile = DetermineBiomeTile(temperatureValue);
                biomeHeart = IsSource ? biomeHeart : ValidateHeart(biomeTile, BiomeHeart.None);
                tileVisual = visualResolver?.Invoke(temperatureValue);
            }

            private BiomeTile DetermineBiomeTile(int temp)
            {
                if (temp > 0) return BiomeTile.Hot;
                if (temp < 0) return BiomeTile.Cold;
                if (temp == 0) return BiomeTile.Green;
                return BiomeTile.Empty;
            }

            private BiomeHeart ValidateHeart(BiomeTile tile, BiomeHeart heart)
            {
                switch (tile)
                {
                    case BiomeTile.Hot:
                        return heart == BiomeHeart.Hot ? BiomeHeart.Hot : BiomeHeart.None;
                    case BiomeTile.Cold:
                        return heart == BiomeHeart.Cold ? BiomeHeart.Cold : BiomeHeart.None;
                    case BiomeTile.Green:
                    case BiomeTile.Empty:
                    default:
                        return BiomeHeart.None;
                }
            }
        }
    }




