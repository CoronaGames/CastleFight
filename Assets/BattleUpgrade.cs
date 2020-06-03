using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUpgrade : MonoBehaviour
{
    [SerializeField] int upgradeCost;
    [SerializeField] string upgradeName;
    [SerializeField] string upgradeDescription;
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] float upgradeValue;
    [SerializeField] int upgradeLevel = 0;
    [SerializeField] Sprite upgradeIcon;
    [SerializeField] float currentBoost = 0f;
    [SerializeField] bool convertValueToPercentage = false;

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }

    public float GetUpgradeValue()
    {
        return upgradeValue;
    }

    public UpgradeType GetUpgradeType()
    {
        return upgradeType;
    }

    public string GetUpgradeName()
    {
        return upgradeName;
    }

    public string GetUpgradeDescription()
    {
        string description;
        if (convertValueToPercentage)
        {
            description = "Go from <color=green>" + (currentBoost*100) + " %</color> to <color=green>" + ((upgradeValue + currentBoost) * 100) + "% </color> " + upgradeDescription;
        }
        else
        {
            description = "Go from <color=green>" + currentBoost + " </color> to <color=green>" + (upgradeValue + currentBoost) + "</color> " + upgradeDescription;
        }

        description += "\n\n Cost: <color=green>" + upgradeCost + "</color> coins."; 
        
        return description;
    }

    public void IncrementUpgradeLevel()
    {
        upgradeLevel++;
        currentBoost += upgradeValue;
    }

    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

    public Sprite GetUpgradeSprite()
    {
        return upgradeIcon;
    }

    public float GetCurrentBoost()
    {
        return currentBoost;
    }

    public void ResetUpgrade()
    {
        upgradeLevel = 0;
        currentBoost = 0f;
    }
}
