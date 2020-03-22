using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Combat;

public class CfTower : MonoBehaviour
{
    public bool hasTarget = false;
    public GameObject projectilePrefab;
    public GameObject projectileInstance;
    [SerializeField] Ability currentAbility;
    [SerializeField] bool usesAbility = false; // if false, tower uses projectilePrefab;
    public List<Health> targetNPC;
    [SerializeField] GameObject preferTarget;

    public Transform goalPosition; // Use to calculate which NPC is closest to Exit
    [Tooltip("Attack interval in seconds")]

    private float attackCooldown = 0f;
    private float actualTime;
    private Rigidbody2D myRigidbody;
    [SerializeField] CircleCollider2D myCircleCollider;
    private BoxCollider2D myBoxCollider;
    public SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    public int numberOfTargets = 0;
    public Selectable selectable;  // Controls mouse click selection
    private TargetMouseSelected mouseController;
    private TeamData teamData;
    private Health health;

    [SerializeField] float projectileDamage = 50f;
    [SerializeField] float aoeDamage = 0f;
    [SerializeField] float attackSpeedInSeconds = 0.8f;
    [SerializeField] float towerAttackRange = 4f;
    [SerializeField] float criticalDamage = 1f; // How many times to multiply critical damage TODO

    private int projectileDamageUpgrades = 0, attackSpeedUpgrades = 0, attackRangeUpgrades = 0;

    bool triggerEntered = false; //For tower placement

    // Start is called before the first frame update
    void Start()
    {
        targetNPC = new List<Health>();
        myCircleCollider = GetComponentInChildren<CircleCollider2D>();
        myCircleCollider.radius = towerAttackRange;
        myBoxCollider = GetComponent<BoxCollider2D>();
        actualTime = 0;
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myAnimator.StopPlayback();
        myRigidbody = GetComponent<Rigidbody2D>();
        selectable = GetComponent<Selectable>();
        mouseController = FindObjectOfType<TargetMouseSelected>();
        teamData = GetComponent<TeamData>();
        health = GetComponent<Health>();



    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead())
        {
            Destroy(gameObject);
        }
        if (hasTarget)
        {
            Shooting();
        }
        Timer();

    }

    private void Timer()
    {
        if (actualTime >= 0)
        {
            actualTime -= Time.deltaTime;
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "NPC") && other.GetComponent<TeamData>())
        {
            if (other.GetComponent<TeamData>().GetTeamBelonging() != teamData.GetTeamBelonging() && other.GetComponent<Health>().GetHp() > 0f)
            {
                AddTarget(other.GetComponent<Health>());
            }
        }


    }

    private void AddTarget(Health target)
    {
        if (targetNPC.Contains(target))
        {
            return;
        }
        
        targetNPC.Add(target);
        hasTarget = true;
        UpdateNumberOfTargets();
    }

    private void UpdateNumberOfTargets()
    {
        numberOfTargets = targetNPC.Count;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC" && other.GetComponent<TeamData>())
        {
                RemoveTarget(other.GetComponent<Health>()); 
        }
    }

    private void RemoveTarget(Health target)
    {
        if (targetNPC.Contains(target))
        {
            targetNPC.Remove(target);
        }
        if (targetNPC.Count == 0)
        {
            hasTarget = false;
        }
        UpdateNumberOfTargets();
    }


    private void Shooting()
    {
        if (actualTime <= 0 && !CheckForMissing())
        {
            if (!usesAbility)
            {
                myAnimator.SetTrigger("Shoot");
                //projectile.GetComponent<Projectile>().SetRotation();
                
            }
            else if (usesAbility)
            {
                myAnimator.SetTrigger("CastSpell");
            }
            actualTime = attackSpeedInSeconds;
        }
        
    }

    private int CheckForPreferedTarget()
    {
        if (preferTarget == null) return 0;
        for (int i = 0; i < targetNPC.Count; i++)
        {
            if (targetNPC[i].transform.name == preferTarget.name)
            {
                return i;
            }
        }

        return 0;
    }

    private void Shoot()
    {
        
        if (targetNPC.Count > 0)
        {
            projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectileInstance.GetComponent<Projectile>().SetTeamBelonging(teamData.GetTeamBelonging());
            projectileInstance.GetComponent<Projectile>().SetTarget(targetNPC[0]);
            projectileInstance.GetComponent<Projectile>().SetProjectileDamage(projectileDamage, aoeDamage);
            myAnimator.ResetTrigger("Shoot");
        }
    }

    private void CastSpell()
    {

        if (targetNPC.Count > 0)
        {
            int targetPrefered = CheckForPreferedTarget();
            if (targetNPC[targetPrefered] != null)
            {
                if (targetNPC[targetPrefered].GetComponent<AbilitiesUsedOnTarget>())
                {
                    targetNPC[targetPrefered].GetComponent<AbilitiesUsedOnTarget>().AddAbilityUsedOnTarget(currentAbility);
                }

                myAnimator.ResetTrigger("CastSpell");
                //isCurrentlyAttacking = false;
            }

        }
    }

    private bool CheckForMissing()
    {
        for (int i = 0; i < targetNPC.Count; i++)
        {
            if (targetNPC[i] == null)
            {
                targetNPC.RemoveAt(i);
            }
            else if(targetNPC[i].GetHp() <= 0f)
            {
                targetNPC.RemoveAt(i);
            }
        }
        if (targetNPC.Count <= 0) return true;
        return false;

    }

    public void IncreaseProjectileDamage(float amountToIncrease)
    {
        if (projectileDamageUpgrades >= 3) return;
        projectileDamageUpgrades++;

        projectileDamage += amountToIncrease;
    }

    public void IncreaseTowerAttackSpeed(float amountToIncrease)
    {
        if (attackSpeedUpgrades >= 3) return;
        attackSpeedUpgrades++;
        attackSpeedInSeconds += amountToIncrease;
    }

    public void IncreaseTowerRange(float amountToIncrease)
    {
        if (attackRangeUpgrades >= 3) return;
        attackRangeUpgrades++;
        myCircleCollider.radius += amountToIncrease;
    }

}
