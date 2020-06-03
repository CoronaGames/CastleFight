using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUpgradesPanel : MonoBehaviour
{
    [SerializeField] Button[] upgradeButtons;

    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeAtIndex(int index)
    {
        if (BattleUpgrades.instance.BuyUpgrade(index))
        {
            UpdateButtons();
        }
        else
        {
            // Cant afford, show in UI;
        }
    }

    public void UpdateButtons()
    {
        BattleUpgrade[] upgrades = BattleUpgrades.instance.GetBattleUpgrades();
        for(int i=0; i<upgradeButtons.Length; i++)
        {
            upgradeButtons[i].image.sprite = upgrades[i].GetUpgradeSprite();
            upgradeButtons[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription(upgrades[i].GetUpgradeName(), upgrades[i].GetUpgradeDescription());
            upgradeButtons[i].GetComponentInChildren<Text>().text = upgrades[i].GetUpgradeLevel().ToString();
        }
    }
}
