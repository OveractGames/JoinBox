using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class _SettingManager : MonoBehaviour
{
    private AudioSource[] allAudioSources;
    public InterfaceSountController sManager;
    public LeanWindow settingWindow;
    public Toggle sfxToggle;
    public Slider volumeSlider;
    public event UnityAction ApplySettingsDelegate;
    private void Awake()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        sfxToggle.onValueChanged.AddListener(delegate { SetEffects(); });
        settingWindow.OnOff.AddListener(delegate { ApplySettings(); });
        settingWindow.OnOn.AddListener(delegate { GetSettings(); });
    }

    private void GetSettings()
    {
        sfxToggle.isOn = PlayerPrefs.GetInt("SFX") == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("VOLUME") * 100;
    }

    private void ApplySettings()
    {
        if (ApplySettingsDelegate != null)
            ApplySettingsDelegate.Invoke();
    }

    private void SetEffects()
    {
        PlayerPrefs.SetInt("SFX", sfxToggle.isOn ? 1 : 0);
    }

    private void SetVolume()
    {
        var value = volumeSlider.value / 100F;
        sManager.SetMusicVolume(value);
        PlayerPrefs.SetFloat("VOLUME", value);
    }

    public void StopAllAudio(bool stop)
    {
        foreach (AudioSource audioS in allAudioSources)
            audioS.volume = stop ? 0f : 100f;
        volumeSlider.value = stop ? 0f : 100f;
        sfxToggle.isOn = !stop;
    }
}
