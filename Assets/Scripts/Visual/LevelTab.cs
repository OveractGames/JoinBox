using Lean.Gui;
using System;
using TMPro;
using UnityEngine;

public class LevelTab : LeanButton
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _timeText;

    [SerializeField] private GameObject _activeState;
    [SerializeField] private GameObject _inactiveState;

    private GameplayManager _gameplayManager;
    private int _level;

    public event Action OnTabClick;

    private bool _isActive = false;
    private new void Start()
    {
        OnClick.AddListener(PlayLevel);
    }

    private void PlayLevel()
    {
        if (_isActive)
        {
            _gameplayManager.PlayLevel(_level);
            OnTabClick.Invoke();
        }
    }

    public void Show()
    {
        _isActive = PlayerPrefsManager.Instance.GetLevelState(_level) != -1;
        _activeState.SetActive(_isActive);
        _inactiveState.SetActive(!_isActive);
        TimeSpan timeSpan = TimeSpan.FromSeconds(PlayerPrefsManager.Instance.GetLevelTime(_level + 1));
        if (timeSpan.Minutes > 60f)
            _timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        else
            _timeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void Setup(int level, GameplayManager gManager)
    {
        _level = level;
        _gameplayManager = gManager;
        int activeStatus = PlayerPrefsManager.Instance.GetLevelState(_level);
        _activeState.SetActive(activeStatus != -1);
        _inactiveState.SetActive(activeStatus == -1);
        _levelText.SetText(level.ToString());
    }
}
