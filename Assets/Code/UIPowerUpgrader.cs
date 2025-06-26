using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Code
{
    public class UIPowerUpgrader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI upgradeTempInfoText;
        [SerializeField] private TextMeshProUGUI upgradeHabitatInfoText;
        
        [SerializeField] private Button upgradeTempButton;
        [SerializeField] private Button upgradeHabitatButton;
        
        [SerializeField] private BiomeCreator biomeCreator;
        
        private void Start()
        {
            upgradeTempButton.onClick.AddListener(UpgradeBiomeHeartTemp);
            upgradeTempInfoText.text = $"Heart Temp: +/- {PowerHolder.Instance.CurrentHeartTemp}";
            
            upgradeHabitatButton.onClick.AddListener(UpgradeBiomeHabitatValue);
            upgradeHabitatInfoText.text =
                $"Living Temps: {GenerateInfoText(PowerHolder.Instance.CurrentHabitatValue)}";
        }

        private void UpgradeBiomeHeartTemp()
        {
            PowerHolder.Instance.IncreaseHeartTemp();
            biomeCreator.UpdateMaxTemperature();
            
            upgradeTempInfoText.text = $"Heart Temp: +/- {PowerHolder.Instance.CurrentHeartTemp}";
            upgradeTempButton.interactable = PowerHolder.Instance.CanUpgradeHeartTemp;
            
        }

        private void UpgradeBiomeHabitatValue()
        {
            PowerHolder.Instance.IncreaseHabitatValue();
            biomeCreator.UpdateMap();

            
            upgradeHabitatInfoText.text =
                $"Living Temps: {GenerateInfoText(PowerHolder.Instance.CurrentHabitatValue)}";
            upgradeHabitatButton.interactable = PowerHolder.Instance.CanUpgradeHabitatValue;
        }
        
        private string GenerateInfoText(int maxValue)
        {
            List<string> parts = new List<string>();
            parts.Add("0");

            for (int i = 1; i <= maxValue; i++)
            {
                parts.Add($"+{i}");
                parts.Add($"-{i}");
            }

            return string.Join(", ", parts);
        }


    }
}