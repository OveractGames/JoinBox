
using ScriptUtils.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class NavigatorManager : Singleton<NavigatorManager>
{
    public GameObject loadingScreen;
    public void LoadGame()
    {
        loadingScreen.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            Navigator.getInstance().LoadLevel("GameScene");
        });
    }
}
