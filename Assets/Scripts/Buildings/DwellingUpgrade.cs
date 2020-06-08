using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwellingUpgrade : MonoBehaviour
{
    [SerializeField] string upgradeName;
    [SerializeField] string description;
    [SerializeField] int[] prices;  // Prices at level 1, 2 and 3.
    [SerializeField] int currentLevel = 0;
    [SerializeField] Sprite upgradeSprite;
    [SerializeField] float[] upgradeVariables; //Variable to change some value on upgrades (Damage, HP, etc...)
    [SerializeField] bool binaryUpgrade = false;
    [SerializeField] UpgradeType upgradeType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public string GetUpgradeName()
    {
        return upgradeName;
    }

    public string GetDescription()
    {
        return description;
    }

    public int[] GetPrices()
    {
        return prices;
    }

    public int GetUpgradePriceCurrentLevel()
    {
        return prices[currentLevel];
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public Sprite GetUpgradeSprite()
    {
        return upgradeSprite;
    }

    public float[] GetUpgradeVariables()
    {
        return upgradeVariables;
    }

    public float GetUpgradeVariableCurrentLevel()
    {
        if (currentLevel == 0) return 0f;
        return upgradeVariables[currentLevel - 1];
    }

    public bool IsBinaryUpgrade()
    {
        return binaryUpgrade;
    }

    public UpgradeType GetUpgradeType()
    {
        return upgradeType;
    }

    public void IncrementLevel()
    {
        currentLevel++;
    }


    public void SetLevel(int level)
    {
        currentLevel = level;
    }
}
