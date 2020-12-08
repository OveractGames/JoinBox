
using ScriptUtils.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class NavigatorManager : Singleton<NavigatorManager>
{
    public loadingbar vicaBar;
    public GameObject loadingScreen;

    private void Start()
    {
        vicaBar.fillDone += VicaBar_fillDone;
    }

    private void VicaBar_fillDone()
    {
        Invoke("LoadGame", 0.5f);
    }

    public void LoadGame()
    {
        loadingScreen.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            Navigator.getInstance().LoadLevel("GameScene");
        });
    }
}
