using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] Slider soundFxSlider;
    [SerializeField] Slider musicSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundFxSlider.value = PlayerPrefs.GetFloat("soundFxVolume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSoundFxVolume()
    {
        float value = soundFxSlider.value;
        Mathf.Clamp(value, 0f, 1f);
        PlayerPrefs.SetFloat("soundFxVolume", value);
        SoundManager.instance.SetSoundFxVolume();
    }

    public void SetMusicVolume()
    {
        float value = musicSlider.value;
        Mathf.Clamp(value, 0f, 1f);
        PlayerPrefs.SetFloat("musicVolume", value);
        SoundManager.instance.SetMusicVolume();
    }
}
