
using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AdCaller : Singleton<AdCaller>
{
    private string adId = "3802129";
    public int maxClicks;
    public int currentClickIndex = 0;
    public bool showAd = false;
    public bool skipLevel = false;
    void Start()
    {
        Advertisement.Initialize(adId, true);
    }
    public void ShowAd()
    {
        if (!DataManager.Instance.testingAd)
        {
            if (PlayerPrefs.HasKey("NO_ADS"))
                return;
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            return;
        }
        if (Advertisement.IsReady())
        {
            LeanButton[] btns = FindObjectsOfType<LeanButton>();
            foreach (LeanButton btn in btns)
                btn.interactable = false;
            Advertisement.Show("video", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }
    public void ShowRewarded(bool skip)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            return;
        }
        skipLevel = skip;
        if (Advertisement.IsReady())
        {
            LeanButton[] btns = FindObjectsOfType<LeanButton>();
            foreach (LeanButton btn in btns)
                btn.interactable = false;
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleRewardedResult });
        }
    }

    private void HandleRewardedResult(ShowResult result)
    {
        showAd = false;
        StartCoroutine(enableMouseEvents(true));
        switch (result)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Finished:
                if (!skipLevel)
                    DataManager.Instance.AddPoints();
                break;
            case ShowResult.Skipped:
                break;
        }
    }

    private IEnumerator enableMouseEvents(bool interactable)
    {
        yield return new WaitForSeconds(1.0f);
        LeanButton[] btns = FindObjectsOfType<LeanButton>();
        foreach (LeanButton btn in btns)
            btn.interactable = interactable;
    }

    private void HandleAdResult(ShowResult result)
    {
        showAd = false;
        StartCoroutine(enableMouseEvents(true));
    }

    public void Click()
    {
        currentClickIndex++;
        if (currentClickIndex >= maxClicks)
        {
            showAd = true;
            currentClickIndex = 0;
            ShowAd();
        }
    }
}
