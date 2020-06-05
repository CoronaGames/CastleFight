using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ProgressData
{
    // Data to save/Serialize
    [Header("Main Data")]
    public int[] levelScore;  // List of levelstars/score. Array index number represent level number, and index value represent number of stars/points from 0 - 3;
    public float[] levelTimer;
    public string playerName;
    public int totalStars = 0;
    public int totalBounty = 0;

    [Header("Unit Upgrades")]
    public int[] upgradesInventory;         // UnitUpgrades.AvailableUpgrades[];
    public int[,] upgradeOnUnitType;     // UnitUpgradeCurrentlyOnUnitType

    [Header("Global Upgrades")]
    public int[] globalUpgrades;

    [Header("Store Inventory")]
    public int[] storeInventory;

    public bool hasSaved;

    public ProgressData(MainData mainData)
    {
        // TODO:

        levelScore = mainData.levelScore;
        levelTimer = mainData.levelTimer;
        playerName = mainData.playerName;
        totalStars = mainData.totalStars;
        totalBounty = mainData.totalBounty;
        
        hasSaved = mainData.hasSaved;

        upgradesInventory = UnitUpgrades.instance.GetSaveArrayInventory();
        upgradeOnUnitType = UnitUpgrades.instance.GetSaveArrayUnitUpgrades();
        storeInventory = UnitUpgrades.instance.GetStoreInventory();


        globalUpgrades = GlobalUpgrades.instance.GetSaveArrayUpgrades();

    }

    public ProgressData()
    {
        levelScore = new int[10];
        levelTimer = new float[10];
        playerName = "";
        totalStars = 0;
        totalBounty = 0;
        hasSaved = false;

        upgradesInventory = new int[0];
        upgradeOnUnitType = new int[3,3]; // 3 unit types with 3 upgrades each
        storeInventory = new int[0];

        globalUpgrades = new int[5];
    }
}
