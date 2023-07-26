using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Ink.Runtime;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource, _effectsSource, _vaSource;
    
    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlayAmbience(string clipName)
    {
        if (clipName == "Nocturne")
        {
            _musicSource.volume = 0.86f;
        }
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Music/{clipName}");
        Debug.Log("PlayAmbience has been called: " + (clip == null));

        Debug.Log($"Audio/Music/{clipName}");
        Debug.Log(clip);
        _musicSource.clip = clip;
        _musicSource.Play();
    }
    
    public void PlayEffect(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Effects/{clipName}");
        Debug.Log($"Audio/Effects/{clipName}");
        Debug.Log(clip);
        _effectsSource.clip = clip;
        _effectsSource.Play();
    }

    public void PlayVoiceOver(string character, string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/VA/{character}/{clipName}");
        Debug.Log($"Audio/VA/{character}/{clipName}");
        Debug.Log(clip);
        _vaSource.clip = clip;
        _vaSource.Play();
    }
}
    