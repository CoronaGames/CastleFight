using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeResetButton : MonoBehaviour
{
    [SerializeField] Text[] textsToReset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnResetClick()
    {
        for(int i=0; i< textsToReset.Length; i++)
        {
            textsToReset[i].text = "0/10";
        }
    }
}
