﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpgrades : MonoBehaviour
{
    public static GlobalUpgrades instance;

    [SerializeField] GlobalUpgrade[] globalUpgradesList;
    // Index 0: Damage Upgrade
    // Index 1: Spawn Time Upgrade
    // Index 2: Crit Chance upgrade
    // Index 3: Attack Speed Upgrade
    // Index 4: Critical Damage Upgrade

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetUpgradeValueOnUpgradesIndex(int index)
    {
        return globalUpgradesList[index].GetCurrentValue();
    }

    public int GetLevelOnIndex(int index)
    {
        return globalUpgradesList[index].GetLevel();
    }

    public string GetDescritionOnIndex(int index)
    {
        return globalUpgradesList[index].GetUpgradeDescritpion();
    }

    public string GetTitleOnIndex(int index)
    {
        return globalUpgradesList[index].GetTitle();
    }

    public int GetMaxLevelOnIndex(int index)
    {
        return globalUpgradesList[index].GetMaxLevel();
    }

    public bool UpgradeIndex(int indexToUpgrade)
    {
        if (MainData.instance.totalStars > 0)
        {
            if(globalUpgradesList[indexToUpgrade].GetLevel() < globalUpgradesList[indexToUpgrade].GetMaxLevel())
            {
                bool success = globalUpgradesList[indexToUpgrade].UpgradeLevel();
                if (success)
                {
                    MainData.instance.totalStars--;
                    CastleFightGui.instance.SetStarsText();
                    return success;
                }
                return success;
            }
        }

        return false;
    }

    public void ResetGlobalUpgrades()
    {
        int starsToReclaim = 0;
        for(int i = 0; i<globalUpgradesList.Length; i++)
        {
            starsToReclaim += globalUpgradesList[i].GetLevel();
            globalUpgradesList[i].SetUpgradeLevel(0);
        }
        MainData.instance.totalStars += starsToReclaim;
        CastleFightGui.instance.SetStarsText();
    }
}