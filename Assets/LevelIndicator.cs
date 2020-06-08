using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] int level = 0;
    [SerializeField] bool binaryUpgrade;
    [SerializeField] GameObject[] objectsToDeactivateOnBinary;

    public void LevelUp()
    {
        if (binaryUpgrade)
        {
            images[1].color = Color.yellow;
        }
        else if(images.Length >= level)
        {
            images[level].color = Color.yellow;
            level++;
        }
    }

    public void SetBinaryUpgrade()
    {
        binaryUpgrade = true;
       for(int i=0; i<objectsToDeactivateOnBinary.Length; i++)
        {
            objectsToDeactivateOnBinary[i].SetActive(false);
        }
    }
}
