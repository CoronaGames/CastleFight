using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUpgrades : MonoBehaviour
{
    // Upgrades which is bought over and over for each game;

    public static BattleUpgrades instance;

    public float damageBoost = 0, moveSpeedBoost = 0, healthBoost = 0, criticalDamageBoost = 0, criticalChanceBoost = 0;
    [SerializeField] BattleUpgrade[] battleUpgrades;
    // Start is called before the first frame update
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

    public bool BuyUpgrade(int indexToBuy)
    {
        if (CastleFightData.instance.ReduceMoney(battleUpgrades[indexToBuy].GetUpgradeCost()))
        {
            ActivateUpgrade(indexToBuy);
            battleUpgrades[indexToBuy].IncrementUpgradeLevel();
            return true;
        }
        else
        {
            // Cant afford upgrade;
            return false;
        }
    }

    private void ActivateUpgrade(int indexToActivate)
    {
        UpgradeType upgradeType = battleUpgrades[indexToActivate].GetUpgradeType();
        if (upgradeType == UpgradeType.CriticalChance)
        {
            criticalChanceBoost += battleUpgrades[indexToActivate].GetUpgradeValue();
        }
        else if (upgradeType == UpgradeType.CriticalDamage)
        {
            criticalDamageBoost += battleUpgrades[indexToActivate].GetUpgradeValue();
        }
        else if (upgradeType == UpgradeType.Damage)
        {
            damageBoost += battleUpgrades[indexToActivate].GetUpgradeValue();
        }
        else if (upgradeType == UpgradeType.Defense)
        {

        }
        else if (upgradeType == UpgradeType.Health)
        {
            healthBoost += battleUpgrades[indexToActivate].GetUpgradeValue();
        }
        else if (upgradeType == UpgradeType.Speed)
        {
            moveSpeedBoost += battleUpgrades[indexToActivate].GetUpgradeValue();
        }
        else if (upgradeType == UpgradeType.MaxUnits)
        {
            CastleFightData.instance.IncrementMaxUnits();
            return;
        }
        UpgradeActiveUnits(indexToActivate, upgradeType);
    }

    public void UpgradeActiveUnits(int indexToActivate, UpgradeType upgradeType)
    {
        LinkedList<Health> unitsToUpgrade = CastleFightData.instance.GetActiveUnitsPlayer();

        if (upgradeType == UpgradeType.CriticalChance)
        {
            foreach(Health unit in unitsToUpgrade)
            {
                unit.GetComponent<Attacker>().IncreaseCriticalChance(battleUpgrades[indexToActivate].GetUpgradeValue());
            }
        }
        else if (upgradeType == UpgradeType.CriticalDamage)
        {
            foreach (Health unit in unitsToUpgrade)
            {
                unit.GetComponent<Attacker>().IncreaseCriticalDamageMultiplier(battleUpgrades[indexToActivate].GetUpgradeValue());
            }
        }
        else if (upgradeType == UpgradeType.Damage)
        {
            foreach (Health unit in unitsToUpgrade)
            {
                unit.GetComponent<Attacker>().AddToBaseDamage(battleUpgrades[indexToActivate].GetUpgradeValue());
            }
        }
        else if (upgradeType == UpgradeType.Defense)
        {

        }
        else if (upgradeType == UpgradeType.Health)
        {
            foreach (Health unit in unitsToUpgrade)
            {
                unit.AddMaxHealth(battleUpgrades[indexToActivate].GetUpgradeValue());
                if (!unit.HasMaxHp())
                {
                    unit.Heal(battleUpgrades[indexToActivate].GetUpgradeValue());
                }
            }
        }
        else if (upgradeType == UpgradeType.Speed)
        {
            foreach (Health unit in unitsToUpgrade)
            {
                unit.GetComponent<Mover>().IncreaseMoveSpeed(battleUpgrades[indexToActivate].GetUpgradeValue());
            }
        }
    }

    public float GetUpgradeValue(int index)
    {
        return battleUpgrades[index].GetUpgradeValue();
    }

    public int GetUpgradeLevelAtIndex(int index)
    {
        return battleUpgrades[index].GetUpgradeLevel();
    }

    public BattleUpgrade[] GetBattleUpgrades()
    {
        return battleUpgrades;
    }

    public BattleUpgrade GetBattleUpgradeAtIndex(int index)
    {
        return battleUpgrades[index];
    }

    public void ResetUpgrades()
    {
        for(int i=0; i< battleUpgrades.Length; i++)
        {
            battleUpgrades[i].ResetUpgrade();
        }
        damageBoost = 0f;
        moveSpeedBoost = 0f;
        criticalDamageBoost = 0f;
        criticalChanceBoost = 0f;
        healthBoost = 0f;
    }
}
