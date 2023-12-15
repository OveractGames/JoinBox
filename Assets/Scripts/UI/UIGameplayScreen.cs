using UnityEngine;
using DG.Tweening;
using Lean.Gui;
using TMPro;
using System;

public class UIGameplayScreen : UIScreen
{
    [SerializeField] private RectTransform _topMenu;

    [SerializeField] private LeanButton _pauseButton;
    [SerializeField] private LeanButton _reloadButton;

    [SerializeField] private TMP_Text _movesText;
    [SerializeField] private TMP_Text _levelText;

    [SerializeField] private UIScreen _uiOutOfMovesScreen;

    [SerializeField] private GameplayManager _gameplayManager;

    public bool _testing = false;

    private void Start()
    {
        _pauseButton.OnClick.AddListener(PauseGame);
        _reloadButton.OnClick.AddListener(Reload);
        _gameplayManager.DispatchNoMovesEvent += ShowNoMovesScreen;
        _gameplayManager.OnBoxClick += UpdateUI;
        _uiOutOfMovesScreen.OnClose += HandleOutOfMovesScreenClose;
        _gameplayManager.OnCreate += UpdateUI;
    }

    private void ShowNoMovesScreen()
    {
        _uiOutOfMovesScreen.Show();
    }

    private void HandleOutOfMovesScreenClose()
    {
        _gameplayManager.CurrentLevel.Unfreeze();
    }

    private void Reload()
    {
        GameplayManager.Instance.StartNextLevel();
    }

    private void PauseGame()
    {
        _pauseButton.gameObject.SetActive(false);
        _reloadButton.gameObject.SetActive(false);
        _topMenu.DOAnchorPosY(200f, .5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            _topMenu.gameObject.SetActive(false);
            UIController.Instance.ShowScreen<UIMainScreen>();
        });
        _gameplayManager.CurrentLevel.FreezeAll();
        Timer.Instance.StopTimer();
    }

    public void StartGame()
    {
        GameplayManager.Instance.StartNextLevel();
        _topMenu.gameObject.SetActive(true);
        _topMenu.DOAnchorPosY(-25f, .5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            _pauseButton.gameObject.SetActive(true);
            _reloadButton.gameObject.SetActive(true);
        });

        Timer.Instance.StartTimer();
        _gameplayManager.CurrentLevel.Unfreeze();
    }

    public void UpdateUI()
    {
        _movesText.SetText(_gameplayManager.Moves.ToString());
        _levelText.SetText(_gameplayManager.CurrentLevel.LevelIndex.ToString());
    }

    public override void Show()
    {
        base.Show();
        StartGame();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
