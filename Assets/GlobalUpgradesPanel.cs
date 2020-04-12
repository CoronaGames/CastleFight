using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradesPanel : MonoBehaviour
{
    [SerializeField] Text[] levelTexts;


    private void OnEnable()
    {
        UpdateLevelTexts();
    }


    public void UpdateLevelTexts()
    {
        for (int i = 0; i < levelTexts.Length; i++)
        {
            levelTexts[i].text =
                GlobalUpgrades.instance.GetLevelOnIndex(i) + "/" + 
                (GlobalUpgrades.instance.GetMaxLevelOnIndex(i)-1);

        }
    }
}
