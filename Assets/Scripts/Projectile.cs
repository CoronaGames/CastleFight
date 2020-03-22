using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using System;

namespace Game.Combat
{
    public class Projectile : MonoBehaviour
    {
        [Tooltip("Does the projectile have AOE damage?")]
        [SerializeField] bool hasAOE = false;
        [Tooltip("Does the projectile hit the ground or the target?")]
        [SerializeField] bool groundHit = false; // Hits the ground instead of targets - IE misses if targets move, may still do damage with AOE attack.
        [Tooltip("Should the projectile point towards target?")]
        [SerializeField] bool setRotation = true; 
        [SerializeField] float aoeDamage = 0f;
        [SerializeField] float projectileDamage = 10f;
        [SerializeField] float projectileSpeed = 5f;
        [SerializeField] string shootSound, targetHitsound;
        [SerializeField] float roatationOffsetDegrees = 90f;
        [SerializeField] float acceptableDistanceToDestination = .15f;

        [SerializeField] Health target;
        [SerializeField] Health shooter;

        bool isExploding = false;
        Transform spawnPosition;
        Vector3 targetPosition;
        Rigidbody2D myRigidbody;
        Animator myAnimator;
        CircleCollider2D myCircleCollider;
        public TeamData teamData;

        
        


        // Start is called before the first frame update
        void Start()
        {
            teamData = GetComponent<TeamData>();
            myRigidbody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            if(hasAOE)
            {
                myCircleCollider = GetComponent<CircleCollider2D>();
                myCircleCollider.enabled = false;
            }
            // Play shootSound            
        }

        // Update is called once per frame
        void Update()
        {
            if(myAnimator.GetBool("hitTarget") == true || isExploding == true) return;
            if (target != null)
            {
                transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
                if(!hasAOE)
                {
                    // No AOE = FollowTarget
                    targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                }
                if(hasAOE && IsCurrentTargetWithinRange())
                {
                    Explode();
                }
             
                
            }
            else
            {
                // transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, projectileSpeed * Time.deltaTime);
                myAnimator.SetBool("hitTarget", true);
            }            
        }

        public bool IsCurrentTargetWithinRange()
        {
            float currentTargetDistance = Mathf.Abs(target.transform.position.x - transform.position.x);
            if (currentTargetDistance <= acceptableDistanceToDestination && (Mathf.Abs(target.transform.position.y - transform.position.y) < .5f))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<TeamData>())
            {
                if (other.GetComponent<TeamData>().GetTeamBelonging() == teamData.GetTeamBelonging()) return;
            }
            if ((other != null) && (target != null) && (other.gameObject != gameObject))
            {
                if (!hasAOE && other.gameObject == target.gameObject)
                {
                    target.TakeDamage(projectileDamage);
                    if (target.GetComponent<Attacker>() && shooter != null)
                    {
                        target.GetComponent<Attacker>().CheckForAggro(shooter);
                    }
                    targetPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
                    myAnimator.SetBool("hitTarget", true);

                }
                else if (hasAOE && isExploding)
                {
                    if(other.tag == "NPC" || other.tag == "Building")
                    {
                        if (other.GetComponent<TeamData>().GetTeamBelonging() != teamData.GetTeamBelonging())
                        {
                            other.GetComponent<Health>().TakeDamage(aoeDamage);
                        }
                        if (target.GetComponent<Attacker>() && shooter != null)
                        {
                            target.GetComponent<Attacker>().CheckForAggro(shooter);
                        }
                    }
                
                }

                /*
                else
                {
                    Debug.Log("Triggered: else statement");
                    myAnimator.SetBool("hitTarget", true);
                }
                */
            }

           
        }

        public void Explode()
        {
            isExploding = true;
            //AudioManager.instance.Play("CanonKuleExplosion");
            myCircleCollider.enabled = true;
            myAnimator.SetBool("hitTarget", true);
        }

        private void DestroyObject()
        {
            Destroy(gameObject);
        }

        public void SetProjectileDamage(float projectileDamage, float aoeDamage)
        {
            this.projectileDamage = projectileDamage;
            this.aoeDamage = aoeDamage;
        }

        public void SetTarget(Health target)
        {
            this.target = target;
            if (setRotation && target != null) SetRotation();
        }

        public void SetRotation()
        {
            Vector3 ildkulePos = gameObject.transform.position;
            ildkulePos.z = 0f;

            Vector3 targetPos = target.transform.position;
            ildkulePos.x = ildkulePos.x - targetPos.x;
            ildkulePos.y = ildkulePos.y - targetPos.y;

            float angle = Mathf.Atan2(ildkulePos.y, ildkulePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - roatationOffsetDegrees));
        }

        public void SetTeamBelonging(Team team)
        {
            if (teamData == null) teamData = GetComponent<TeamData>();
            teamData.SetTeamBelonging(team);
        }

        public Health GetShooter()
        {
            return shooter;
        }

        public void SetShooter(Health shooter)
        {
            this.shooter = shooter;
        }
    }
}
