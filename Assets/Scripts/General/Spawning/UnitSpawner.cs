using Game.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] unitPrefab;
    [SerializeField] float[] spawningDelay;  // Length has to be same as unitPrefabs
    [SerializeField] int[] numberOfUnitsToSpawnPerTick;
    [SerializeField] bool spawningActivated = false;
    [SerializeField] bool isCurrentlySpawning = false;
    [SerializeField] int indexInCurrentWave = 0;
    [SerializeField] float timeToNextSpawn;
    [SerializeField] float spawnDelay = .1f;
    [SerializeField] float spawnDelayCurrent;
    [SerializeField] int unitsSpawnedThisIndex = 0;
    [SerializeField] TeamData teamData;
    private bool infoTextSet = false;

    [SerializeField] Transform[] waypointsList;

    void Start()
    {
        teamData = GetComponent<TeamData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawningActivated && !isCurrentlySpawning)
        {
            Timer();
        }
        else if(spawningActivated && isCurrentlySpawning)
        {
            if (!infoTextSet)
            {
                CastleFightGui.instance.SetInfoText("Enemy reinforcments incoming!");
                infoTextSet = true;
            }

            SpawnUnits();
        }
    }

    private bool Timer()    // Is Timer Active
    {
       if(timeToNextSpawn <= 0f && !isCurrentlySpawning)
        {
            isCurrentlySpawning = true;
            unitsSpawnedThisIndex = 0;
            return false;
        }
        timeToNextSpawn -= Time.deltaTime;
        return true;
    }


    private void NextSpawnIndex()
    {
        indexInCurrentWave++;
        if(indexInCurrentWave >= unitPrefab.Length)  // reached the end of unit array
        {
            StopSpawning();
            return;
        }
        isCurrentlySpawning = false;
        infoTextSet = false;
        if (indexInCurrentWave >= spawningDelay.Length) return;
        timeToNextSpawn = spawningDelay[indexInCurrentWave];

    }

    private void SpawnUnits()
    {
        if(unitsSpawnedThisIndex < numberOfUnitsToSpawnPerTick[indexInCurrentWave])
        {
            if(spawnDelayCurrent > spawnDelay)
            {
                // Spawn Unit

                InstantiateUnit(unitPrefab[indexInCurrentWave]);
                spawnDelayCurrent = 0f;
                unitsSpawnedThisIndex++;
            }
            else
            {
                spawnDelayCurrent += Time.deltaTime;
            }
        }
        else
        {
            NextSpawnIndex();
        }
    }

    private void InstantiateUnit(GameObject unitToSpawn)
    {
        GameObject instance = Instantiate(unitPrefab[indexInCurrentWave], transform);

   
          instance.transform.SetParent(gameObject.transform);

        if (instance.GetComponent<TeamData>())
        {
            instance.GetComponent<TeamData>().SetTeamBelonging(teamData.GetTeamBelonging());
        }

        if (instance.GetComponent<NPCcontroller>())
        {
            instance.GetComponent<NPCcontroller>().SetWaypoints(waypointsList);
        }
    }

    public void StartSpawning()
    {
        
        spawningActivated = true;
    }

    public void StopSpawning()
    {
        timeToNextSpawn = 0f;
        indexInCurrentWave = 0;
        isCurrentlySpawning = false;
        spawningActivated = false;
    }
}
