using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    CircleCollider2D myCircleCollider;
    Health health;
    Mover mover;
    TeamData team;

    // Start is called before the first frame update
    void Start()
    {
        myCircleCollider = GetComponent<CircleCollider2D>();
        health = GetComponentInParent<Health>();
        mover = GetComponentInParent<Mover>();
        team = GetComponentInParent<TeamData>();
        myCircleCollider.enabled = true;



    }

    private void Update()
    {
        transform.position = transform.parent.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
          if (other.GetComponent<Health>())
        {
            if (other.GetComponent<TeamData>().GetTeamBelonging() != team.GetTeamBelonging())
            {
                mover.MoveTo(other.gameObject);
            }
        }
    }
}
