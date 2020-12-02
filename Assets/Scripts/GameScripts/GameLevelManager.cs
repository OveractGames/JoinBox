
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

public class GameLevelManager : MonoBehaviour
{
    public GameObject starFX;
    public AudioClip shutterFX;
    public GameObject finalScreenshot;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finishedLevelText;
    public RectTransform levelParent;
    public DOTweenAnimation menuScreenAnim;
    public GameObject gameScreen;
    public _SettingManager settingManager;
    public LeanWindow loadingModal;
    public LeanWindow backingModal;
    public LeanWindow noMoreLevelsModal;
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
     
        if (!PlayerPrefs.HasKey("VOLUME"))
            PlayerPrefs.SetFloat("VOLUME", 0.5f);
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetInt("SFX", 1);
        if (!PlayerPrefs.HasKey("LEVEL"))
            PlayerPrefs.SetInt("LEVEL", 1);
        LoadLevel();
    }


    private void FSCreen_FadeEvent(FadeScreenSystem target, FadeScreenSystem.FadeState state)
    {
        if (state == FadeScreenSystem.FadeState.VISIBLE)
        {
            LoadLevel();
            finalScreenshot.SetActive(false);
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
        }
    }
    public void TuttorialDone()
    {
        PlayerPrefs.SetInt("TUTTORIAL", 1);
    }
    private void Update()
    {
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
        //loadingModal.TurnOff();

        if (!forceTest)
            currentLevelIndex = PlayerPrefs.GetInt("LEVEL");
        string path = "Levels/" + levelPrefix + (currentLevelIndex.ToString());
        Level level_prefab = Resources.Load<Level>(path);
        if(level_prefab == null)
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
        starFX.SetActive(true);
        Timer.Instance.StopTimer();
        Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();
        var s = Resources.Load<AudioClip>("doneFX");
        foreach (Rigidbody2D rb in rbs)
        {
            rb.simulated = false;
            rb.isKinematic = true;
        }
        //timerText.SetText("Time: " + Timer.Instance._textMesh.text);
        string mark = "red";
        timerText.SetText("You have completed the level in " + "<color=" + mark + ">" + FindObjectOfType<Level>().clickCount + "</color>" + " moves!" + "\n" + "Time: " + Timer.Instance._textMesh.text);
        //You Cleared with <color="red">7</color> moves
        //Time: < color = "green" > 00:35 </ color >
        finishedLevelText.SetText("Perfect! " + "\n" + "<size='26'>" + "LEVEL " + currentLevelIndex + " COMPLETED!");
        ScreenshootManager.Instance.InitScreenCapture();
        Invoke("ShowScreenshot", 0.5f);
    }
    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://overact-games.github.io/privacy_policy.html");
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("LEVEL");
        for (int i = 0; i < DataManager.Instance.currentLevelIndex + 5; i++)
        {
            string key = "level" + i;
            if (PlayerPrefs.HasKey(key))
                PlayerPrefs.DeleteKey(key);
        }
        loadingModal.TurnOn();
        loadingModal.GetComponentInChildren<Loading>().SetText("Reseting...");
        foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
        {
            DirectoryInfo data_dir = new DirectoryInfo(directory);
            data_dir.Delete(true);
        }
        foreach (var file in Directory.GetFiles(Application.persistentDataPath))
        {
            FileInfo file_info = new FileInfo(file);
            file_info.Delete();
        }
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
            finalScreenshot.transform.GetChild(0).GetComponent<Image>().sprite = sp;
        }
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
