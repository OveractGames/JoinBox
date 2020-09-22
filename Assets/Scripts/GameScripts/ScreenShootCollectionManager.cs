using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShootCollectionManager : MonoBehaviour
{
    LeanWindow window;
    public Image screenshot;
    public LeanButton leftArrow;
    public LeanButton rightArrow;
    public int currentScreenshootIndex;
    private Sprite _sprite = null;
    void Start()
    {
        window = GetComponent<LeanWindow>();
        window.OnOn.AddListener(LoadScreenshots);
    }
    private void LoadScreenshots()
    {
        currentScreenshootIndex = PlayerPrefs.GetInt("LEVEL");
        var path = Application.persistentDataPath + "/Screenshot_" + (currentScreenshootIndex - 1) + ".png";
        _sprite = LoadSprite(path);
        screenshot.sprite = _sprite;
        LoadCurrentScreenshot(currentScreenshootIndex);
    }

    private void LoadCurrentScreenshot(int index)
    {
        var path = Application.persistentDataPath + "/Screenshot_" + index + ".png";
        if (!System.IO.File.Exists(path))
        {
            currentScreenshootIndex--;
            return;
        }
        _sprite = LoadSprite(path);
        screenshot.sprite = _sprite;
    }
    public void LeftClick()
    {
        if (!screenshotExist(currentScreenshootIndex - 1))
            return;
        currentScreenshootIndex--;
        LoadCurrentScreenshot(currentScreenshootIndex);
    }

    private bool screenshotExist(int index)
    {
        return System.IO.File.Exists(Application.persistentDataPath + "/Screenshot_" + (index) + ".png");
    }
    public void RightClick()
    {
        if (!screenshotExist(currentScreenshootIndex + 1))
            return;
        currentScreenshootIndex++;
        LoadCurrentScreenshot(currentScreenshootIndex);
    }
    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
}
