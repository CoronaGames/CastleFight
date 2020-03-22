using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTrigger : MonoBehaviour
{
    BoxCollider2D boxCollider;
    TeamData teamData;
    [SerializeField] UnitSpawner unitSpawner;
    [SerializeField] Trap enemyTrap;
    [SerializeField] bool spawningTrigger;
    [SerializeField] bool trapTrigger;
    [SerializeField] bool triggerOnce = true;
    [SerializeField] float delayBetweenTriggers = 0f;  // If trigger once is false
    [SerializeField] float currentDelayTime = 0f;
    [SerializeField] bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        teamData = GetComponent<TeamData>();
    }

    private void Update()
    {
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
