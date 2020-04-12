using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalUpgradeButton : MonoBehaviour
         , IPointerEnterHandler
        , IPointerExitHandler

{
    [SerializeField] Text levelText;
    [SerializeField] int globalUpgradeIndex;
    [SerializeField] GeneralTooltip toolTip;
    
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        //UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        bool success = GlobalUpgrades.instance.UpgradeIndex(globalUpgradeIndex);
        if (success)
        {
            UpdateText();
            UpdateToolTip();
        }
        else
        {
            // Print feedback message on GUI

        }
        
    }

    private void UpdateText()
    {
        levelText.text = GlobalUpgrades.instance.GetLevelOnIndex(globalUpgradeIndex) + " / " + (GlobalUpgrades.instance.GetMaxLevelOnIndex(globalUpgradeIndex)-1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);
        toolTip.gameObject.SetActive(true);
        UpdateToolTip();
        toolTip.SetTooltipPosition(new Vector2(transform.position.x, transform.position.y));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
        toolTip.gameObject.SetActive(false);
    }

    public void UpdateToolTip()
    {
        toolTip.SetDescription(GlobalUpgrades.instance.GetDescritionOnIndex(globalUpgradeIndex));
        toolTip.SetTitle(GlobalUpgrades.instance.GetTitleOnIndex(globalUpgradeIndex));
    }
}

