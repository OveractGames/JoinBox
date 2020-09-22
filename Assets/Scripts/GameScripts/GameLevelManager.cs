
using Lean.Gui;
using ScriptUtils.Visual;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GameLevelManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public AudioClip bgMusic;
    public GameObject starFX;
    public AudioClip click;
    public AudioClip shutterFX;
    public TextMeshProUGUI[] levelTextIndicators;
    public GameObject whitePanel;
    public GameObject backingPanel;
    public GameObject finalScreenshot;
    public GameObject finishPanel;
    public RectTransform levelParent;
    public GameObject menuScreen;
    public GameObject gameReasonPanel;
    public GameObject gameScreen;
    public _SettingManager settingManager;
    public InterfaceSountController bg_sound_controller;
    public SoundGameManager sfx_sound_controller;
    public LeanWindow loadingModal;
    public LeanWindow tuttorialModal;
    public LeanWindow backingModal;
    public LeanWindow scrollLevelModal;
    public string levelPrefix = "";
    public bool forceTest = false;
    public int currentLevelIndex = 1;
    private Level currentLevel = null;
    public int availableTips = 1;
    public bool done = false;
    public bool removeLocalStorage = false;
    private FadeScreenSystem FSCreen;
    private LeanButton[] allGameButtons;

    void Start()
    {
        allGameButtons = FindObjectsOfType<LeanButton>();
        FSCreen = FadeScreenSystem.CreateFadeScreen(FadeScreenSystem.FadeState.INVISIBLE);
        FSCreen.FadeEvent += FSCreen_FadeEvent;
        FSCreen.transform.position = new Vector3(0f, 0f, -8f);
        settingManager.ApplySettingsDelegate += SettingManager_ApplySettingsDelegate;
        tuttorialModal.gameObject.SetActive(!PlayerPrefs.HasKey("TUTTORIAL"));
        if (bg_sound_controller != null)
        {
            bg_sound_controller.SetMusic(bgMusic);
            bg_sound_controller.PlayMusic(true);
        }
        backingPanel.SetActive(true);
        if (!PlayerPrefs.HasKey("VOLUME"))
            PlayerPrefs.SetFloat("VOLUME", 0.5f);
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetInt("SFX", 1);
        if (!PlayerPrefs.HasKey("LEVEL"))
            PlayerPrefs.SetInt("LEVEL", 1);
        LoadLevel();
        SetSettings();
        menuScreen.GetComponent<RectTransform>().DOAnchorPosY(-76f, 0.35f).SetEase(Ease.OutBack);
    }


    private void FSCreen_FadeEvent(FadeScreenSystem target, FadeScreenSystem.FadeState state)
    {
        if (state == FadeScreenSystem.FadeState.VISIBLE)
        {
            LoadLevel();
            finalScreenshot.SetActive(false);
            finishPanel.SetActive(false);
            gameScreen.SetActive(true);
            menuScreen.SetActive(true);
            backingModal.TurnOff();
            starFX.SetActive(false);
        }
        if (state == FadeScreenSystem.FadeState.INVISIBLE)
        {
            foreach (LeanButton btn in allGameButtons)
                btn.interactable = true;
            menuScreen.GetComponent<RectTransform>().DOAnchorPosY(-76f, 0.35f).SetEase(Ease.OutBack);
        }
    }
    private void SettingManager_ApplySettingsDelegate()
    {
        SetSettings();
    }
    public void TuttorialDone()
    {
        PlayerPrefs.SetInt("TUTTORIAL", 1);
    }
    private void SetSettings()
    {
        bg_sound_controller.SetMusicVolume(PlayerPrefs.GetFloat("VOLUME"));
        sfx_sound_controller.SetEffectsVolume(PlayerPrefs.GetInt("SFX") == 1 ? 1.0f : 0.0f);
    }
    private void Update()
    {
        if (removeLocalStorage)
        {
            PlayerPrefs.DeleteAll();
            removeLocalStorage = false;
        }
    }
    public void LoadLevel()
    {        
        loadingModal.TurnOff();
        if (!forceTest)
            currentLevelIndex = PlayerPrefs.GetInt("LEVEL");
        string path = "Levels/" + levelPrefix + (currentLevelIndex.ToString());
        Level level_prefab = Resources.Load<Level>(path);
        DataManager.Instance.currentLevelIndex = currentLevelIndex;
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        if (level_prefab != null)
        {
            availableTips = PlayerPrefs.GetInt("Tips");
            currentLevel = Instantiate(level_prefab, Vector3.zero, Quaternion.identity);
            currentLevel.transform.SetParent(levelParent);
            currentLevel.transform.localScale = Vector3.one;
            currentLevel.transform.position = Vector3.zero;
            currentLevel.GetComponent<RectTransform>().anchoredPosition = new Vector3(levelParent.anchoredPosition.x, levelParent.anchoredPosition.y);
            currentLevel.OnLevelDone += Done;
            levelTextIndicators[0].SetText(currentLevelIndex.ToString());
            levelTextIndicators[1].transform.parent.gameObject.SetActive(currentLevelIndex > 1);
            if (levelTextIndicators[1].transform.parent.gameObject.activeSelf)
                levelTextIndicators[1].SetText((currentLevelIndex - 1).ToString());
            levelTextIndicators[2].SetText((currentLevelIndex + 1).ToString());
        }
    }
    private void Done()
    {
        Invoke("Freeze", 2.0f);
        Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();
        var s = Resources.Load<AudioClip>("doneFX");
        if (s != null)
            sfx_sound_controller.PlaySound(s);
        foreach (Rigidbody2D rb in rbs)
        {
            rb.simulated = false;
            rb.isKinematic = true;
        }
        menuScreen.GetComponent<RectTransform>().DOAnchorPosY(300f, 0.35f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            ScreenshootManager.Instance.InitScreenCapture();
            Invoke("ShowScreenshot", 0.5f);
        });
    }
    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://overact-games.github.io/privacy_policy.html");
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("LEVEL");
        loadingScreen.SetActive(true);
        Invoke("Start", 2.0f);
    }
    private void ShowScreenshot()
    {
        var path = Application.persistentDataPath + "/Screenshot_" + PlayerPrefs.GetInt("LEVEL") + ".png";
        if (System.IO.File.Exists(path))
        {
            finalScreenshot.SetActive(true);
            var sp = ScreenshootManager.Instance.LoadSprite(path);
            gameScreen.SetActive(false);
            backingModal.TurnOn();
            DataManager.Instance.helpPanel.TurnOff();
            finalScreenshot.transform.GetChild(0).GetComponent<Image>().sprite = sp;
        }
        sfx_sound_controller.PlaySound(shutterFX);
    }
    private void Freeze()
    {
        gameReasonPanel.SetActive(true);
    }
    public void enableWhitePanel()
    {
        whitePanel.SetActive(true);
    }
    public void Level_OnLevelDone(bool reload)
    {
        foreach (LeanButton btn in allGameButtons)
            btn.interactable = false;
        FSCreen.FadeInOut();
        if (!reload)
        {
            currentLevelIndex++;
            PlayerPrefs.SetInt("LEVEL", currentLevelIndex);
            DataManager.Instance.currentLevelIndex = currentLevelIndex;
        }
        else
        {
            CancelInvoke();
        }
    }
}
