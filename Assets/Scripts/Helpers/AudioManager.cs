using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType
{
    CLICK = 0,
    WIN = 1,
    DESTROY = 2,
    COLLECT = 3,
    LOST = 4,
    FIREWORK = 5,
    OUT_OF_MOVES = 6
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Sound[] _clips;

    public void Play(SoundType type)
    {
        Sound sound = System.Array.Find(_clips, s => s.Type == type);
        if (sound != null && sound.Clip != null)
        {
            GameObject soundObject = new GameObject("Sound_" + type);
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = sound.Clip;
            source.Play();

            // Clean up the AudioSource and GameObject after the sound finishes playing
            Destroy(soundObject, sound.Clip.length);
        }
        else
        {
            Debug.LogWarning("Sound not found or AudioClip missing for type: " + type);
        }
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip Clip;
    public SoundType Type;
}
