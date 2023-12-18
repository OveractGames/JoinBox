using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelsScreen : UIScreen
{
    [SerializeField] private LeanButton _close;

    [SerializeField] private Transform _levelsTabsContainer;

    [SerializeField] private int _levelCount;

    [SerializeField] private LevelTab _levelTab;

    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private LevelTab[] _tabs;
    private void Start()
    {
        _levelCount = Resources.LoadAll("Levels").Length - 1;
        _close.OnClick.AddListener(() =>
        {
            UIController.Instance.ShowScreen<UIMainScreen>();
        });
        SpawnLevelTabs();
    }

    private void SpawnLevelTabs()
    {
        _tabs = new LevelTab[_levelCount];
        for (int i = 0; i < _levelCount; i++)
        {
            LevelTab levelTab = Instantiate(_levelTab, _levelsTabsContainer);
            levelTab.gameObject.SetActive(true);
            levelTab.OnTabClick += TabClick;
            levelTab.Setup(i + 1, _gameplayManager);
            _tabs[i] = levelTab;
        }
    }

    private void TabClick()
    {
        UIController.Instance.ShowScreen<UIGameplayScreen>();
        Hide();
    }

    public override void Show()
    {
        base.Show();
        foreach (var tab in _tabs)
        {
            tab.Show();
        }
    }

    public override void Hide()
    {
        base.Hide();
    }
}
