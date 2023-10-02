using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    ///Now that its a singleton, you can access things using:
    ///AudioManager.instance.PlayOneShot("Music");
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep AudioManager alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Background music start
        //Make loop
        Play("BackgroundMusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.varyPitch)
        {
            s.source.pitch = UnityEngine.Random.Range(s.minRange, s.maxRange);
        }
        else
        {
            s.source.pitch = 1;
        }
        s.source.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.varyPitch)
        {
            s.source.pitch = UnityEngine.Random.Range(s.minRange, s.maxRange);
        }
        else
        {
            s.source.pitch = 1;
        }
        s.source.PlayOneShot(s.source.clip);
    }

    public void Play(string name, float stopTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.varyPitch)
        {
            s.source.pitch = UnityEngine.Random.Range(s.minRange, s.maxRange);
        }
        else
        {
            s.source.pitch = 1;
        }
        s.source.Play();
        StartCoroutine(StopAudioAfterTime(s.source, stopTime));
    }

    private IEnumerator StopAudioAfterTime(AudioSource audioSource, float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.Stop();
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    public bool varyPitch = false;

    [Range(0f, 2f)]
    public float minRange;

    [Range(0f, 2f)]
    public float maxRange;

    [HideInInspector]
    public AudioSource source;
}