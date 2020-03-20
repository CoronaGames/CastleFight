using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using System;
using Game.Combat;

namespace Game.Movement
{
    public class NPCcontroller : MonoBehaviour
    {
        public Transform[] wayPoints;
        public Transform[] loopingWaypoints;
        public Transform currentWaypoint;
        public bool followingPlayer = false;
        public float totalFollowTimeInSeconds = 5f;
        public float currentFollowTime = 0f;
        [SerializeField] int currentWaypointIndex = -1;
        Health health;
        Mover mover;
        Attacker attacker;
        [SerializeField] TeamData teamBelonging;
        private bool switchToLooping = false;

        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Attacker>())
            {
                attacker = GetComponent<Attacker>();
            }
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            teamBelonging = GetComponent<TeamData>();
        }

        public void BackToFollowingWaypoint()
        {
            if (currentWaypointIndex >= wayPoints.Length)
            {
                return;
            }

            mover.MoveTo(wayPoints[currentWaypointIndex].gameObject);
        }

        public void NextWaypoint()
        {
            if (switchToLooping)
            {
                if (loopingWaypoints.Length <= 0) return;
                if (currentWaypointIndex < loopingWaypoints.Length-1)
                {
                    currentWaypointIndex++;
                }
                else
                {
                    currentWaypointIndex = 0;
                }

                mover.MoveTo(loopingWaypoints[currentWaypointIndex].gameObject);
                currentWaypoint = loopingWaypoints[currentWaypointIndex];
            }
            else
            {
                //TODO
                if (currentWaypointIndex < wayPoints.Length)
                {
                    currentWaypointIndex++;

                }
                if (currentWaypointIndex >= wayPoints.Length)
                {
                    mover.MoveTo(wayPoints[wayPoints.Length - 1].gameObject);
                    switchToLooping = true;
                    currentWaypointIndex = 0;
                    return;
                }

                mover.MoveTo(wayPoints[currentWaypointIndex].gameObject);
                currentWaypoint = wayPoints[currentWaypointIndex];
            }
           



        }

        public void SetWaypoints(Transform[] waypoints)
        {
            wayPoints = waypoints;
            currentWaypoint = waypoints[0];
        }


        public void StopFollowingTarget()
        {
            followingPlayer = false;
            mover.isMoving = false;
        }

   

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Destination")
            {
                mover.MoveTo(null);
            }
          
        }

        public void SetLoopingWaypoints(Transform[] waypoints)
        {
            loopingWaypoints = waypoints;
        }
    }

}