using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] GameObject[] unitList;
    [SerializeField] int[] unitsToSpawn;
    [SerializeField] float[] spawningDelay;


    public GameObject[] GetUnitList()
    {
        return unitList;
    }

    public int[] GetUnitsToSpawn()
    {
        return unitsToSpawn;
    }

    public float[] GetSpawningDelay()
    {
        return spawningDelay;
    }

    
}
