using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;
using Game.Core;

public class DwellingScript : MonoBehaviour
{
    [SerializeField] GameObject unitPrefabToInstantiate;
    [SerializeField] GameObject unitParent;
    [SerializeField] GameObject spawnPosition;  // Position to spawn units
    [SerializeField] Transform[] waypointsList;
    [SerializeField] Transform[] loopingWaypointsList;

    [SerializeField] float currentSpawnTimeInSeconds;
    [SerializeField] Transform spawnFill;
    [SerializeField] GameObject baseDwelling;
    [SerializeField] bool destroyOnZeroHealth = false;
    [SerializeField] int buyCost;
    int sellProfit; // Buycost divided by 2
    [SerializeField] Sprite dwellingSprite;

    TargetMouseSelected mouseController;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Selectable selectable;
    TeamData teamData;
    Health health;

    [Header("Upgrades/Variables")]
    [SerializeField] int numberOfUnitsToSpawnPerCountDown = 1;
    [SerializeField] float spawnTimeInSeconds;
    [SerializeField] bool spawningActivated = true;


    // Start is called before the first frame update
    void Start()
    {
        mouseController = FindObjectOfType<TargetMouseSelected>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        selectable = GetComponent<Selectable>();
        teamData = GetComponent<TeamData>();
        unitParent = transform.parent.parent.GetChild(2).gameObject;
        health = GetComponent<Health>();
        sellProfit = buyCost / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead())
        {
            DestroyDwelling();
        }
        if (CastleFightData.instance.gameInitiated && spawningActivated)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
       if(currentSpawnTimeInSeconds < spawnTimeInSeconds)
        {
            currentSpawnTimeInSeconds += Time.deltaTime;
        }
        else
        {
            InstantiateUnit();
            currentSpawnTimeInSeconds = 0f;
        }
        SetSpawnFill();
    }

    private void SetSpawnFill()
    {
        float fillScale = (currentSpawnTimeInSeconds / spawnTimeInSeconds);
        if (fillScale < 0)
        {
            fillScale = 0f;
        }
        spawnFill.localScale = new Vector3(fillScale, 1f, 1f);
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        waypointsList = waypoints;
    }

    public Transform[] GetWaypointsList()
    {
        return waypointsList;
    }

    public void SetLoopingWaypointsList(Transform[] waypoints)
    {
        loopingWaypointsList = waypoints;
    }

    public Transform[] GetLoopingWaypointsList()
    {
        return loopingWaypointsList;
    }

    private void InstantiateUnit()
    {
        if (unitPrefabToInstantiate == null) return;
        for(int i=0; i<numberOfUnitsToSpawnPerCountDown; i++)
        {
            GameObject instance = Instantiate(unitPrefabToInstantiate);
            if (unitParent != null)
            {
                instance.transform.SetParent(unitParent.transform);
            }
            else
            {
                instance.transform.SetParent(gameObject.transform);
            }

            instance.transform.position = spawnPosition.transform.position;

            if (instance.GetComponent<TeamData>())
            {
                instance.GetComponent<TeamData>().SetTeamBelonging(teamData.GetTeamBelonging());
            }

            if (instance.GetComponent<NPCcontroller>())
            {
                instance.GetComponent<NPCcontroller>().SetWaypoints(waypointsList);
                if(loopingWaypointsList != null) instance.GetComponent<NPCcontroller>().SetLoopingWaypoints(loopingWaypointsList);
            }

        }
    }

    private void GoBackToBaseDwelling()
    {
    
        GameObject instance = Instantiate(baseDwelling);
        instance.transform.position = transform.position;
        instance.transform.parent = transform.parent;
        instance.transform.GetChild(1).gameObject.SetActive(true);
        if (instance.GetComponentInChildren<DwellingUpgrader>())
        {
            instance.GetComponentInChildren<DwellingUpgrader>().SetWaypointsList(waypointsList);
        }
        instance.transform.GetChild(1).gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void DestroyDwelling()
    {
        // Play destroy animation and instantiate BaseDwelling
        if (destroyOnZeroHealth)
        {
            Destroy(gameObject);
        }
        else
        {
            GoBackToBaseDwelling();
        }
        
    }

    public void SellDwelling()
    {
        // Return money, play sell animation and instantiate basedwelling
        CastleFightData.instance.AddMoney(sellProfit);
        GoBackToBaseDwelling();
    }

    public int GetBuyCost()
    {
        return buyCost;
    }

    public void ReduceSpawnTime(float reduceSpawnTimeBy)
    {
        if(spawnTimeInSeconds > reduceSpawnTimeBy)
        {
            spawnTimeInSeconds -= reduceSpawnTimeBy;
        }
    }


    public void SetNumberOfUnitsToSpawnPerCountdown(int numberOfUnits)
    {
        numberOfUnitsToSpawnPerCountDown = numberOfUnits;
    }

    public int GetNumberOfUnitsSpawnedPerCountdown()
    {
        return numberOfUnitsToSpawnPerCountDown;
    }
    public Sprite GetDwellingSprite()
    {
        return dwellingSprite;
    }

    public void SetDestroyOnZeroHealth(bool value)
    {
        destroyOnZeroHealth = value;
    }

    public bool IsSpawningActivated()
    {
        return spawningActivated;
    }

    public void SetSpawningActivated(bool value)
    {
        spawningActivated = value;
    }

    public void SetUnitPrefabToInstantiate(GameObject prefab)
    {
        unitPrefabToInstantiate = prefab;
    }
}
