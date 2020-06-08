using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DwellingButton : MonoBehaviour
        , IPointerEnterHandler
        , IPointerExitHandler
{
    public Button button;
    [SerializeField] int buttonIndex = 0;
    BasicDwelling dwellingUpgrader;
    // Start is called before the first frame update
    void Start()
    {
        dwellingUpgrader = GetComponentInParent<BasicDwelling>();
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);
        dwellingUpgrader.ActivateShowDemo(buttonIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
        dwellingUpgrader.DeactivateShowDemo();
    }
}
