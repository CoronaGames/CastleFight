using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTrigger : MonoBehaviour
{
    BoxCollider2D boxCollider;
    TeamData teamData;
    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] Trap enemyTrap;
    [Tooltip("Check if used to trigger unit spawners.")]
    [SerializeField] bool spawningTrigger;
    [Tooltip("Check if used to trigger traps.")]
    [SerializeField] bool trapTrigger;
    [SerializeField] bool triggerOnce = true;
    [SerializeField] float delayBetweenTriggers = 0f;  // If trigger once is false
    [SerializeField] float currentDelayTime = 0f;
    [SerializeField] bool triggered = false;
    [Tooltip("Time until trigger is active.")]
    [SerializeField] float waitToActivateTriggerInSeconds = 0f; // First trigger

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        teamData = GetComponent<TeamData>();
        if(waitToActivateTriggerInSeconds > 0f)
        {
            boxCollider.enabled = false;
        }
    }

    private void Update()
    {
        if(waitToActivateTriggerInSeconds > 0f && CastleFightData.instance.gameInitiated)
        {
            waitToActivateTriggerInSeconds -= Time.deltaTime;
            if(waitToActivateTriggerInSeconds <= 0f)
            {
                boxCollider.enabled = true;
            }
            return;
        }

        if (!triggerOnce && triggered)
        {
            if(currentDelayTime >= delayBetweenTriggers)
            {
                boxCollider.enabled = true;
                triggered = false;
                currentDelayTime = 0f;
            }
            else
            {
                currentDelayTime += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TeamData>())
        {
            if(other.GetComponent<TeamData>().GetTeamBelonging() != teamData.GetTeamBelonging())
            {
                triggered = true;
                if (spawningTrigger)
                {
                    // Activate trigger
                    unitSpawner.StartSpawning();
                    
                     boxCollider.enabled = false;
                    
                    
                    return;
                }

                if (trapTrigger)
                {
                    enemyTrap.TriggerTrap();
                    boxCollider.enabled = false;
                }
                
            }
        }
    }

}
