using UnityEngine;
using DG.Tweening;
using Lean.Gui;

public class UIGameplayScreen : UIScreen
{
    [SerializeField] private RectTransform _topMenu;

    [SerializeField] private LeanButton _pauseButton;
    [SerializeField] private LeanButton _reloadButton;

    [SerializeField] private GameplayManager _gameplayManager;

    public bool _testing = false;


    private void Start()
    {
        _pauseButton.OnClick.AddListener(PauseGame);
        _reloadButton.OnClick.AddListener(Reload);
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

    public void ContinueGame()
    {
        //if (SessionState.GetInt("START", 0) == 0 || _testing)
        //{
        //    GameplayManager.Instance.StartNextLevel();
        //    _testing = false;
        //    SessionState.SetInt("START", 1);
        //}
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

    public override void Show()
    {
        ContinueGame();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
