using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Combat;

public class Attacker : MonoBehaviour
{

    public CircleCollider2D circleCollider;
    public CapsuleCollider2D capsuleCollider;
    public List<Health> targets;
    public Health target;
    public bool hasTarget = false;
    public int numberOfTargets = 0;
    public float actualTime;
    public bool collisionActive = false;
    [SerializeField] bool canTargetBuildings = true;
    public bool isCurrentlyAttacking = false;

    public Animator myAnimator;
    public TeamData teamBelonging;
    public Mover mover;

    // Start is called before the first frame update
    void Start()
    {
        
        teamBelonging = GetComponent<TeamData>();
        mover = GetComponent<Mover>();
        targets = new List<Health>();
        myAnimator = GetComponent<Animator>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    public bool HasTarget()
    {
        return hasTarget;
    }

    public bool IsCurrentTargetCloser(int otherIndex)
    {
        float currentTargetDeltaDistance;
        float otherTargetDeltaDistance;

        currentTargetDeltaDistance = Mathf.Abs(target.transform.position.x - transform.position.x) + Mathf.Abs(target.transform.position.y - transform.position.y);
        otherTargetDeltaDistance = Mathf.Abs(targets[otherIndex].transform.position.x - transform.position.x) + Mathf.Abs(targets[otherIndex].transform.position.y - transform.position.y);

        if (currentTargetDeltaDistance < otherTargetDeltaDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Timer()
    {
        if (actualTime >= 0)
        {
            actualTime -= Time.deltaTime;
        }
    }

    public virtual bool OverHeadUpdate()
    {
        return false;
    }

    public virtual bool Attack()
    {
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.GetComponent<Health>())
        {
            if ((other.GetComponent<TeamData>().GetTeamBelonging() != teamBelonging.GetTeamBelonging()) && other.GetComponent<Health>().GetHp() > 0f)
            {
                if (!CheckIfTargetIsInList(other.GetComponent<Health>()))
                {
                    AddTarget(other.GetComponent<Health>());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Health>())
        {
            if ((other.GetComponent<TeamData>().GetTeamBelonging() != teamBelonging.GetTeamBelonging()))
            {
                RemoveTarget(other.GetComponent<Health>());
            }
        }
    }

    public bool CheckIfTargetIsInList(Health target)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == target)
            {
                return true;
            }
        }
        return false;
    }

    public void AddTarget(Health target)
    {
        if (!canTargetBuildings)
        {
            if (!target.IsUnit()) return;
        }
        targets.Add(target);
        hasTarget = true;
        UpdateNumberOfTargets();
        if (numberOfTargets <= 0)
        {
            // TODO?
            return;
        }
        ChooseClosestTarget();
    }

    public void RemoveTarget(Health target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
            ChooseClosestTarget();
        }
        if (targets.Count == 0)
        {
            hasTarget = false;
        }
        UpdateNumberOfTargets();

    }

    public bool CheckForMissing()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
            else if (targets[i].GetHp() <= 0f)
            {
                targets.RemoveAt(i);
            }
        }
        if (targets.Count <= 0)
        {
            target = null;
            return true;
        }
        return false;

    }

    public void UpdateNumberOfTargets()
    {
        numberOfTargets = targets.Count;
    }

    public void ChooseClosestTarget()
    {
        CheckForMissing();
        for (int i = 0; i < targets.Count; i++)
        {
            if (target == null || i == 0)
            {
                target = targets[i];
            }
            else if (!IsCurrentTargetCloser(i))
            {
                target = targets[i];
            }
        }
        if (target != null) hasTarget = true;
        else hasTarget = false;
    }

    public void ChooseTarget(Health chosenTarget)
    {
        AddTarget(chosenTarget);
        target = chosenTarget;
    }

    public Health GetTarget()
    {
        return target;
    }

    public bool IsCurrentlyAttacking()
    {
        return isCurrentlyAttacking;
    }

    public void CheckForAggro(Health attacker)  // Triggered by projectile hit. Used by idle unit to retaliate from enemy attacks
    {
        if(target == null && attacker != null)
        {
            AddTarget(attacker);
        }
    }
}
