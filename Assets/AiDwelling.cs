using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDwelling : MonoBehaviour
{
    [SerializeField] DwellingScript dwellingScript;
    [SerializeField] int dwellingIndexToUpgrade;
    [SerializeField] SpriteRenderer dwellingRenderer;
    [SerializeField] Sprite inactiveSprite;
    [SerializeField] Sprite activeSprite;

    [Header("Booleans:")]
    [SerializeField] bool waitingToUpgrade = false;
    [SerializeField] bool waitingToStartSpawning = false;
    [SerializeField] bool waitingToIncrementNumberOfSpawnedUnits = false;

    [Header("Timers:")]
    [SerializeField] float timeToUpgradeDwelling;
    [SerializeField] float currentTimeToUpgrade;

    [SerializeField] float timeToStartSpawning;
    [SerializeField] float currentTimeToStartSpawning;

    [SerializeField] float[] timeToIncrementNumberOfUnitsSpawned;
    [SerializeField] int currentIncrementIndex = 0;
    [SerializeField] float currentTimeToIncrementNumberOfUnitsSpawned;


    void Start()
    {
        dwellingScript = GetComponent<DwellingScript>();
        currentTimeToUpgrade = 0f;
        if (waitingToStartSpawning)
        {
            dwellingRenderer.sprite = activeSprite;
            dwellingScript.SetSpawningActivated(false);
        }
        if (waitingToUpgrade)
        {
            dwellingRenderer.sprite = inactiveSprite;
        }
    }

    void Update()
    {
        if (!CastleFightData.instance.gameInitiated) return;
        if (Timer())
        {
            return;
        }
        else
        {
            UpgradeDwelling();
        }
    }

    private bool Timer()    // Returns true if timer is active
    {
        if(waitingToUpgrade || waitingToStartSpawning || waitingToIncrementNumberOfSpawnedUnits)
        {
            if (waitingToStartSpawning)
            {
                if (waitingToStartSpawning && currentTimeToStartSpawning < timeToStartSpawning)
                {
                    currentTimeToStartSpawning += Time.deltaTime;
                }
                else
                {
                    StartSpawning();
                    
                }
                return false;
            }
           
            if (currentTimeToUpgrade < timeToUpgradeDwelling)
            {
                currentTimeToUpgrade += Time.deltaTime;
            }
            else
            {
                UpgradeDwelling();
            }

            if (currentTimeToIncrementNumberOfUnitsSpawned < timeToIncrementNumberOfUnitsSpawned[currentIncrementIndex])
            {
                currentTimeToIncrementNumberOfUnitsSpawned += Time.deltaTime;
            }
            else
            {
                IncrementUnitsSpawned();
            }

           
            return true;
        }
     
        return false;
    }

    public void UpgradeDwelling()
    {
 
    }

    public void StartSpawning()
    {
        dwellingScript.SetSpawningActivated(true);
        waitingToStartSpawning = false;
    }

    public void IncrementUnitsSpawned()
    {
        dwellingScript.SetNumberOfUnitsToSpawnPerCountdown(dwellingScript.GetNumberOfUnitsSpawnedPerCountdown() + 1);
        currentIncrementIndex++;
        if(currentIncrementIndex < timeToIncrementNumberOfUnitsSpawned.Length)
        {
            currentTimeToIncrementNumberOfUnitsSpawned = 0f;
        }
        else
        {
            waitingToIncrementNumberOfSpawnedUnits = false;
        }
        
    }

}
