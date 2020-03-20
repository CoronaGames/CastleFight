using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{

    public TargetMouseSelected targetMouseSelected;
    [SerializeField] GameObject uiObject;
    [SerializeField] bool isSelectable = true;
    [SerializeField] int waitForClicks = 0;  // Set to 1 if you want to wait one click before selecting, i.e so that using mouse to place tower dont also activate target slection.


    void Start()
    {
        targetMouseSelected = FindObjectOfType<TargetMouseSelected>();
    }

    public void SelectObject()
    {
        uiObject.SetActive(true);
    }

    public void DeSelectObject()
    {
        uiObject.SetActive(false);
    }

    public void SetSelectable(bool value)
    {
        isSelectable = value;
    }

    public bool IsSelectable()
    {
        return isSelectable;
    }

    public void SetWaitForClikcs(int clicks)
    {
        waitForClicks = clicks;
    }

    public int GetWaitForClicks()
    {
        return waitForClicks;
    }

    public void Clicked()
    {
        waitForClicks--;
    }


}
