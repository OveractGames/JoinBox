using Lean.Transition.Method;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] private LeanPlaySound[] _leanSoundTransitions;
    [SerializeField] private AudioSource _backgroundVoiceSource;

    private float _sfxVolume = 1f;
    private bool _voiceActive = true;

    private new void Awake()
    {
        // Cache LeanPlaySound objects on Awake to avoid FindObjectsOfType at runtime.
        _leanSoundTransitions = FindObjectsOfType<LeanPlaySound>(true);
    }

    public void ToggleSfx(bool active)
    {
        _sfxVolume = active ? 1f : 0f;
        foreach (var leanSound in _leanSoundTransitions)
        {
            if (leanSound != null && leanSound.Data != null)
            {
                leanSound.Data.Volume = _sfxVolume;
            }
        }
    }

    public void ToggleVoice(bool active)
    {
        _voiceActive = active;
        if (active)
        {
            _backgroundVoiceSource.Play();
        }
        else
        {
            _backgroundVoiceSource.Stop();
        }
    }

    public float GetSfxVolume()
    {
        return _sfxVolume;
    }

    public bool IsVoiceActive()
    {
        return _voiceActive;
    }
}
