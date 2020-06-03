using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] int themeTrackToPlay;
    void Start()
    {
        SoundManager.instance.PlayMusic(themeTrackToPlay);
    }
}
