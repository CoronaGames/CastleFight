using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BountyStore : MonoBehaviour
{

    [SerializeField] Button[] store;
    [SerializeField] Button[] inventory;
    [SerializeField] GameObject toolTip;
    [SerializeField] int noOfPages = 0;
    [SerializeField] int currentPage = 0;
    [SerializeField] Text currentPageText;
    UnitUpgradeCurrentlyOnUnitType unitSelected;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        CalculateStorePages();
        RefreshPanel();
    }

    private void CalculateStorePages()
    {
        int storeInventorySize = UnitUpgrades.instance.GetStoreInventory().Length;
        noOfPages = Mathf.RoundToInt(storeInventorySize / store.Length);
        if (noOfPages > 1) noOfPages--;
    }

    public void RefreshPanel()
    {
        SetUpInventory();
        SetUpStore();
        SetCurrentPageText();
    }

    public void RemoveActiveUpgrade(int index)    // Used for buttons/active upgrades 
    {
        int bufferValue = unitSelected.GetUnitUpgrades()[index];
        UnitUpgrades.instance.AddToAvailableUpgrades(bufferValue);
        unitSelected.GetUnitUpgrades()[index] = 0;
        toolTip.SetActive(false);
        RefreshPanel();
    }

    public void SelectAvailableUpgrade(int index)    // Used for buttons 
    {
        bool success = false;
        for (int i = 0; i < unitSelected.GetUnitUpgrades().Length; i++)
        {
            if (unitSelected.GetUnitUpgrades()[i] == 0)
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
        toolTip.SetActive(false);
        UnitUpgrades.instance.RemoveFromAvailableUpgrades(index);
        RefreshPanel();
    }


    public void SetUpInventory()
    {
        UnitUpgrade[] upgradeList = UnitUpgrades.instance.GetAvailableUpgradesList();
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i].image.sprite = upgradeList[i].GetSprite();
            if (upgradeList[i].GetUpgradeType() == UpgradeType.None)
            {
                inventory[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription("", "");
                inventory[i].interactable = false;
            }
            else
            {
                if (inventory[i].GetComponent<UnitUpgradeButton>())
                {
                    inventory[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription
                        (
                            upgradeList[i].GetTitle(),
                            upgradeList[i].GetDescription()
                        );
                }
                inventory[i].interactable = true;
            }
        }
    }

    public void SetUpStore()
    {
        int startIndex;
        int stopIndex;

        startIndex = store.Length * currentPage;    // page 1: 24 * 0 = 0, page 2: 24 * 1 = 24.
        stopIndex = store.Length + (store.Length * currentPage);    // Page 1: 24 + (24*0) = 24, Page 2: 24 + (24*1) = 48;

        UnitUpgrade[] upgradeList = UnitUpgrades.instance.GetAllUpgradesList();
        int[] storeInventory = UnitUpgrades.instance.GetStoreInventory();
        for (int i = 0; i < store.Length; i++)
        {
            store[i].image.sprite = upgradeList[storeInventory[i+startIndex]].GetSprite();
            if (upgradeList[storeInventory[i + startIndex]].GetUpgradeType() == UpgradeType.None)
            {
                store[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription("", "");
                store[i].interactable = false;
            }
            else
            {
                if (store[i].GetComponent<UnitUpgradeButton>())
                {
                    store[i].GetComponent<UnitUpgradeButton>().SetTitleAndDescription
                        (
                            upgradeList[storeInventory[i + startIndex]].GetTitle(),
                            upgradeList[storeInventory[i + startIndex]].GetDescription()
                        );
                }
                store[i].interactable = true;
            }
        }
    }

    public void NextStorePage(int value)   // 0 for left, 1 for right;
    {
        if (value == 0)
        {
            if (currentPage <= 0) return;
            else currentPage--;
        }
        else if (value == 1)
        {
            if (currentPage >= noOfPages) return;
            else currentPage++;
        }
        else
        {
            return;
        }

        SetCurrentPageText();
        SetUpStore();
    }

    private void SetCurrentPageText()
    {
        currentPageText.text = (currentPage + 1) + "/" + (noOfPages + 1);
    }
}
