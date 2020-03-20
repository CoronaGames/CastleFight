using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Movement;

namespace Game.Combat
{
    public class Shooter : Attacker // Extends attacker
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float projectileDamage = 10f;
        [SerializeField] float aoeDamage = 5f;
        [SerializeField] float attackSpeedInSeconds = 0.8f;

        public GameObject projectilePrefab;
        public GameObject projectileInstance;

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
            if (target != null) return true;

            return false;

            // TODO
        }



        private void Shoot()
        {
            
            if (targets.Count > 0)
            {
                if(targets[0] != null && target)
                {
                    projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
                    projectileInstance.GetComponent<Projectile>().SetTeamBelonging(GetComponent<TeamData>().GetTeamBelonging());
                    projectileInstance.GetComponent<Projectile>().SetTarget(target);  // TODO make ProjectileArrow more generic IE only Projectile
                    projectileInstance.GetComponent<Projectile>().SetShooter(GetComponent<Health>());
                    projectileInstance.GetComponent<Projectile>().SetProjectileDamage(projectileDamage, aoeDamage);

                    isCurrentlyAttacking = false;
                    myAnimator.ResetTrigger("Shoot");
                }
                
            }
        }


     }

    
}


