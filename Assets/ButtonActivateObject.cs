using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivateObject : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;

    public void CheckForActivation()
    {
        if (objectToActivate.activeInHierarchy)
        {
            objectToActivate.SetActive(false);
        }
        else
        {
            objectToActivate.SetActive(true);
        }
    }
}
