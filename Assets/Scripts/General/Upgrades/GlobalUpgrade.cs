using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpgrade : MonoBehaviour
{
    [SerializeField] int upgradeLevel = 0;    // current level of the upgrade
    [SerializeField] float[] upgradeValue;  // variable "upgradeLevel" is the index of upgradeValue;
    [SerializeField] string upgradeName;
    [SerializeField] string upgradeDescription;
    [SerializeField] bool convertDescriptionToPercentage = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public float GetIndex(int index)
    {
        return upgradeValue[index];
    }

    public float GetCurrentValue()
    {
        return upgradeValue[upgradeLevel];
    }

    public bool UpgradeLevel()
    {
        if (upgradeValue.Length-1 <= upgradeLevel) return false;    // Return error or warning on GUI
        upgradeLevel++;
        return true;
    }

    public void SetUpgradeLevel(int level)
    {
        upgradeLevel = level;
    }

    public int GetLevel()
    {
        return upgradeLevel;
    }

    public int GetMaxLevel()
    {
        return upgradeValue.Length;
    }
    
    public string GetTitle()
    {
        return upgradeName;
    }

    public string GetUpgradeDescritpion()
    {
        string description;
        if (upgradeLevel < upgradeValue.Length-1)
        {
            float oldValue = upgradeValue[upgradeLevel];
            float newValue = upgradeValue[upgradeLevel + 1];
            if (convertDescriptionToPercentage)
            {
                oldValue *= 100f;
                newValue *= 100f;
                description = "Go from <color=green>" + oldValue + "%</color> to <color=green>" + newValue + "%</color> " + upgradeDescription;
            }
            else
            {
                description = "Go from <color=green>" + oldValue + "</color> to <color=green>" + newValue + "</color> " + upgradeDescription;
            }
            
        }
        else
        {
            if (convertDescriptionToPercentage)
            {
                float oldValue = upgradeValue[upgradeLevel] * 100f;
                description = "You have reached max level on this upgrade with <color=green>" + oldValue + "%</color> " + upgradeDescription;
            }
            else
            {
                description = "Max level reached with <color=green>" + upgradeValue[upgradeLevel] + "</color> " + upgradeDescription;
            }
        }

        return description;
    }
 
}
