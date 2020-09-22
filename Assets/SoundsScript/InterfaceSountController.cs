using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSountController : MonoBehaviour
{
    private AudioSource _music;
    private bool _musicChanged = false;
    private bool _musicIsPlaying = false;

    //Getters & Setters
    public float SoundsVolume
    {
        get => AudioListener.volume;
    }
    public float MusicVolume 
    {
        get => _music.volume;
    }
    private void Awake()
    {
        _music = gameObject.AddComponent<AudioSource>();
        _music.ignoreListenerVolume = true;
        _music.loop = true;
        //_sounds = gameObject.AddComponent<AudioSource>();
    }


    public void SetEffectsVolume(float newValue)
    {
        AudioListener.volume = newValue;
    }
    public void SetMusicVolume(float newValue)
    {
        _music.volume = newValue;
    }
    public void SetMusic(AudioClip sound)
    {
        _music.clip = sound;
        _musicChanged = true;
    }
    public void PlayMusic(bool play)
    {
        if (!_musicChanged && _musicIsPlaying) return;
        _musicIsPlaying = true;
        _musicChanged = false;

        if (play)
            _music.Play();
        else
            _music.Stop();
    }

    public void PlaySound(AudioClip sound)
    {
        //_sounds.PlayOneShot(sound);
    }

    public bool IsMusicPlaying()
    {
        return _musicIsPlaying;
    }
}
