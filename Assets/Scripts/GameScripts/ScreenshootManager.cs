
using System;
using System.Collections;
using UnityEngine;
public class ScreenshootManager : Singleton<ScreenshootManager>
{
    public void InitScreenCapture()
    {
        string imagePath = Application.persistentDataPath + "/Screenshot_" + PlayerPrefs.GetInt("LEVEL") + ".png";
        StartCoroutine(captureScreenshot(imagePath));
    }
    IEnumerator captureScreenshot(string imagePath)
    {
        yield return new WaitForEndOfFrame();
        //about to save an image capture
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        Debug.Log(" screenImage.width" + screenImage.width + " texelSize" + screenImage.texelSize);
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();
        Debug.Log("imagesBytes=" + imageBytes.Length);
        //Save image to file
        System.IO.File.WriteAllBytes(imagePath, imageBytes);
        string path = Application.persistentDataPath + "/Screenshot_" + PlayerPrefs.GetInt("LEVEL") + ".png";
        Debug.Log(Application.persistentDataPath + "/Screenshot_" + PlayerPrefs.GetInt("LEVEL") + ".png");
        LoadSprite(path);
    }
    public Sprite LoadSprite(string path)
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
