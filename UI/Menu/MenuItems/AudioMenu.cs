using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : UIMenu
{
    [SerializeField] AudioMixer MusicAudio = null;
    [SerializeField] AudioMixer AmbienceAudio = null;
    [SerializeField] AudioMixer SoundFXAudio = null;


    public void SetMusicVolume(float volume)
    {
        MusicAudio.SetFloat("MusicVolume", volume);
    }

    public void SetAmbienceVolume(float volume)
    {
        AmbienceAudio.SetFloat("AmbienceVolume", volume);
    }

    public void SetSoundFXVolume(float volume)
    {
        SoundFXAudio.SetFloat("SoundFXVolume", volume);
    }

}
