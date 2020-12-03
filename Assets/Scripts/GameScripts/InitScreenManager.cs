using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class InitScreenManager : MonoBehaviour
{
    public DOTweenAnimation headerInterfaceTween;
    public DOTweenAnimation headerGameTween;
    public Image fadeScreen;
    public bool canClick = true;
    public GameObject FX;
    public GameObject playText;
    public void hideInterfaceHeader()
    {
        if (!canClick)
            return;
        StartCoroutine(hideInterfaceCoroutine());
        FX.SetActive(false);
        playText.SetActive(false);
    }
    public void showInterfaceHeader()
    {
        if (!canClick)
            return;
        StartCoroutine(showInterfaceCoroutine());
        FX.SetActive(true);
    }

    private IEnumerator showInterfaceCoroutine()
    {
        canClick = false;
        headerGameTween.DOPlayBackwards();
        yield return new WaitForSeconds(0.25f);
        headerInterfaceTween.DOPlayForward();
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.DOFade(0.35f, 0.5f).OnComplete(() =>
        { canClick = true;
            playText.SetActive(true);
        });
        Timer.Instance.StopTimer();
    }
    private IEnumerator hideInterfaceCoroutine()
    {
        canClick = false;
        headerInterfaceTween.DOPlayBackwards();
        yield return new WaitForSeconds(0.25f);
        headerGameTween.DOPlayForward();
        fadeScreen.DOPlayBackwards();
        fadeScreen.DOFade(0f, 0.5f).OnComplete(() =>
        { fadeScreen.gameObject.SetActive(false); canClick = true;
            Timer.Instance.StartTimer();
        });
    }
}
