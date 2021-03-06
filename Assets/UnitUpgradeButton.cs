﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitUpgradeButton : MonoBehaviour
        , IPointerEnterHandler
        , IPointerExitHandler
{
    public Button button;
    [SerializeField] GeneralTooltip toolTip;
    [SerializeField] bool staticTooltipPosition = false;
    [SerializeField] string buttonTitle;
    [SerializeField] string buttonDescription;

    void Start()
    {
        button = GetComponent<Button>();
        //UpdateText();
    }

    public void SetTitleAndDescription(string title, string description)
    {
        buttonTitle = title;
        buttonDescription = description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);
        if (buttonTitle == "") return;
        toolTip.gameObject.SetActive(true);
        UpdateToolTip();
        if(!staticTooltipPosition) toolTip.SetTooltipPosition(new Vector2(transform.position.x, transform.position.y));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
        toolTip.gameObject.SetActive(false);
    }

    public void UpdateToolTip()
    {
        toolTip.SetDescription(buttonDescription);
        toolTip.SetTitle(buttonTitle);
    }
}
