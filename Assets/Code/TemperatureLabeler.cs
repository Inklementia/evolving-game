using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

namespace Code
{
    public class TemperatureLabeler : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameObject labelPrefab;
        [SerializeField] private BiomeCreator biomeCreator; // Ссылка на BiomeCreator

        private readonly Dictionary<Vector3Int, GameObject> labelInstances = new();

        private void Start()
        {
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            foreach (var pair in biomeCreator.GetBiomeTiles())
            {
                var pos = pair.Key;
                var data = pair.Value;

                if (!tilemap.HasTile(pos)) continue;

                if (!labelInstances.TryGetValue(pos, out var label))
                {
                    Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
                    label = Instantiate(labelPrefab, worldPos, Quaternion.identity, transform);
                    labelInstances[pos] = label;
                }

                var tmp = label.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.text = $"<color={ColorFromTemp(data.Temperature)}>{data.Temperature}</color>";

                }
            }
        }
        
        private string ColorFromTemp(int temp)
        {
            if (temp == 0) return "green";
            return temp > 0 ? "red" : "blue";
        }

    }
}
