using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUIcontroller : MonoBehaviour
{
    public Image[] button1Upgrades;
    public Image[] button2Upgrades;
    public Image[] button3Upgrades;

    public GameObject towerRadius;
    public CircleCollider2D circleCollider;
    private float imageToCircleColliderRatio = 8.743f;

    [SerializeField] Image[][] imagesList;

    [SerializeField] int[] buttonClicks;
    Canvas canvas;
    

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void ButtonOne()
    {
        if (buttonClicks[0] >= button1Upgrades.Length) return;
        button1Upgrades[buttonClicks[0]].enabled = true;
        buttonClicks[0]++;
        Debug.Log("ButtonOne Pressed");
    }

    public void ButtonTwo()
    {
        if (buttonClicks[1] >= button2Upgrades.Length) return;
        button2Upgrades[buttonClicks[1]].enabled = true;
        buttonClicks[1]++;
        Debug.Log("ButtonTwo Pressed");
    }

    public void ButtonThree()
    {
        if (buttonClicks[2] >= button3Upgrades.Length) return;
        Debug.Log("Button Clicks[2] value: " + buttonClicks[2]);
        button3Upgrades[buttonClicks[2]].enabled = true;
        buttonClicks[2]++;
        Debug.Log("ButtonThree Pressed");
        UpdateTowerRangeImage();
    }

    private void UpdateTowerRangeImage()
    {
        Vector3 scaleChange = new Vector3(circleCollider.radius * imageToCircleColliderRatio, circleCollider.radius * imageToCircleColliderRatio, 1f);
        towerRadius.transform.localScale = scaleChange;
    }
}
