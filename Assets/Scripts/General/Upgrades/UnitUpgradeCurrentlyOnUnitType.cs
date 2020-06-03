using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgradeCurrentlyOnUnitType : MonoBehaviour
{
    [SerializeField] int[] upgradesActive;  // Refrences "allUpgrades" in class UnitUpgrades
    [SerializeField] string unitType; // reference to unit type(Archer, Soldier, etc);
    [SerializeField] Sprite displaySprite;
    [SerializeField] GameObject unitPrefab; // For collecting data in UI;

    public int[] GetUnitUpgrades()
    {
        return upgradesActive;
    }

    public string GetUnitType()
    {
        return unitType;
    }

    public GameObject GetUnitPrefab()
    {
        return unitPrefab;
    }

    public Sprite GetDisplaySprite()
    {
        return displaySprite;
    }

    public void SetUpgrade(int unitUpgradeIndex, int allUpgradesIndex)
    {
        if (unitUpgradeIndex > upgradesActive.Length || allUpgradesIndex > UnitUpgrades.instance.GetAllUpgradesList().Length) return;
        upgradesActive[unitUpgradeIndex] = allUpgradesIndex;

    }

    public void SetUnitUpgrades(int[] upgrades)
    {
        upgradesActive = upgrades;
    }
}
