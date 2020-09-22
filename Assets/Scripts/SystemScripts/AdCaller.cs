
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
    void Start()
    {
        Advertisement.Initialize(adId, true);
    }
    public void ShowAd()
    {
        //if (PlayerPrefs.HasKey("NO_ADS"))
        //    return;
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
    public void ShowRewarded()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            DataManager.Instance.errorPanel.TurnOn();
            return;
        }
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
        StartCoroutine(enableMouseEvents(true));
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
        StartCoroutine(enableMouseEvents(true));
    }
    public void Click(bool next)
    {
        Level curr_level = FindObjectOfType<Level>();
        currentClickIndex++;
        if (next)
        {
            if (currentClickIndex >= maxClicks)
            {
                showAd = true;
                currentClickIndex = 0;
                ShowAd();
            }
        }
        else
        {
            if ((currentClickIndex >= maxClicks) || curr_level.clicks <= 0)
            {
                showAd = true;
                currentClickIndex = 0;
                ShowAd();
            }
        }
    }
}
