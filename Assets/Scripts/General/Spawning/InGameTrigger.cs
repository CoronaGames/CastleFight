using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTrigger : MonoBehaviour
{
    BoxCollider2D boxCollider;
    TeamData teamData;
    [SerializeField] UnitSpawner unitSpawner;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        teamData = GetComponent<TeamData>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TeamData>())
        {
            if(other.GetComponent<TeamData>().GetTeamBelonging() != teamData.GetTeamBelonging())
            {
                // Activate trigger
                unitSpawner.StartSpawning();
                boxCollider.enabled = false;
            }
        }
    }

}
