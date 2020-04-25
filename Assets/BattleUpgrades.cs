using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUpgrades : MonoBehaviour
{
    // Upgrades which is bought over and over for each game;

    public static BattleUpgrades instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
