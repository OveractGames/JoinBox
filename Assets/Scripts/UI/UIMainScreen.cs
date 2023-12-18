using Lean.Gui;
using System;
using UnityEngine;

public class UIMainScreen : UIScreen
{
    [SerializeField] private LeanButton _coinsButton;
    [SerializeField] private LeanButton _playButton;
    [SerializeField] private LeanButton _shopButton;
    [SerializeField] private LeanButton _levelsButton;

    [Header("SETTINGS")]
    [SerializeField] private LeanButton sfxButton;
    [SerializeField] private LeanButton voiceButton;

    [SerializeField] private GameObject _sfxInnactiveBar;
    [SerializeField] private GameObject _voiceInnactiveBar;

    private bool sfxEnabled = true;
    private bool voiceEnabled = true;

    private void Start()
    {
        _coinsButton.OnClick.AddListener(OpenShop);
        _playButton.OnClick.AddListener(PlayGame);
        _shopButton.OnClick.AddListener(OpenShop);
        _levelsButton.OnClick.AddListener(OpenLevelsScreen);

        sfxEnabled = PlayerPrefs.GetInt("sfxEnabled", 1) == 1;
        voiceEnabled = PlayerPrefs.GetInt("voiceEnabled", 1) == 1;

        SettingsManager.Instance.ToggleSfx(sfxEnabled);
        SettingsManager.Instance.ToggleVoice(voiceEnabled);

        sfxButton.OnClick.AddListener(ToggleSFX);
        voiceButton.OnClick.AddListener(ToggleVoice);
    }

    private void PlayGame()
    {
        UIController.Instance.ShowScreen<UIGameplayScreen>();
    }

    private void OpenShop()
    {
        UIController.Instance.ShowScreen<UIShopScreen>();
    }

    private void OpenLevelsScreen()
    {
        UIController.Instance.ShowScreen<UILevelsScreen>();
    }

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;

        PlayerPrefs.SetInt("sfxEnabled", sfxEnabled ? 1 : 0);
        _sfxInnactiveBar.SetActive(!sfxEnabled);

        SettingsManager.Instance.ToggleSfx(sfxEnabled);
    }

    public void ToggleVoice()
    {
        voiceEnabled = !voiceEnabled;

        PlayerPrefs.SetInt("voiceEnabled", voiceEnabled ? 1 : 0);
        _voiceInnactiveBar.SetActive(!voiceEnabled);

        SettingsManager.Instance.ToggleVoice(voiceEnabled);
    }

    public override void Show()
    {
        PlayerPrefs.SetInt("BOOT", 1);
        _sfxInnactiveBar.SetActive(!sfxEnabled);
        _voiceInnactiveBar.SetActive(!voiceEnabled);

        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
