
using Lean.Gui;
using ScriptUtils.Visual;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.IO;
using System.Collections;
using ScriptUtils.Interface;

public class GameLevelManager : MonoBehaviour
{
    public GameObject starFX;
    public AudioClip shutterFX;
    public AudioClip finishFX;
    public GameObject loadingScreen;
    public GameObject FX_manager;
    public DOTweenAnimation headerGameTween;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finishedLevelText;
    public RectTransform levelParent;
    public DOTweenAnimation menuScreenAnim;
    public GameObject gameScreen;
    public _SettingManager settingManager;
    public LeanWindow loadingModal;
    public LeanWindow backingModal;
    public LeanWindow noMoreLevelsModal;
    public LeanWindow finishModal;
    public bool levelDone = false;
    public string levelPrefix = "";
    public bool forceTest = false;
    public int currentLevelIndex = 1;
    private Level currentLevel = null;
    public bool done = false;
    public bool removeLocalStorage = false;
    private FadeScreenSystem FSCreen;
    private LeanButton[] allGameButtons;
    private bool reset = false;
    void Start()
    {
        allGameButtons = FindObjectsOfType<LeanButton>();
        FSCreen = FadeScreenSystem.CreateFadeScreen(FadeScreenSystem.FadeState.INVISIBLE);
        FSCreen.FadeEvent += FSCreen_FadeEvent;
        FSCreen.transform.position = new Vector3(0f, 0f, -8f);
        Init();
    }


    private void Init()
    {
        if (!PlayerPrefs.HasKey("LEVEL"))
            PlayerPrefs.SetInt("LEVEL", 1);
        LoadLevel();
    }


    private void FSCreen_FadeEvent(FadeScreenSystem target, FadeScreenSystem.FadeState state)
    {
        if (state == FadeScreenSystem.FadeState.VISIBLE)
        {
            LoadLevel();
            gameScreen.SetActive(true);
            backingModal.TurnOff();
            starFX.SetActive(false);
            DataManager.Instance.leanPanel.gameObject.SetActive(true);
            DataManager.Instance.leanPanel.TurnOff();
            menuScreenAnim.DORewind();
            if (reset)
            {
                reset = false;
            }
            DataManager.Instance.failingModal.TurnOff();
        }
        if (state == FadeScreenSystem.FadeState.INVISIBLE)
        {
            loadingModal.TurnOff();
            foreach (LeanButton btn in allGameButtons)
                btn.interactable = true;
            menuScreenAnim.DOPlay();
            DataManager.Instance.failingModal.TurnOff();
            headerGameTween.DOPlayForward();
        }
    }
    public void TuttorialDone()
    {
        PlayerPrefs.SetInt("TUTTORIAL", 1);
    }
    private void Update()
    {
        if (reset)
            return;
        if (removeLocalStorage)
        {
            PlayerPrefs.DeleteAll();
            removeLocalStorage = false;
        }
        if (!levelDone)
        {
            if (starFX.activeSelf)
                starFX.SetActive(false);
        }
        if (levelDone)
        {
            if (DataManager.Instance.failingModal.On)
                DataManager.Instance.failingModal.TurnOff();
        }
    }

    public bool HasLevels()
    {
        string path = "Levels/" + levelPrefix + (currentLevelIndex.ToString());
        Level level_prefab = Resources.Load<Level>(path);
        return level_prefab != null;
    }
    public void LoadLevel()
    {
        if (!forceTest)
            currentLevelIndex = PlayerPrefs.GetInt("LEVEL");
        string path = "Levels/" + levelPrefix + (currentLevelIndex.ToString());
        Level level_prefab = Resources.Load<Level>(path);
        if (level_prefab == null)
        {
            if (currentLevel != null)
                Destroy(currentLevel.gameObject);
            Debug.Log("Level not exist");
            noMoreLevelsModal.TurnOn();
            starFX.gameObject.SetActive(true);
            return;
        }
        DataManager.Instance.currentLevelIndex = currentLevelIndex;
        DataManager.Instance.updateLevelText();
        Timer.Instance.ResetTimer();
        Timer.Instance.StartTimer();
        DataManager.Instance.failingModal.TurnOff();
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        if (level_prefab != null)
        {
            currentLevel = Instantiate(level_prefab, Vector3.zero, Quaternion.identity);
            currentLevel.transform.SetParent(levelParent);
            currentLevel.transform.localScale = Vector3.one;
            currentLevel.transform.position = Vector3.zero;
            currentLevel.GetComponent<RectTransform>().anchoredPosition = new Vector3(levelParent.anchoredPosition.x, levelParent.anchoredPosition.y);
            currentLevel.OnLevelDone += Done;
            levelDone = false;
        };
    }

