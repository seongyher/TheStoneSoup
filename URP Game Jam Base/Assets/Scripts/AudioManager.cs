using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    public float volumePercent;
}

public class AudioManager : Singleton<AudioManager>
{
    public List<Sound> sounds = new List<Sound>();
    public List<Sound> music = new List<Sound>();

    private List<AudioSource> activeSources = new List<AudioSource>();
    private AudioSource musicSource;
    private Sound currentMusic;

    public float volumeLevel = 1f;
    public bool masterMute = false;
    public bool sfxMute = false;
    public bool musicMute;

    private void Start()
    {
        ChangeMusic("Test");
    }

    public void PlaySound(string soundToPlay)
    {
        Sound sound = sounds.Find(s => s.name == soundToPlay);

        if (sound.clip == null)
        {
            Debug.Log("Sound not found: " + soundToPlay);
            return;
        }

        PlaySound(sound);
    }
    
    public void PlaySound(Sound soundToPlay)
    {
        if (masterMute || sfxMute)
            return;
        
        if (activeSources.Count >= 5)
            return;

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = soundToPlay.clip;
        source.volume = soundToPlay.volumePercent * volumeLevel;
        source.Play();

        activeSources.Add(source);
    }

    public void ChangeMusic(string musicToPlay)
    {
        Sound sound = music.Find(s => s.name == musicToPlay);

        if (sound.clip == null)
        {
            Debug.Log("Music not found: " + musicToPlay);
            return;
        }

        if(!musicSource)
            musicSource = gameObject.AddComponent<AudioSource>();
        
        currentMusic = sound;
        musicSource.clip = currentMusic.clip;
        musicSource.volume = currentMusic.volumePercent * volumeLevel;
        
        musicSource.loop = true;
        musicSource.Play();
    }

    private void Update()
    {
        // Clean up finished sounds
        for (int i = activeSources.Count - 1; i >= 0; i--)
        {
            if (!activeSources[i].isPlaying)
            {
                Destroy(activeSources[i]);
                activeSources.RemoveAt(i);
            }
        }
    }
}
