using Lean.Gui;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class _SettingManager : MonoBehaviour
{
    public AudioSource sSource;
    public AudioClip bgMusic;
    public LeanToggle sfxToggle;
    public LeanToggle soundsToggle;
    public LeanToggle vibrationToggle;
    public Slider volumeSlider;
    private IEnumerator Start()
    {
        if (!PlayerPrefs.HasKey("VIBRATION")) PlayerPrefs.SetInt("VIBRATION", 1);
        if (!PlayerPrefs.HasKey("SOUNDS")) PlayerPrefs.SetInt("SOUNDS", 1);
        if (!PlayerPrefs.HasKey("SFX")) PlayerPrefs.SetInt("SFX", 1);
        if (!PlayerPrefs.HasKey("BG_VOLUME")) PlayerPrefs.SetFloat("BG_VOLUME", 100);
        yield return new WaitForSeconds(0.5f);

        volumeSlider.onValueChanged.AddListener(delegate { setVolumeLevel(); });
        vibrationToggle.Set(PlayerPrefs.GetInt("VIBRATION") == 1);
        sfxToggle.Set(PlayerPrefs.GetInt("SFX") == 1);
        soundsToggle.Set(PlayerPrefs.GetInt("SOUNDS") == 1);
        volumeSlider.value = PlayerPrefs.GetFloat("BG_VOLUME");
    }
    public void setVibrationState(bool on)
    {
        PlayerPrefs.SetInt("VIBRATION", on ? 1 : 0);
        if (on)
            Handheld.Vibrate();
    }

    public void setEffectsState(bool on)
    {
        PlayerPrefs.SetInt("SFX", on ? 1 : 0);
        Lean.Transition.Method.LeanPlaySound[] allSFX = FindObjectsOfType<Lean.Transition.Method.LeanPlaySound>();
        foreach (Lean.Transition.Method.LeanPlaySound sfx in allSFX)
            sfx.Data.Volume = on ? 100f : 0f;
    }

    public void setSoundState(bool on)
    {
        sSource.mute = !on;
        volumeSlider.interactable = on;
        if (!on)
            sSource.Stop();
        else
            sSource.Play();
        sSource.volume = volumeSlider.value / 100f;
        PlayerPrefs.SetInt("SOUNDS", on ? 1 : 0);
    }

    public void setVolumeLevel()
    {
        var value = volumeSlider.value / 100f;
        PlayerPrefs.SetFloat("BG_VOLUME", volumeSlider.value);
        sSource.volume = value;
    }
}
