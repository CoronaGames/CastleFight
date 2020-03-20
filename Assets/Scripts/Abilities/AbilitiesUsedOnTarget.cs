using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class AbilitiesUsedOnTarget : MonoBehaviour
{
    Ability currentlyActiveDOT; // List of abilities used on target;
    Ability currentlyActiveSlow;
    Ability currentAoe;
    public Animator abilitiesAnimator;
    // Start is called before the first frame update
    Health health;
    float dotTimerInterval = 2f;
    float currentDotTime = 0;
    int numberOfDotsLeft = 0;

    float currentFreezeTime = 0f;
    float movementSpeedFactor = 1f;

    [SerializeField] SpriteRenderer[] spriteRenderer;

    void Start()
    {
        health = GetComponent<Health>();
        if (spriteRenderer == null)
        {
            spriteRenderer = new SpriteRenderer[1];
            spriteRenderer[0] = GetComponent<SpriteRenderer>();
        }
    }


    void Update()
    {
        DamageOverTime();
        Slow();
    }

    private void DamageOverTime()
    {
        if (currentlyActiveDOT == null) return;
        if (numberOfDotsLeft <= 0)
        {
            currentlyActiveDOT = null;
            currentDotTime = 0f;
            if (currentlyActiveSlow != null) return;
            SetColorOnParentRenderes(new Color(255f, 255f, 255f));
            return;
        }
        else if (currentDotTime <= 0)
        {
            health.TakeDamage(currentlyActiveDOT.GetDotDamage());
            currentDotTime = dotTimerInterval;
            numberOfDotsLeft--;

        }
        currentDotTime -= Time.deltaTime;
    }

    private void Slow()
    {
        if (currentlyActiveSlow == null) return;
        if (currentFreezeTime <= 0)
        {
            currentlyActiveSlow = null;
            ResetMoveSpeed();
            if (currentlyActiveDOT != null) return;
            SetColorOnParentRenderes(new Color(255f, 255f, 255f));
            return;
        }
        else if (currentFreezeTime > 0)
        {
            currentFreezeTime -= Time.deltaTime;
        }
    }

    private void NoFlag(Ability ability) // Regular Damage
    {
        abilitiesAnimator.SetTrigger(ability.GetAnimatorCallString());
        health.TakeDamage(ability.GetDamage());
    }

    private void CheckFlag(Ability ability)
    {
        if (ability.flagCurrent == Flag.None)
        {
            NoFlag(ability);
        }
        else if (ability.flagCurrent == Flag.DamageOverTime)
        {
            currentlyActiveDOT = ability;
            numberOfDotsLeft = ability.GetNumberOfTimesToDamage();
            SetColorOnParentRenderes(ability.GetColor());
        }
        else if (ability.flagCurrent == Flag.Slow)
        {
            currentFreezeTime = ability.GetTimeSlowed();
            currentlyActiveSlow = ability;
            SlowDownSpeed(ability);
            //mover.SetMoveSpeedFactor(ability.GetMovementReductionFactor());
            NoFlag(ability); // if slow has damage
            SetColorOnParentRenderes(ability.GetColor());
        }
        else if (ability.flagCurrent == Flag.Heal)
        {
            health.Heal(ability.GetHealAmount());
            abilitiesAnimator.SetTrigger("Heal");
        }

        else if (ability.flagCurrent == Flag.AOE)
        {

        }
    }

    public void AddAbilityUsedOnTarget(Ability ability)
    {
        CheckFlag(ability);
    }

    private void SetColorOnParentRenderes(Color color)
    {
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = color;
        }
    }

    private void SlowDownSpeed(Ability ability)
    {
        if (GetComponent<Mover>())
        {
            GetComponent<Mover>().SetMoveSpeedFactor(ability.GetMovementReductionFactor());
        }

    }

    private void ResetMoveSpeed()
    {
        if (GetComponent<Mover>())
        {
            GetComponent<Mover>().ResetMoveSpeed();
        }
    }
}
