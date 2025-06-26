using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class PowerHolder : SingletonClass<PowerHolder>
    { 
        public int CurrentHeartTemp = 3;
        public int CurrentHabitatValue = 0;
        
        public bool CanUpgradeHeartTemp { get; private set; }
        public bool CanUpgradeHabitatValue { get; private set; }

        private int _maxTemp = 5;
        private int _maxHabitatValue = 4; // 5 - 1;

        private void Start()
        {
            CanUpgradeHeartTemp = true;
            CanUpgradeHabitatValue = true;
            CurrentHabitatValue = 0;
            CurrentHeartTemp = 3;
        }

        public bool IncreaseHeartTemp()
        {
            if (CurrentHeartTemp == _maxTemp - 1)
            {
                CanUpgradeHeartTemp = false;
                CurrentHeartTemp += 1;
                return true;
            }

            if (CurrentHeartTemp < _maxTemp - 1)
            {
                CurrentHeartTemp += 1;
                CanUpgradeHeartTemp = true;
                return true;
            }
            CanUpgradeHeartTemp = false;
            return false;
        }

        public bool IncreaseHabitatValue()
        {
          
                if (CurrentHabitatValue == _maxHabitatValue - 1)
                {
                    CanUpgradeHabitatValue = false;
                    CurrentHabitatValue += 1;
                    return true;
                }

                if (CurrentHabitatValue < _maxHabitatValue - 1)
                {
                    if (CurrentHabitatValue == CurrentHeartTemp - 2)
                    {
                        CanUpgradeHabitatValue = false;
                    }
                    else
                    {
                        CanUpgradeHabitatValue = true;
                    }
                 
                    CurrentHabitatValue += 1;
                    return true;
                }
                Debug.Log("here 2");
                return false;
        }
    }
}