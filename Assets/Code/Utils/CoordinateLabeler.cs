namespace Code
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using TMPro;

    public class CoordinateLabeler : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameObject labelPrefab;

        private void Start()
        {
            PlaceLabels();
        }

        private void PlaceLabels()
        {
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;

                Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
                GameObject label = Instantiate(labelPrefab, worldPos, Quaternion.identity, transform);

                TextMeshProUGUI tmp = label.GetComponent<TextMeshProUGUI>();
                if (tmp == null)
                {
                    Debug.LogError("Missing TextMeshPro on labelPrefab!");
                    return;
                }

                tmp.text = $"({pos.x}, {pos.y})";
            }
        }
        
      

    }

}