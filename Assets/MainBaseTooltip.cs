﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseTooltip : MonoBehaviour
{
    [SerializeField] Text tooltipTitle;
    [SerializeField] Text tooltipDescription;
    [SerializeField] float yAxisOffset = 100f;

    public void SetTitle(string description)
    {
        tooltipTitle.text = description;
    }

    public void SetDescription(string description)
    {
        tooltipDescription.text = description;
    }

    public void SetTooltipPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y + yAxisOffset, 0);
    }
}
