using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgrades : MonoBehaviour
{
    public static UnitUpgrades instance;
    
    [SerializeField] UnitUpgrade[] allUpgrades; // An array of all unit upgrades in the game;
    [SerializeField] UnitUpgrade[] availableUpgrades;
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

    public UnitUpgrade[] GetAvailableUpgradesList()
    {
        return availableUpgrades;
    }

    public void AddToAvailableUpgrades(int indexToAdd) // Index reference from "allUpgrades"
    {
        for(int i=0; i < availableUpgrades.Length; i++)
        {
            if(availableUpgrades[i].GetUpgradeType() == UpgradeType.None)
            {
                availableUpgrades[i] = allUpgrades[indexToAdd];
                return;
            }
        }
    }

    public void RemoveFromAvailableUpgrades(int indexToRemove) // Index reference from "availableUpgrades"
    {
          availableUpgrades[indexToRemove] = allUpgrades[0];
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

}
