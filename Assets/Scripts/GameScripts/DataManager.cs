
using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public ClickText clickText;
    public LeanWindow errorPanel;
    public LeanWindow skippedPanel;
    public LeanWindow helpPanel;
    public TextMeshProUGUI tipsText;
    public GameObject noAdsButton;
    private int tipsAmount = 0;

    public int currentLevelIndex = 0;

    public GameObject[] panels;
    public GameObject screenshotImg;

    public void AddTips(int amount)
    {
        tipsAmount = PlayerPrefs.GetInt("Tips");
        tipsAmount += amount;
        tipsText.SetText(tipsAmount.ToString());
        PlayerPrefs.SetInt("Tips", tipsAmount);
    }
    public void RemoveAds()
    {
        PlayerPrefs.SetString("NO_ADS", "OFF");
        noAdsButton.SetActive(false);
    }
    public void ShowHelpPanel()
    {
        helpPanel.TurnOn();
    }

}
