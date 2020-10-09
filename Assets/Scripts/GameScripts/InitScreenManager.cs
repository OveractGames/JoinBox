using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InitScreenManager : MonoBehaviour
{
    public RectTransform logo;
    public RectTransform menuRect;
    public LeanButton playButton;
    public GameObject menuScreen;
    public LeanButton[] _myButtons;
    private void Start()
    {
        ShowInitScreen();
    }
    public void DoPlay()
    {
        logo.DOAnchorPosY(400f, 0.5f).SetEase(Ease.InOutBack);
        menuRect.DOAnchorPosY(300f, 0.5f).SetEase(Ease.InOutBack);
        playButton.GetComponent<RectTransform>().DOAnchorPosY(-300f, 0.5f).SetEase(Ease.InOutBack);
        playButton.interactable = false;
        foreach (LeanButton btn in _myButtons)
            btn.interactable = false;
        GetComponent<Image>().DOFade(0f, 0.5f).SetDelay(0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            menuScreen.GetComponent<DOTweenAnimation>().DOPlay();
            Timer.Instance.StartTimer();
        });
    }
    public void ShowInitScreen()
    {
        Timer.Instance.StopTimer();
        logo.DOAnchorPosY(-400f, 0.5f).SetEase(Ease.OutBack);
        menuRect.DOAnchorPosY(-53.75f, 0.5f).SetEase(Ease.OutBack);
        foreach (LeanButton btn in _myButtons)
            btn.interactable = true;
        playButton.GetComponent<RectTransform>().DOAnchorPosY(300f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            playButton.interactable = true;
        });
        GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
        {
            menuScreen.GetComponent<DOTweenAnimation>().DORewind();
        });
    }
}
