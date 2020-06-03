using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManagerPanel : MonoBehaviour
{
    public static UnitManagerPanel instance;

    [SerializeField] int[] maxUnitTypes; // Index 0 = Archer, Index 1 = Soldier, Index 2 = Wizard;
    [SerializeField] Text[] unitNumbersChosen;
    [SerializeField] Text[] unitNumbersCurrent;
    [SerializeField] Button pauseSpawningButton;
    [SerializeField] Sprite spawningOnSprite;
    [SerializeField] Sprite spawningOffSprite;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        ResetUnitTypes();
    }

    public void AddOnIndex(int index)
    {
        if (CheckTotalOverflow()) return;
        maxUnitTypes[index]++;
        UpdateText(index);
    }

    public void ReduceOnIndex(int index)
    {
        if (maxUnitTypes[index] <= -1) return;
        maxUnitTypes[index]--;
        UpdateText(index);
    }

    private void UpdateText(int index)
    {
        if (maxUnitTypes[index] < 0) SetInfinite(index);
        else unitNumbersChosen[index].text = maxUnitTypes[index].ToString(); 
    }

    private void SetInfinite(int index)
    {
        unitNumbersChosen[index].text = "-";
    }

    public int GetMaxUnitsOnIndex(int index)
    {
        return maxUnitTypes[index];
    }

    private void ResetUnitTypes()
    {
        for(int i=0; i<maxUnitTypes.Length; i++)
        {
            maxUnitTypes[i] = -1;
            SetInfinite(i);
        }
    }

    private bool CheckTotalOverflow() // Check if total number of selected units is less or equal to game data max units variable;
    {
        int currentTotal = 0;
        for(int i=0; i<maxUnitTypes.Length; i++)
        {
            currentTotal += maxUnitTypes[i];
        }
        if(currentTotal >= CastleFightData.instance.GetMaxUnits())
        {
            return true;
        }
        return false;
    }

    public void SpawningButtonClicked()
    {
        if (CastleFightData.instance.IsPauseSpawningUnits())
        {
            CastleFightData.instance.SetPauseSpawningUnits(false);
            pauseSpawningButton.image.sprite = spawningOnSprite;
        }
        else
        {
            CastleFightData.instance.SetPauseSpawningUnits(true);
            pauseSpawningButton.image.sprite = spawningOffSprite;
        }
    }

    private void CheckSpawningButton()
    {
        if (CastleFightData.instance.IsPauseSpawningUnits())
        {
            pauseSpawningButton.image.sprite = spawningOffSprite;
        }
        else
        {
            pauseSpawningButton.image.sprite = spawningOnSprite;
        }
    }

    public void ResetIndex(int index)
    {
        maxUnitTypes[index] = -1;
        UpdateText(index);
    }

    public void UpdateCurrentUnits()
    {
        int[] numberOfUnits = CastleFightData.instance.GetAmountOfUnitTypes();
        for(int i=0; i<unitNumbersCurrent.Length; i++)
        {
            unitNumbersCurrent[i].text = numberOfUnits[i].ToString();
        }
    }
}
