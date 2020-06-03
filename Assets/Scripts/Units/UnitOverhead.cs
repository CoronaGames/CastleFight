using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Combat;
using Game.Movement;
using Game.Core;

public class UnitOverhead : MonoBehaviour
{
    Mover mover;
    NPCcontroller controller;
    Health health;
    [SerializeField] Attacker attackerScript;
    [SerializeField] AbilityCaster abilityCaster;
    
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        controller = GetComponent<NPCcontroller>();
        health = GetComponent<Health>();
        if(attackerScript == null)
        {
            attackerScript = GetComponent<Attacker>();
        }
        if(abilityCaster == null)
        {
            abilityCaster = GetComponent<AbilityCaster>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead()) return;  // - Not dead?
        // UseAbility()
        if (abilityCaster != null && abilityCaster.Attack()) return;
        else if (attackerScript.OverHeadUpdate()) return; 
        else if (mover.HasDestination()) mover.UpdateMovement();  
        else controller.NextWaypoint();  
    }
}
