using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradesPanel : MonoBehaviour
{
    [SerializeField] int currentlySelected = 0; // References "unitsArray" in UnitUprads class
    [SerializeField] Image unitSprite;
    [SerializeField] Text unitNameText;
    [SerializeField] Button[] activeUpgrades;
    [SerializeField] Button[] abilitiesAvailableList;
    // Start is called before the first frame update
    UnitUpgradeCurrentlyOnUnitType unitSelected;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        RefreshPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPanel()
    {
        SetUpUnitDisplay();
        SetUpButtons();
    }

    public void RemoveActiveUpgrade(int index)    // Used for buttons/active upgrades 
    {
        int bufferValue = unitSelected.GetUnitUpgrades()[index];
        UnitUpgrades.instance.AddToAvailableUpgrades(bufferValue);
        unitSelected.GetUnitUpgrades()[index] = 0;

        RefreshPanel();
    }

    public void SelectAvailableUpgrade(int index)    // Used for buttons 
    {
        bool success = false;
        for(int i=0; i < unitSelected.GetUnitUpgrades().Length; i++)
        {
            if(unitSelected.GetUnitUpgrades()[i] == 0)
            {
               
                unitSelected.GetUnitUpgrades()[i] = UnitUpgrades.instance.GetAvailableUpgradesList()[index].GetUpgradeId();
                success = true;
                break;
            }
        }
        if (!success)
        {
            // Print error and return
            return;
        }

        UnitUpgrades.instance.RemoveFromAvailableUpgrades(index);
        RefreshPanel();
    }

    public void SetUpUnitDisplay()
    {
        unitSelected = UnitUpgrades.instance.GetUnitsArray()[currentlySelected];
        if (unitSelected == null) return;
        unitSprite.sprite = unitSelected.GetDisplaySprite();
        unitNameText.text = unitSelected.GetUnitType();
        UnitUpgrade[] upgradesReference = UnitUpgrades.instance.GetAllUpgradesList();
        int[] activeUpgReference = unitSelected.GetUnitUpgrades();
        for (int i=0; i< activeUpgReference.Length; i++)
        {
            activeUpgrades[i].image.sprite = upgradesReference[activeUpgReference[i]].GetSprite();
            if(activeUpgReference[i] == 0)
            {
                activeUpgrades[i].interactable = false;
            }
            else
            {
                if (activeUpgrades[i].GetComponent<UnitUpgradeButton>())
                {
                    activeUpgrades[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription
                        (
                            upgradesReference[activeUpgReference[i]].GetTitle(),
                            upgradesReference[activeUpgReference[i]].GetDescription()
                        );
                }
                activeUpgrades[i].interactable = true;
            }
        }

    }

    public void SetUpButtons()
    {
        UnitUpgrade[] upgradeList = UnitUpgrades.instance.GetAvailableUpgradesList();
        for(int i=0; i<abilitiesAvailableList.Length; i++)
        {
            abilitiesAvailableList[i].image.sprite = upgradeList[i].GetSprite();
            if(upgradeList[i].GetUpgradeType() == UpgradeType.None)
            {
                abilitiesAvailableList[i].interactable = false;
            }
            else
            {
                if (abilitiesAvailableList[i].GetComponent<UnitUpgradeButton>())
                {
                    abilitiesAvailableList[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription
                        (
                            upgradeList[i].GetTitle(),
                            upgradeList[i].GetDescription()
                        );
                }
                abilitiesAvailableList[i].interactable = true;
            }
        }

    }

    public void NextUnitButton(int value)   // 0 for left, 1 for right;
    {
        if(value == 0)
        {
            currentlySelected--;
        }
        else if(value == 1)
        {
            currentlySelected++;
        }
        else
        {
            return;
        }

        if(currentlySelected >= UnitUpgrades.instance.GetUnitsArray().Length)
        {
            currentlySelected = 0;
        }
        else if(currentlySelected < 0)
        {
            currentlySelected = UnitUpgrades.instance.GetUnitsArray().Length - 1;
        }

        SetUpUnitDisplay();
    }

}
