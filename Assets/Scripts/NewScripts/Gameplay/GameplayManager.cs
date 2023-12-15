using DG.Tweening;
using System;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Transform _levelContainer;

    [SerializeField] private Level[] _levels;

    [SerializeField] private GameObject _destroyEffect;

    private Level _currentLevel;

    private int _levelIndex = 0;

    private int _moves;

    public event Action OnBoxClick;
    public Level CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
    public int Moves { get => _moves; private set => _moves = value; }

    public event Action OnCreate;
    public event Action DispatchNoMovesEvent;

    private Level OldLevel;

    public void StartNextLevel()
    {
        if (CurrentLevel != null)
        {
            CurrentLevel.OnBlockDestroyEvent -= BlockDestroy;
            CurrentLevel.LevelComplete -= LevelComplete;
            CurrentLevel.LevelFall -= StartNextLevel;
            Destroy(CurrentLevel.gameObject);
        }
        if (_levelIndex >= _levels.Length)
        {
            Debug.Log("All levels completed!");
            return;
        }
        CurrentLevel = Instantiate(_levels[_levelIndex], _levelContainer);
        CurrentLevel.LevelFall += StartNextLevel;
        CurrentLevel.LevelComplete += LevelComplete;
        CurrentLevel.OnBlockDestroyEvent += BlockDestroy;
        RectTransform rectTransform = CurrentLevel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(1000f, 125f);
        CurrentLevel.GetComponent<RectTransform>().DOAnchorPosX(8f, 1f).SetEase(Ease.Linear);
        _moves = CurrentLevel.Moves;
        OnCreate?.Invoke();

    }

    private void LevelComplete()
    {
        Debug.Log("Level complete!");
        _levelIndex++;
        OldLevel = CurrentLevel;
        CurrentLevel = null;
        OldLevel.GetComponent<RectTransform>().DOAnchorPosX(-1000f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(OldLevel.gameObject);
            OldLevel = null;
        });
        StartNextLevel();
    }


    private void BlockDestroy(Transform targetBlock)
    {
        if (Moves <= 0)
        {
            DispatchNoMovesEvent?.Invoke();
            return;
        }
        Moves--;
        Moves = Mathf.Clamp(Moves, 0, CurrentLevel.Moves);
        OnBoxClick?.Invoke();
        Destroy(targetBlock.gameObject);
        Debug.Log("Destroyed " + targetBlock.name);
        GameObject effect = Instantiate(_destroyEffect, targetBlock.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}

public enum BlockType
{
    PLAYER = 0,
    TARGET = 1,
    BLOCK = 2
}
