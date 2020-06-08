using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DwellingUpgrader : MonoBehaviour
{
    [SerializeField] DwellingUpgrade[] upgrades;
    [SerializeField] GameObject[] buttons;
    [SerializeField] Image[] upgradeSprites;
    [SerializeField] Text[] priceTags;
    [SerializeField] int sellValue;
    [SerializeField] GeneralTooltip[] tooltips;
    [SerializeField] LevelIndicator[] levelIndicators;

    void Start()
    {
        SetUpUpgrades();
    }

    private void SetUpUpgrades()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if(upgrades[i] == null)
            {
                buttons[i].SetActive(false);
                continue;
            }
            upgrades[i].SetLevel(0);
            upgradeSprites[i].sprite = upgrades[i].GetUpgradeSprite();
            priceTags[i].text = upgrades[i].GetPrices()[upgrades[i].GetCurrentLevel()].ToString();
            tooltips[i].gameObject.SetActive(true);
            tooltips[i].SetTitle(upgrades[i].GetUpgradeName());
            tooltips[i].SetDescription(upgrades[i].GetDescription());
            tooltips[i].gameObject.SetActive(false);
            if (upgrades[i].IsBinaryUpgrade()) levelIndicators[i].SetBinaryUpgrade();
        }
    }

    public void BuyUpgrade(int upgradeIndex)
    {

        if(CastleFightData.instance.playerMoney >= upgrades[upgradeIndex].GetUpgradePriceCurrentLevel())
        {
            upgrades[upgradeIndex].IncrementLevel();
            UpdateUpgradeButton(upgradeIndex);
            levelIndicators[upgradeIndex].LevelUp();
            
        }
        else
        {
            CastleFightGui.instance.SetInfoText("Cant afford upgrade");
        }
      
    }

    public void SellDwelling()
    {

    }

    public void UpdateUpgradeButton(int buttonToUpdate) // Updates graphic on upgrade levels
    {
        if (upgrades[buttonToUpdate].GetCurrentLevel() >= 3 || upgrades[buttonToUpdate].IsBinaryUpgrade())
        {
            buttons[buttonToUpdate].GetComponent<Button>().interactable = false;
            upgradeSprites[buttonToUpdate].color = new Color(1f, 1f, 1f, 0.25f);
        }
        else
        {
            priceTags[buttonToUpdate].text = upgrades[buttonToUpdate].GetPrices()[upgrades[buttonToUpdate].GetCurrentLevel()].ToString();
            tooltips[buttonToUpdate].SetDescription(upgrades[buttonToUpdate].GetDescription());
        }
        
    }
}
