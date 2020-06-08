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
    [SerializeField] DwellingUpgrader dwellingUpgrader;


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
    [SerializeField] UnitManagerPanel unitManager; // Defines max number of units available for spawning og currentUnitType;
    [SerializeField] int unitIndexForUnitManager;
    [SerializeField] DwellingUpgrade[] activeUnitUpgrades;

    [Header("Special Attributes")]
    [SerializeField] bool limitAmountOfUnits;
    [SerializeField] int maxUnits;
    [SerializeField] int currentNoOfUnits;
    [SerializeField] Health[] currentUnits;



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
        currentUnits = new Health[3];
        if(teamData.GetTeamBelonging() == Team.TeamRed)
        {
            spawnTimeInSeconds -= GlobalUpgrades.instance.GetUpgradeValueOnUpgradesIndex(1);    // Index 1 is spawnTimeUpgrade
            unitManager = FindObjectOfType<UnitManagerPanel>();
        }
        if(dwellingUpgrader == null)
        {
            dwellingUpgrader = GetComponentInChildren<DwellingUpgrader>();
        }
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
        SetSpawnFill();
        if (currentSpawnTimeInSeconds < spawnTimeInSeconds)
        {
            currentSpawnTimeInSeconds += Time.deltaTime;
        }
        else
        {
            if (limitAmountOfUnits && CheckForMaxUnits()) return;
            if (teamData.GetTeamBelonging() == Team.TeamRed )
            {
                if (CastleFightData.instance.IsMaxUnitsReached() || CheckIfUnitCapacityReached() || CastleFightData.instance.IsPauseSpawningUnits()) return;
                InstantiateUnit();
                currentSpawnTimeInSeconds = 0f;
            }
            else
            {
                InstantiateUnit();
                currentSpawnTimeInSeconds = 0f;
            }
        }
       
    }

    public bool CheckForMaxUnits() // Returns true if max units reached;
    {
        int units = 0;
        for(int i=0; i<currentUnits.Length; i++)
        {
            if (currentUnits[i] != null) units++;
        }
        currentNoOfUnits = units;
        if (currentNoOfUnits >= maxUnits) return true;
        else return false;
    }

    private bool CheckIfUnitCapacityReached()
    {
        int currentUnits = CastleFightData.instance.GetAmountOfUnit(unitIndexForUnitManager);
        int maxUnits = unitManager.GetMaxUnitsOnIndex(unitIndexForUnitManager);
        if (maxUnits < 0) return false;
        if (currentUnits >= maxUnits || maxUnits == 0) return true;
        return false;
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
                if (teamData.GetTeamBelonging() == Team.TeamRed)
                {
                    CastleFightData.instance.AddPlayerUnitCount();
                    AddUpgradesToUnit(instance);
                }

            }

            if (instance.GetComponent<NPCcontroller>())
            {
                instance.GetComponent<NPCcontroller>().SetWaypoints(waypointsList);
                if(loopingWaypointsList != null) instance.GetComponent<NPCcontroller>().SetLoopingWaypoints(loopingWaypointsList);
            }

        }
    }

    private void AddUpgradesToUnit(GameObject instance)
    {
        for(int i=0; i<activeUnitUpgrades.Length; i++)
        {
            if (activeUnitUpgrades[i] == null) continue;
            if (activeUnitUpgrades[i].GetUpgradeType() == UpgradeType.Damage)
            {
                instance.GetComponent<Attacker>().AddToBaseDamage(activeUnitUpgrades[i].GetUpgradeVariableCurrentLevel());
            }
            else if (activeUnitUpgrades[i].GetUpgradeType() == UpgradeType.ActivateAbility)
            {
                instance.GetComponent<AbilityCaster>().ActivateAbility();
            }
            else if (activeUnitUpgrades[i].GetUpgradeType() == UpgradeType.AbilityDamage)
            {
                
            }
        }
        
    }

    private void GoBackToBaseDwelling()
    {
    
        GameObject instance = Instantiate(baseDwelling);
        instance.transform.position = transform.position;
        instance.transform.parent = transform.parent;
        instance.transform.GetChild(1).gameObject.SetActive(true);
        if (instance.GetComponentInChildren<BasicDwelling>())
        {
            instance.GetComponentInChildren<BasicDwelling>().SetWaypointsList(waypointsList);
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

    public void AddUpgrade(DwellingUpgrade upgrade)
    {
         if(upgrade.GetUpgradeType() == UpgradeType.Damage)
         {
            int index;
            if (CheckIfUpgradeExists(upgrade, UpgradeType.Damage, out index))
            {
                activeUnitUpgrades[index].IncrementLevel();
                return;
            } 
            
         }
         else if(upgrade.GetUpgradeType() == UpgradeType.SpawnExtraUnits)
        {
            SetNumberOfUnitsToSpawnPerCountdown(GetNumberOfUnitsSpawnedPerCountdown() + Mathf.RoundToInt(upgrade.GetUpgradeVariables()[upgrade.GetCurrentLevel()]));
        }
    }

    private void AddUpgradeToList(DwellingUpgrade upgrade)
    {
        for(int i=0; i<activeUnitUpgrades.Length; i++)
        {
            if(activeUnitUpgrades[i] == null)
            {
                activeUnitUpgrades[i] = upgrade;
            }
        }
    }

    private bool CheckIfUpgradeExists(DwellingUpgrade upgrade, UpgradeType upgradeType, out int index) // Returns true if upgrade exists
    {
        for(int i=0; i < activeUnitUpgrades.Length; i++)
        {
            if (activeUnitUpgrades[i] != null && activeUnitUpgrades[i].GetUpgradeType() == upgradeType)
            {
                index = i;
                return true;
            }
        }
        index = 0;
        return false;
    }
}
