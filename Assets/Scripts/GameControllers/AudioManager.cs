using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource stepsSource;

    public List<AudioClip> sfxClips;
    public AudioClip stepsClip;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        stepsSource.clip = stepsClip;
    }

    
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Count)
        {
            sfxSource.clip = sfxClips[index];
            sfxSource.Play();
        }
        else
        {
            Debug.LogWarning("Индекс звукового эффекта вне диапазона!");
        }
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySteps()
    {
        if (!stepsSource.isPlaying)
        {
            stepsSource.loop = true;
            stepsSource.Play();
        }
    }

    public void StopSteps()
    {
        if (stepsSource.isPlaying)
        {
            stepsSource.loop = false;
            stepsSource.Stop();
        }
    }

    public void PauseAll()
    {
        StopSteps();
        StopMusic();
    }

    
}
