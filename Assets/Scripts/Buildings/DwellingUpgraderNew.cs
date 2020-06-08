using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DwellingUpgraderNew : MonoBehaviour
{
    [SerializeField] GameObject parentObject;
    [SerializeField] GameObject[] dwellingPrefabs;
    [SerializeField] Transform[] waypointsList;
    [SerializeField] Transform[] loopingWaypointsList;
    [SerializeField] Text[] priceTags;

    public Image[] button1Upgrades;
    public Image[] button2Upgrades;
    public Image[] button3Upgrades;

    [SerializeField] GameObject demoSprite;
    [SerializeField] SpriteRenderer dwellingSprite;
    [SerializeField] bool destroyOnZeroHealth = false;


    [SerializeField] Image[][] imagesList;

    [SerializeField] int[] buttonClicks;
    Canvas canvas;
    TeamData teamData;

    [SerializeField] bool isUpgrade; // is the buttons on this dwelling an upgrade or a new dwelling?
    [SerializeField] DwellingScript dwellingScript;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        teamData = GetComponentInParent<TeamData>();
        if (GetComponentInParent<DwellingScript>())
        {
            dwellingScript = GetComponentInParent<DwellingScript>();
            loopingWaypointsList = dwellingScript.GetLoopingWaypointsList();
            waypointsList = dwellingScript.GetWaypointsList();

        }
        SetUpPriceTags();
        parentObject = transform.parent.gameObject;
    }

    private void SetUpPriceTags()
    {
        for (int i = 0; i < dwellingPrefabs.Length; i++)
        {
            if (dwellingPrefabs[i] != null)
            {
                priceTags[i].text = dwellingPrefabs[i].GetComponent<DwellingScript>().GetBuyCost().ToString();
            }
        }
    }

    public void PressButton(int buttonIndex)
    {
        if (dwellingPrefabs[buttonIndex] == null) return;
        if (!isUpgrade)
        {
            SetDwellingData(dwellingPrefabs[buttonIndex]);
            return;
        }
        // TODO: Fix this: 
        if (buttonClicks[buttonIndex] >= button3Upgrades.Length) return;
        Debug.Log("Button Clicks[2] value: " + buttonClicks[buttonIndex]);
        button3Upgrades[buttonClicks[buttonIndex]].enabled = true;
        buttonClicks[buttonIndex]++;
        Debug.Log("ButtonThree Pressed");
    }

    private void SetDwellingData(GameObject dwelling)
    {

        if (CastleFightData.instance.ReduceMoney(dwelling.GetComponent<DwellingScript>().GetBuyCost()))
        {
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            GameObject instance = Instantiate(dwelling, gameObject.transform);

            if (instance.GetComponent<TeamData>())
            {
                instance.GetComponent<TeamData>().SetTeamBelonging(teamData.GetTeamBelonging());
            }
            if (instance.GetComponent<DwellingScript>())
            {
                instance.GetComponent<DwellingScript>().SetDestroyOnZeroHealth(destroyOnZeroHealth);
                if (waypointsList.Length > 0)
                {
                    instance.GetComponent<DwellingScript>().SetWaypoints(waypointsList);
                }
                else
                {
                    instance.GetComponent<DwellingScript>().SetWaypoints(GetComponentInParent<DwellingScript>().GetWaypointsList());
                }
                if (loopingWaypointsList.Length > 0)
                {
                    if (loopingWaypointsList.Length > 0)
                    {
                        instance.GetComponent<DwellingScript>().SetLoopingWaypointsList(loopingWaypointsList);
                    }
                    else
                    {
                        instance.GetComponent<DwellingScript>().SetLoopingWaypointsList(GetComponentInParent<DwellingScript>().GetLoopingWaypointsList());
                    }
                }
            }
            //else if()

            instance.transform.SetParent(gameObject.transform.parent.parent);
            instance.transform.position = transform.parent.position;
            Destroy(parentObject);
        }
        else
        {
            CastleFightGui.instance.SetInfoText("Not enough money to buy dwelling!");
            // Connect to UI element for info
        }
    }



    public void SetWaypointsList(Transform[] waypoints)
    {
        waypointsList = waypoints;
    }

    public void ActivateShowDemo(int index)
    {
        demoSprite.SetActive(true);
        dwellingSprite.enabled = false;
        if (dwellingPrefabs[index] != null)
        {
            if (dwellingPrefabs[index].GetComponent<DwellingScript>() && demoSprite.GetComponent<SpriteRenderer>())
            {
                demoSprite.GetComponent<SpriteRenderer>().sprite = dwellingPrefabs[index].GetComponent<DwellingScript>().GetDwellingSprite();
            }
        }
        else
        {
            DeactivateShowDemo();
        }
    }

    public void DeactivateShowDemo()
    {
        demoSprite.SetActive(false);
        dwellingSprite.enabled = true;
    }

    public Sprite GetDwellingSprite()
    {
        return dwellingSprite.sprite;
    }
}