    public void OpenAppLink(string link)
    {
        Application.OpenURL(link);
    }
    private void Done()
    {
        levelDone = true;
        DataManager.Instance.leanPanel.gameObject.SetActive(false);
        DataManager.Instance.failingModal.TurnOff();

        Timer.Instance.StopTimer();
        Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();
        SoundGameManager.Instance.PlaySound(finishFX);
        headerGameTween.DOPlayBackwards();
        foreach (Rigidbody2D rb in rbs)
        {
            rb.simulated = false;
            rb.isKinematic = true;
        }
        string mark = "red";
        timerText.SetText("You have completed the level in " + "<color=" + mark + ">" + FindObjectOfType<Level>().clickCount + "</color>" + " moves!" + "\n" + "Time: " + Timer.Instance._textMesh.text);
        finishedLevelText.SetText("Perfect! " + "\n" + "<size='26'>" + "LEVEL " + currentLevelIndex + " COMPLETED!");
        StartCoroutine(destroyOtherEnemies());
    }


    private IEnumerator destroyOtherEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("enemy");
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < allEnemies.Length; i++)
        {
            Destroy(allEnemies[i]);
            EnemyBox enemyBox = allEnemies[i].GetComponent<EnemyBox>();
            ParticleSystem effect = Instantiate(currentLevel.particles[enemyBox.effectIndex], new Vector3(enemyBox.transform.position.x, enemyBox.transform.position.y, -1.5f), Quaternion.identity);
            Destroy(effect.gameObject, 2.0f);
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                Handheld.Vibrate();
            if (PlayerPrefs.GetInt("SFX") == 1)
                SoundGameManager.Instance.PlaySound(shutterFX);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);
        currentLevel.GetComponent<Image>().raycastTarget = false;
        starFX.SetActive(true);
        finishModal.TurnOn();
    }
    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://overact-games.github.io/privacy_policy.html");
    }
    public void ResetProgress()
    {
        reset = true;
        //loadingModal.TurnOn();
        //loadingModal.GetComponentInChildren<Loading>().SetText("Reseting...");
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        bool isBuyed = PlayerPrefs.HasKey("NO_ADS");
        PlayerPrefs.DeleteAll();
        if (isBuyed)
            PlayerPrefs.SetString("NO_ADS", "OFF");
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("Init");
        Destroy(gameObject);
        Destroy(FX_manager);
    }
    public void Level_OnLevelDone(bool reload)
    {
        StartCoroutine(loadLevelCoroutine(reload));
    }
    public IEnumerator loadLevelCoroutine(bool reload)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (LeanButton btn in allGameButtons)
            btn.interactable = false;
        FSCreen.FadeInOut();
        if (!reload)
        {
            currentLevelIndex++;
            PlayerPrefs.SetInt("LEVEL", currentLevelIndex);
            DataManager.Instance.currentLevelIndex = currentLevelIndex;
            string levelKey = "level" + currentLevelIndex.ToString();
            PlayerPrefs.SetInt(levelKey, 1);
        }
    }
    public void ResetLoad()
    {
        foreach (LeanButton btn in allGameButtons)
            btn.interactable = false;
        FSCreen.FadeInOut();
        reset = true;
    }
}
