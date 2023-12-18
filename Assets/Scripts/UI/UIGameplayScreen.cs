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
    [SerializeField] private LeanButton _finishLevelButton;

    [SerializeField] private TMP_Text _movesText;
    [SerializeField] private TMP_Text _levelText;

    [SerializeField] private GameObject _bombsCircleContainer;
    [SerializeField] private GameObject _reloadsCircleContainer;

    [SerializeField] private TMP_Text _bombsCountText;
    [SerializeField] private TMP_Text _reloadsCountText;

    [SerializeField] private UIOutOfMovesScreen _uiOutOfMovesScreen;

    [SerializeField] private GameplayManager _gameplayManager;

    public bool _testing = false;

    private bool _haveBombs = false;
    private bool _haveReloads = false;

    private void Start()
    {
        _pauseButton.OnClick.AddListener(PauseGame);
        _reloadButton.OnClick.AddListener(Reload);
        _finishLevelButton.OnClick.AddListener(ForceFinishLevel);
        _gameplayManager.DispatchNoMovesEvent += ShowNoMovesScreen;
        _gameplayManager.OnBoxClick += UpdateUI;
        _uiOutOfMovesScreen.OnClose += HandleOutOfMovesScreenClose;
        _gameplayManager.OnCreate += UpdateUI;
        _gameplayManager.OnLevelComplete += LevelComplete;
    }

    private void LevelComplete()
    {
        PlayerPrefsManager.Instance.IncreaseLevel();
        _uiOutOfMovesScreen.Hide();
    }

    private void ForceFinishLevel()
    {
        if (_haveBombs)
        {
            PlayerPrefsManager.Instance.DecreaseBomb();
            UpdateUI();
            _gameplayManager.DestroyCurrentLevelAndMoveToNext();
        }
        else
        {
            string txt = $"Watch a short video to get +1 bomb.";
            _uiOutOfMovesScreen.Show(txt, RewardType.BOMBS);
        }
    }

    private void ShowNoMovesScreen(string text, RewardType type)
    {
        if (_uiOutOfMovesScreen.IsActive)
        {
            return;
        }
        AudioManager.Instance.Play(SoundType.OUT_OF_MOVES);
        _uiOutOfMovesScreen.Show(text, type);
    }

    private void HandleOutOfMovesScreenClose()
    {
        _gameplayManager.CurrentLevel.Unfreeze();
    }

    private void Reload()
    {
        if (_haveReloads)
        {
            PlayerPrefsManager.Instance.DecreaseReloads();
            if (_gameplayManager.IsOneLevelGame)
            {
                _gameplayManager.PlayLevel(_gameplayManager.LevelIndex);
            }
            else
            {
                _gameplayManager.StartNextLevel();
            }
        }
        else
        {
            string txt = $"Watch a short video to get +2 reloads.";
            _uiOutOfMovesScreen.Show(txt, RewardType.RELOADS);
        }
    }

    private void PauseGame()
    {
        _pauseButton.gameObject.SetActive(false);
        _reloadButton.gameObject.SetActive(false);
        _finishLevelButton.gameObject.SetActive(false);
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
        _gameplayManager.StartNextLevel();
        _topMenu.gameObject.SetActive(true);
        _topMenu.DOAnchorPosY(-25f, .5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            _pauseButton.gameObject.SetActive(true);
            _reloadButton.gameObject.SetActive(true);
            _finishLevelButton.gameObject.SetActive(true);
        });

        Timer.Instance.StartTimer();
        _gameplayManager.CurrentLevel.Unfreeze();
    }

    public void UpdateUI()
    {
        _movesText.SetText(_gameplayManager.Moves.ToString());
        _levelText.SetText(_gameplayManager.LevelIndex.ToString());

        _haveBombs = PlayerPrefsManager.Instance.BombCount > 0;
        _haveReloads = PlayerPrefsManager.Instance.ReloadsCount > 0;

        _bombsCircleContainer.SetActive(_haveBombs);
        _bombsCountText.SetText(PlayerPrefsManager.Instance.BombCount.ToString());

        _reloadsCircleContainer.SetActive(_haveReloads);
        _reloadsCountText.SetText(PlayerPrefsManager.Instance.ReloadsCount.ToString());
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
