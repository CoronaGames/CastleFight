using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Movement;

public class SpellCaster : Attacker
{
    [SerializeField] Ability currentAbility;

    [SerializeField] float weaponRange = 2f;
    [SerializeField] float attackSpeedInSeconds = 0.8f;
    

    void Start()
    {
        teamBelonging = GetComponent<TeamData>();
        mover = GetComponent<Mover>();
        targets = new List<Health>();
        myAnimator = GetComponent<Animator>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        circleCollider.radius = weaponRange;
    }

    public void Cancel()
    {
        mover.isMoving = true;
    }

    public override bool OverHeadUpdate()
    {
        if (Attack()) return true;
        else return false;
    }


    public override bool Attack()
    {
        if (CheckForMissing()) return false;
        if (actualTime <= 0 && !CheckForMissing() && !GetComponent<Health>().IsDead())
        {
            isCurrentlyAttacking = true;
            myAnimator.SetTrigger("Shoot");
            actualTime = attackSpeedInSeconds;

        }
        return false;

        // TODO
    }



    private void Shoot()    // In this case, use spell
    {

        if (targets.Count > 0)
        {
            if (targets[0] != null && target)
            {
                if (targets[0].GetComponent<AbilitiesUsedOnTarget>())
                {
                    targets[0].GetComponent<AbilitiesUsedOnTarget>().AddAbilityUsedOnTarget(currentAbility);
                }

                myAnimator.ResetTrigger("Shoot");
                isCurrentlyAttacking = false;
            }

        }
    }
}
