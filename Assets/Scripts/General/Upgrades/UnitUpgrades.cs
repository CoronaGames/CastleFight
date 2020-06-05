using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgrades : MonoBehaviour
{
    public static UnitUpgrades instance;
    
    [SerializeField] UnitUpgrade[] allUpgrades; // An array of all unit upgrades in the game;
    //[SerializeField] UnitUpgrade[] availableUpgrades;
    [SerializeField] int[] availableUpgrades;
    [SerializeField] int[] storeInventory; // Each number references index in "allUpgrades" list.
    // Showcased in "unit upgrades" UI panel, also the players currently available upgrades; 
    [SerializeField] UnitUpgradeCurrentlyOnUnitType[] unitsArray;


    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UnitUpgrade[] GetAllUpgradesList()
    {
        return allUpgrades;
    }

    public UnitUpgradeCurrentlyOnUnitType[] GetUnitsArray()
    {
        return unitsArray;
    }

    /*
    public UnitUpgrade[] GetAvailableUpgradesList()
    {
        return availableUpgrades;
    }
    */

    public int[] GetAvailableUpgradesList()
    {
        return availableUpgrades;
    }

    public void AddToAvailableUpgrades(int indexToAdd) // Index reference from "allUpgrades"
    {

        for(int i=0; i < availableUpgrades.Length; i++)
        {
            if(allUpgrades[availableUpgrades[i]].GetUpgradeType() == UpgradeType.None)
            {
                availableUpgrades[i] = indexToAdd;
                return;
            }
        }
    }

    public void AddToStore(int indexToAdd) // Index reference from "allUpgrades"
    {

        for (int i = 0; i < storeInventory.Length; i++)
        {
            if (allUpgrades[storeInventory[i]].GetUpgradeType() == UpgradeType.None)
            {
                storeInventory[i] = indexToAdd;
                return;
            }
        }
    }

    public void RemoveFromAvailableUpgrades(int indexToRemove) // Index reference from "availableUpgrades"
    {
          availableUpgrades[indexToRemove] = 0;
        SortAvailableUpgradesList();
    }

    public void AddUpgradesToUnit(Health unit)  // Called when units spawns ingame;
    {
        int unitsArrayIndex = 0;
        bool succes = true;
        // Check if unit type has upgrades
        for(int i=0; i<unitsArray.Length; i++)
        {
            if (unitsArray[i].GetUnitType() == unit.GetName())
            {
                unitsArrayIndex = i;
                break;
            }
        }
        // Check if unit type has upgrades
        if(!(unitsArray[unitsArrayIndex].GetUnitUpgrades().Length > 0))
        {
            return;
        }

        for(int i=0; i<unitsArray[unitsArrayIndex].GetUnitUpgrades().Length; i++)
        {
            // Check upgrade and add to unit.
           AddUpgrade(unitsArray[unitsArrayIndex].GetUnitUpgrades()[i], unit);
            
        }
            
    }

    private bool AddUpgrade(int upgradeReference, Health unit)
    {
        if(allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.None)
        {
            // Used For Empty Slots / do nothing
        }
        else if (allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.Defense)
        {

        }
        else if (allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.Health)
        {
            unit.AddMaxHealth(allUpgrades[upgradeReference].GetUpgradeValue());
        }
        else if (allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.Speed)
        {
            unit.GetComponent<Mover>().IncreaseMoveSpeed(allUpgrades[upgradeReference].GetUpgradeValue());
        }
        else if (allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.Damage)
        {
            unit.GetComponent<Attacker>().AddToBaseDamage(allUpgrades[upgradeReference].GetUpgradeValue());
        }
        else if (allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.CriticalChance)
        {
            unit.GetComponent<Attacker>().IncreaseCriticalChance(allUpgrades[upgradeReference].GetUpgradeValue());
        }
        else if (allUpgrades[upgradeReference].GetUpgradeType() == UpgradeType.CriticalDamage)
        {
            unit.GetComponent<Attacker>().IncreaseCriticalDamageMultiplier(allUpgrades[upgradeReference].GetUpgradeValue());
        }
        else
        {
            // No Upgrade took place
            return false;
        }
        return true;
    }

    public int[] GetSaveArrayInventory() // Used for saving inventory data
    {
        int[] saveArray = new int[unitsArray.Length];
        for(int i=0; i< saveArray.Length; i++)
        {
            
            for(int j=0; i< allUpgrades.Length; j++)    // j = allupgradesIndex;
            {
                if(availableUpgrades[i] == j)   // Potesneill feil?
                {
                    saveArray[i] = j;
                    break;
                }
            }
        }
        return saveArray;
    }

    public int[,] GetSaveArrayUnitUpgrades() // Used for saving unit upgrade data 
    {
        int[,] saveArray = new int[unitsArray.Length, 3];
        for(int i=0; i<unitsArray.Length; i++)
        {
            for(int j=0; j<3; j++)
            {
                saveArray[i, j] = unitsArray[i].GetUnitUpgrades()[j];
            }
        }

        return saveArray;
    } 

    public void SetInventory(int[] inventoryData)
    {
        if (inventoryData == null)
        {
            Debug.LogError("InventoryData equals null");
            return;
        }
        int[] inventory = inventoryData;
        for(int i=0; i< inventory.Length; i++)
        {
            availableUpgrades[i] = inventory[i];
        }
    }

    public void SetUnitUpgrades(int[,] upgradeData)
    {
        if (upgradeData == null)
        {
            Debug.LogError("UpgradeData equals null");
            return;
        }

        int[,] data = upgradeData;
        int uBound0 = data.GetUpperBound(0);
        int uBound1 = data.GetUpperBound(1);

        int[] current;

        for (int i=0; i<=uBound0; i++)
        {
            current = new int[3];
            for(int j = 0; j <= uBound1; j++)
            {
                current[j] = data[i, j]; 
            }
            unitsArray[i].SetUnitUpgrades(current);
        }
    }

    public int[] GetStoreInventory()
    {
        return storeInventory;
    }

    public void SetStoreInventory(int[] storeInventory)
    {
        this.storeInventory = storeInventory;
    }

    public void SortAvailableUpgradesList()
    {
        int index = 0;
        int[] bufferList = new int[availableUpgrades.Length];
        for(int i=0; i<availableUpgrades.Length; i++)
        {
            if(availableUpgrades[i] != 0)
            {
                bufferList[index] = availableUpgrades[i];
                index++;
            }
        }
        availableUpgrades = bufferList;
    }

    public void SortStoreInvenotryList()
    {
        int index = 0;
        int[] bufferList = new int[storeInventory.Length];
        for (int i = 0; i < storeInventory.Length; i++)
        {
            if (storeInventory[i] != 0)
            {
                bufferList[index] = storeInventory[i];
                index++;
            }
        }
        storeInventory = bufferList;
    }
}
