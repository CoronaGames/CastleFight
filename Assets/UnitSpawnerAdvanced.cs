using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;

public class UnitSpawnerAdvanced : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] GameObject[] currentWave;
    [SerializeField] float[] spawningDelay;
    [SerializeField] int[] numberOfUnitsToSpawnPerTick;
    [SerializeField] float spawnDelay = .1f;
    [SerializeField] float spawnDelayCurrent;
    [SerializeField] bool spawningActivated = false;
    [SerializeField] bool isCurrentlySpawning = false;
    [SerializeField] float timeToNextSpawn;
    [SerializeField] int unitsSpawnedThisIndex;
    [SerializeField] int indexInCurrentWave = 0;
    [SerializeField] int waveIndex = 0;
    [SerializeField] bool spawnerFinished = false;

    [SerializeField] float[] waitForNextWaveDelay;
    [SerializeField] float currentWaitForNextWave = 0f;
    [SerializeField] bool nextWaveDelayActive = false;

    [SerializeField] TeamData teamData;
    private bool infoTextSet = false;

    [SerializeField] Transform[] waypointsList;
    [SerializeField] Transform[] loopingWaypoints;
    [SerializeField] bool addedSpawnerToData = false;

    // Start is called before the first frame update
    void Start()
    {
        teamData = GetComponent<TeamData>();
        
    }

    void Update()
    {
        
        if (!CastleFightData.instance.gameInitiated || spawnerFinished)
        {
            return;
        }
        else if(CastleFightData.instance.gameInitiated && !spawningActivated && !addedSpawnerToData)
        {
            CastleFightData.instance.AddSpawner();
            StartSpawning();
            addedSpawnerToData = true;
        }
        if (nextWaveDelayActive)
        {
            WaitBetweenWavesTimer();
            return;
        }
        if (spawningActivated && !isCurrentlySpawning)
        {
            Timer();
        }
        else if (spawningActivated && isCurrentlySpawning)
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
        if (timeToNextSpawn <= 0f && !isCurrentlySpawning)
        {
            isCurrentlySpawning = true;
            unitsSpawnedThisIndex = 0;
            return false;
        }
        timeToNextSpawn -= Time.deltaTime;
        return true;
    }

    private void WaitBetweenWavesTimer()
    {
        if (waveIndex >= waitForNextWaveDelay.Length) return;
        if(currentWaitForNextWave < waitForNextWaveDelay[waveIndex])
        {
            currentWaitForNextWave += Time.deltaTime;
        }
        else
        {
            currentWaitForNextWave = 0f;
            nextWaveDelayActive = false;
        }
    }

    private void NextWave()
    {

        waveIndex++;
        if (waveIndex >= waves.Length)
        {
            StopSpawning();
            return;
        }
        currentWave = waves[waveIndex].GetUnitList();
        spawningDelay = waves[waveIndex].GetSpawningDelay();
        numberOfUnitsToSpawnPerTick = waves[waveIndex].GetUnitsToSpawn();
        nextWaveDelayActive = true;
    }

    private void NextSpawnIndex()
    {
        indexInCurrentWave++;
        if (indexInCurrentWave >= currentWave.Length)  // reached the end of unit array
        {
            //spawningActivated = false;
            NextWave();
            return;
        }
        isCurrentlySpawning = false;
        infoTextSet = false;
        timeToNextSpawn = spawningDelay[indexInCurrentWave];

    }

    private void SpawnUnits()
    {
        if (indexInCurrentWave >= currentWave.Length)
        {
            StopSpawning();
            return;
        }
        if (unitsSpawnedThisIndex < numberOfUnitsToSpawnPerTick[indexInCurrentWave])
        {
            if (spawnDelayCurrent > spawnDelay)
            {
                // Spawn Unit

                InstantiateUnit(currentWave[indexInCurrentWave]);
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
        GameObject instance = Instantiate(currentWave[indexInCurrentWave], transform);


        instance.transform.SetParent(gameObject.transform);

        if (instance.GetComponent<TeamData>())
        {
            instance.GetComponent<TeamData>().SetTeamBelonging(teamData.GetTeamBelonging());
        }

        if (instance.GetComponent<NPCcontroller>())
        {
            instance.GetComponent<NPCcontroller>().SetWaypoints(waypointsList);
            if(loopingWaypoints != null) instance.GetComponent<NPCcontroller>().SetLoopingWaypoints(loopingWaypoints);
        }
    }

    public void StartSpawning()
    {
        spawningActivated = true;
        waveIndex = 0;
        SetFirstWave();
    }

    public void StopSpawning()
    {
        spawnerFinished = true;
        spawningActivated = false;
        CastleFightData.instance.SpawnerFinished();
    }

    private void SetFirstWave()
    {
        currentWave = waves[waveIndex].GetUnitList();
        spawningDelay = waves[waveIndex].GetSpawningDelay();
        numberOfUnitsToSpawnPerTick = waves[waveIndex].GetUnitsToSpawn();
        nextWaveDelayActive = true;
    }

}
