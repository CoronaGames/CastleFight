using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }

    private GameObject oneShotGameObject;
    private AudioSource oneShotAudioSource;


    [SerializeField] AudioSource backgroundMusicSource;
    private int currentThemePlaying;
    [SerializeField] float[] songTimeStamp; // Used to save song time position when changing between scenes

    public void SetMusicVolume()
    {
        CheckForBackgroundMusicSource();
        backgroundMusicSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }

    public void SetSoundFxVolume()
    {
        CheckForOneShotAudioSource();
        oneShotAudioSource.volume = PlayerPrefs.GetFloat("soundFxVolume");
    }



    public void PlayMusic(int themeTrack)
    {
        CheckForBackgroundMusicSource();
        if (backgroundMusicSource.isPlaying)
        {
            Debug.Log("Music is playing");
            songTimeStamp[currentThemePlaying] = backgroundMusicSource.time;
        }
        AudioClip song = GameAssets.instance.themeSongs[themeTrack];
        currentThemePlaying = themeTrack;

        backgroundMusicSource.clip = song;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.time = songTimeStamp[currentThemePlaying];
        backgroundMusicSource.Play();
    }

    public void PlaySound(Sound sound)
    {
        CheckForOneShotAudioSource();
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    public void PlaySound(Sound sound, Vector3 position)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
        audioSource.Play();
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.instance.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private void CheckForBackgroundMusicSource()
    {
        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = GetComponent<AudioSource>();
            backgroundMusicSource.volume = PlayerPrefs.GetFloat("musicVolume");
            songTimeStamp = new float[GameAssets.instance.themeSongs.Length];
        }
    }

    private void CheckForOneShotAudioSource()
    {
        if (oneShotGameObject == null)
        {
            oneShotGameObject = new GameObject("One Shot Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            oneShotAudioSource.volume = PlayerPrefs.GetFloat("soundFxVolume");
        }
    }
}
