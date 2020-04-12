using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUpgraderButton : MonoBehaviour
        , IPointerEnterHandler
        , IPointerExitHandler
{
    public Button button;
    [SerializeField] int buttonIndex = 0;
    DwellingUpgrader dwellingUpgrader;
    [SerializeField] GeneralTooltip toolTip;

    [SerializeField] string buttonTitle;
    [SerializeField] string buttonDescription;

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
        toolTip.SetDescription(buttonDescription);
        toolTip.SetTitle(buttonTitle);
    }
}
