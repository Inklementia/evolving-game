using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class UIBiomeSelector : MonoBehaviour
    {
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private BiomeCreator biomeCreator;

        private void Start()
        {
            foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                toggle.onValueChanged.AddListener(isOn => OnToggleChanged(toggle, isOn));
            }
        }

        private void OnToggleChanged(Toggle changedToggle, bool isOn)
        {
            if (isOn)
            {
                UIBiomeToggleValue toggleValue = changedToggle.GetComponent<UIBiomeToggleValue>();
                if (toggleValue != null)
                {
                    Debug.Log("Selected value: " + toggleValue.BiomeHeart);
                    ApplyValue(toggleValue.BiomeHeart);
                }
                else
                {
                    Debug.LogWarning("Toggle without ToggleValue component!");
                }
            }
        }

        private void ApplyValue(BiomeHeart biome)
        {
            biomeCreator.SetCurrentBiomeMode(biome);
        }
    }
    
}