using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem abilityFX;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] Ability ability;
    [SerializeField] Team team;
    [SerializeField] float timerMax;
    [SerializeField] float timerCurent;
    [SerializeField] float castingDelay = 0f;
    [SerializeField] float castingDelayCurrent = 0f;
    [SerializeField] bool isActive = false;
    

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        abilityFX = GetComponent<ParticleSystem>();
        team = GetComponentInParent<TeamData>().GetTeamBelonging();
        circleCollider.enabled = false;
    }

    public void ActivateAbility()
    {
        if (isActive) return;
        timerCurent = timerMax;
        castingDelayCurrent = castingDelay;
        isActive = true;
       
    }

    private void CastAbility()
    {
        circleCollider.enabled = true;
        circleCollider.radius = ability.GetRadius();
        if(abilityFX)abilityFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if(castingDelayCurrent > 0)
            {
                castingDelayCurrent -= Time.deltaTime;
                if (castingDelayCurrent <= 0) CastAbility();
                return;
            }
            timerCurent -= Time.deltaTime;
            if(timerCurent <= 0f)
            {
                isActive = false;
                circleCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TeamData>() && other.GetComponent<TeamData>().GetTeamBelonging() != team)
        {
            if (other.GetComponent<AbilitiesUsedOnTarget>())
            {
                other.GetComponent<AbilitiesUsedOnTarget>().AddAbilityUsedOnTarget(ability);
            }
        }
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        
    }

    public Ability GetAbility()
    {
        return ability;
    }
}
