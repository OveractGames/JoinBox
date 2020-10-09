using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShootLoader : MonoBehaviour
{
    public LeanWindow parentWindow;
    public int levelIndex;
    public Image screenshotImage;
    public bool screenshotLoaded = false;
    public TextMeshProUGUI levelText;
     
    private void LoadScreenshot()
    {
        levelIndex = transform.GetSiblingIndex() + 1;
        var path = Application.persistentDataPath + "/Screenshot_" + levelIndex + ".png";
        if (System.IO.File.Exists(path))
        {
            var sc = ScreenshootManager.Instance.LoadSprite(path);
            screenshotImage.sprite = sc;
        }
        else
        {
            screenshotImage.sprite = null;
        }
    }
    private void Start()
    {
        parentWindow.OnOn.AddListener(LoadScreenshot);
        SetText();
    }
    public void SetText()
    {
        levelText.text = "Level " + (transform.GetSiblingIndex() + 1).ToString();
    }
    private void Update()
    {
        if (parentWindow.On == true)
        {
            if (screenshotImage.sprite == null)
                LoadScreenshot();
            screenshotLoaded = screenshotImage.sprite != null;
        }
    }
}
