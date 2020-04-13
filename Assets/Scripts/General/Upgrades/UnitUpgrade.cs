using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgrade : MonoBehaviour
{
    [SerializeField] int upgradeId; // Uniqe for each upgrade;
    [SerializeField] Sprite symbol;
    [SerializeField] string upgradeName;
    [SerializeField] string description;
    [SerializeField] bool equippable;
    [SerializeField] string[] worksOnUnitTypes; // Determines if unit can use upgrade
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] float upgradeValue; // variable to modify data with respect to upgradeType (HP, damage, move speed, etc...);

    public UpgradeType GetUpgradeType()
    {
        return upgradeType;
    }

    public float GetUpgradeValue()
    {
        return upgradeValue;
    }

    public int GetUpgradeId()
    {
        return upgradeId;
    }

    public Sprite GetSprite()
    {
        return symbol;
    }

    public string GetTitle()
    {
        return upgradeName;
    }

    public string GetDescription()
    {
        return description;
    }
}
