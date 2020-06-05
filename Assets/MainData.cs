using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainData : MonoBehaviour
{
    public static MainData instance;
    // Main data class containing essential variables needed to save game
    [Header("Main Data")]
    public int[] levelScore;  // List of levelstars/score. Array index number represent level number, and index value represent number of stars/points from 0 - 3;
    public float[] levelTimer;
    public string playerName;
    public int totalStars = 0;
    public int totalBounty = 0;

    [Header("Unit Upgrades")]
    public int[] upgradesInventory;         // UnitUpgrades.AvailableUpgrades[];
    public int[,] upgradesOnUnitType;

    [Header("Global Upgrades")]
    public int[] globalUpgrades;

    [Header("Used for main menu")]
    public bool hasSaved = false; // Used by main menu to determine if save file exists.

    [Header("Store Inventory")]
    public int[] storeInventory;

    [SerializeField] bool activateChanges = false; // Used to trigger changes in inspector in runtime.

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        LoadData();
    }

    private void Update()
    {
        if (activateChanges)
        {
            SetUnitUpgradesData();
            SetGlobalUpgradesData();
            activateChanges = false;
            Debug.Log("Changes Updated");
        }
    }

    public void UpdateTotalStars()  // Calculates Player stars total
    {
        int score = 0;
        for(int i=0; i<levelScore.Length; i++)
        {
            score += levelScore[i];
        }
        totalStars = score;
    }

    public void SaveData()
    {
        hasSaved = true;
        SaveSystem.SaveGameData(this);
        Debug.Log("Game Saved");
    }

    public void LoadData()
    {
        ProgressData progressData = SaveSystem.LoadGameData();

        levelScore = progressData.levelScore;
        playerName = progressData.playerName;
        totalStars = progressData.totalStars;
        totalBounty = progressData.totalBounty;

        upgradesInventory = progressData.upgradesInventory;
        upgradesOnUnitType = progressData.upgradeOnUnitType;
        storeInventory = progressData.storeInventory;

        globalUpgrades = progressData.globalUpgrades;

        SetUnitUpgradesData();
        SetGlobalUpgradesData();


        Debug.Log("Game Data Loaded");
    }

    private void SetUnitUpgradesData()
    {
        UnitUpgrades.instance.SetInventory(upgradesInventory);
        UnitUpgrades.instance.SetUnitUpgrades(upgradesOnUnitType);
        UnitUpgrades.instance.SetStoreInventory(storeInventory);
    }

    private void SetGlobalUpgradesData()
    {
        GlobalUpgrades.instance.LoadGlobalUpgrades(globalUpgrades);
    }

    public void AddScore(int level, int score)
    {
        levelScore[level] = score;

    }

    public void AddStar()
    {
        totalStars ++;
        CastleFightGui.instance.SetStarsText();
    }

    public void AddBounty()
    {
        totalBounty++;
    }

    public void AddBounty(int amountToAdd)
    {
        totalBounty += amountToAdd;
    }
}
