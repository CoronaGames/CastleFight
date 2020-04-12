using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;



public class MeleeAttacker : Attacker // Extends attacker
{
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float maxTargetDistanceToAttack = .4f;
    [SerializeField] float currentTargetDistance;

    // Start is called before the first frame update
    void Start()
    {
        teamBelonging = GetComponent<TeamData>();
        mover = GetComponent<Mover>();
        targets = new List<Health>();
        myAnimator = GetComponent<Animator>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        if (teamBelonging.GetTeamBelonging() == Team.TeamRed)
        {
            AddToBaseDamage(GlobalUpgrades.instance.GetUpgradeValueOnUpgradesIndex(0));    // Index 0 is attackDamageUpgrade
            IncreaseCriticalChance(GlobalUpgrades.instance.GetUpgradeValueOnUpgradesIndex(2)); // Index 2 is CriticalChance
            IncreaseAttackSpeed(GlobalUpgrades.instance.GetUpgradeValueOnUpgradesIndex(3));
            IncreaseCriticalDamageMultiplier(GlobalUpgrades.instance.GetUpgradeValueOnUpgradesIndex(4));
        }
    }

    void Update()
    {
        if (GetComponent<Health>().GetHp() <= 0f)
        {
            return;
        }
        
        Timer();
    }

    public bool IsCurrentTargetWithinRange()    // Only useful for melee attackers
    {
        if (collisionActive) return true;
        currentTargetDistance = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (currentTargetDistance <= maxTargetDistanceToAttack && (Mathf.Abs(target.transform.position.y - transform.position.y) < .5f))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if(other.gameObject.tag == "Building")
        {
            if (other.gameObject.GetComponent<TeamData>().GetTeamBelonging() == teamBelonging.GetTeamBelonging()) return;   // Do not attack team
            ChooseTarget(other.gameObject.GetComponent<Health>());
            collisionActive = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
  
        if (other.gameObject.tag == "Building")
        {
            RemoveTarget(other.gameObject.GetComponent<Health>());
            collisionActive = false;
        }
    }

    public override bool OverHeadUpdate()
    {
        return Attack();
    }

    public override bool Attack()
    {
        if (CheckForMissing()) return false;
        
        if (target != null && !IsCurrentTargetWithinRange() && !GetComponent<Health>().IsDead() && !collisionActive)
        {
            mover.MoveTo(target.gameObject);
            return false;
        }
        else if (actualTime <= 0 && !CheckForMissing())
        {
            isCurrentlyAttacking = true;
            myAnimator.SetTrigger("Attack");
            actualTime = attackSpeedInSeconds;
        }
        else if (collisionActive)
        {
            myAnimator.SetBool("Idle", true);
            return true;
        }
        if (target != null) return true;
        return false;
    }

    public void MeleeAttack()
    {
        isCurrentlyAttacking = false;
        if (target == null) return;
        // Check for critical damage;
        if (CheckForCriticalDamage())
        {
            target.TakeDamage(weaponDamage * criticalDamageMultiplier);
        }
        else
        {
            target.TakeDamage(weaponDamage);
        }
    }


}

