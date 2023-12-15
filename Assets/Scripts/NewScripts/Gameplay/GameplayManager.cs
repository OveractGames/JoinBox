using System;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Transform _levelContainer;
    [SerializeField] private Level[] _levels;
    [SerializeField] private GameObject _destroyEffect;

    private Level _currentLevel;
    private int _levelIndex = 0;

    public Level CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

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
        CurrentLevel.OnBlockDestroyEvent += BlockDestroy;
        CurrentLevel.LevelFall += StartNextLevel;
        CurrentLevel.LevelComplete += LevelComplete;
    }

    private void LevelComplete()
    {
        Debug.Log("Level complete!");
        _levelIndex++;
        StartNextLevel();
    }

    private void BlockDestroy(Transform targetBlock)
    {
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
