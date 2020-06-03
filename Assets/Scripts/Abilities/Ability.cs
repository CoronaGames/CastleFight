using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ability : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] string abilityName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] GameObject abilityProjectilePrefab; // Sprites, animation etc...
    [SerializeField] GameObject abilityImpactPrefab; // Sprites, animation etc...
    [SerializeField] float manaCost;
    [SerializeField] Color color;
    [SerializeField] string animatorCallString;
    [SerializeField] bool onlyTargetUnits = false;
    [SerializeField] bool targetFriendlyUnits = false;
    [SerializeField] bool isAoeAbility = false;


    [SerializeField] float coolDown;
    private float currentCooldown = 0f;
    [SerializeField] bool isOnCooldown = false;

    public Flag flagCurrent;
    [SerializeField] CircleCollider2D circleCollider;

    [Header("Dot: ")]
    [SerializeField] float dotDamage;
    [SerializeField] int numberOfTimesToDamage;

    [Header("Slow: ")]
    [SerializeField] float timeSlowed;
    [Tooltip("Between 0 and 1")]
    [SerializeField] float movementReductionFactor; // between 0 and 1

    [Header("Heal: ")]
    [SerializeField] float healAmount;

    [Header("AOE: ")]
    [SerializeField] float aoeRadius;


    // Start is called before the first frame update
    void Start()
    {
        isOnCooldown = false;
    }

    public float GetDamage()
    {
        return damage;
    }

    public string GetName()
    {
        return abilityName;
    }

    public string GetDescription()
    {
        return description;
    }


    public float GetDotDamage()
    {
        return dotDamage;
    }

    public float GetInitialDamage()
    {
        return damage;
    }

    public int GetNumberOfTimesToDamage()
    {
        return numberOfTimesToDamage;
    }

    public Color GetColor()
    {
        return color;
    }

    public float GetTimeSlowed()
    {
        return timeSlowed;
    }

    public float GetMovementReductionFactor()
    {
        return movementReductionFactor;
    }

    public Sprite GetSprite()
    {
        if (icon != null)
        {
            return icon;
        }
        else
        {
            return null;
        }

    }

    public float GetRadius()
    {
        return aoeRadius;
    }

    public float GetHealAmount()
    {
        return healAmount;
    }

    public GameObject GetAbilityImpactPrefab()
    {
        return abilityImpactPrefab;

    }

    public float GetCooldown()
    {
        return coolDown;
    }

    public float GetCurrentCoolDown()
    {
        return currentCooldown;
    }

    public void SetCurrentCoolDown(float currentCooldown)
    {
        this.currentCooldown = currentCooldown;
    }

    public bool IsOnCoolDown()
    {
        return isOnCooldown;
    }

    public void SetOnCoolDown(bool value)
    {
        isOnCooldown = value;
    }

    public string GetAnimatorCallString()
    {
        return animatorCallString;
    }

    public bool OnlyTargetUnits()
    {
        return onlyTargetUnits;
    }

    public bool IsTargetFriendlyUnits()
    {
        return targetFriendlyUnits;
    }

    public Flag GetAbilityFlag()
    {
        return flagCurrent;
    }

    public bool IsAoe()
    {
        return isAoeAbility;
    }
}
