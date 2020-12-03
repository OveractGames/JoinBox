using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class DataManager : Singleton<DataManager>
{
    public int points;
    public bool testingAd = false;
    public LeanWindow leanPanel;
    public LeanWindow failingModal;
    public LeanButton skipButton;
    public TextMeshProUGUI levelText;
    public int currentLevelIndex = 0;

    private void Start()
    {
        failingModal.OnOn.AddListener(FailingModalOn);
    }

    private void FailingModalOn()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            skipButton.interactable = false;
            Debug.Log("Error. Check internet connection!");
            return;
        }
        skipButton.interactable = true;
    }

    public void updateLevelText()
    {
        levelText.SetText("Level " + currentLevelIndex.ToString());
    }
    public void RemoveAds()
    {
        PlayerPrefs.SetString("NO_ADS", "OFF");
        NoAdsButton.Instance.RemoveAds();
    }
    public void Init(int points)
    {
        this.points = points;
        ClickText.Instance.SetText(points);
    }
    public void AddPoints()
    {
        points++;
        ClickText.Instance.SetText(points);
    }
    public void takePoints()
    {
        if (points == 0)
            return;
        points--;
        ClickText.Instance.SetText(points);
    }
}
