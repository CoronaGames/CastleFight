using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class MainBase : MonoBehaviour
{
    Health health;
    CastleFightData castleFightData;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        castleFightData = FindObjectOfType<CastleFightData>();
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
        CastleFightData.instance.BaseDestroyed(this);
    }

}
