using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Transform levelContainer;

    [SerializeField] private GameObject destroyEffect;

    private Level currentLevel;
    private int moves;

    public Level CurrentLevel { get => currentLevel; private set => currentLevel = value; }
    public int Moves { get => moves; private set => moves = value; }

    public event Action OnBoxClick;
    public event Action OnCreate;
    public event Action OnLevelComplete;
    public event Action DispatchNoMovesEvent;

    private Coroutine _transitionCoroutine;
    public void StartNextLevel()
    {
        if (CurrentLevel != null)
        {
            UnsubscribeFromEvents(CurrentLevel);
            Destroy(CurrentLevel.gameObject);
        }
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }
        Level level = Resources.Load<Level>($"Levels/Level{PlayerPrefsManager.Instance.LevelIndex}");
        if (level == null)
        {
            Debug.Log($"Cannont find level{PlayerPrefsManager.Instance.LevelIndex}");
            return;
        }
        CurrentLevel = Instantiate(level, levelContainer);
        SetUpLevel(CurrentLevel);
    }

    private void SetUpLevel(Level level)
    {
        level.LevelFall += StartNextLevel;
        level.LevelComplete += () => ShowWinScreen(level);
        level.OnBlockDestroyEvent += BlockDestroy;

        RectTransform rectTransform = level.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(1000f, 125f);
        rectTransform.DOAnchorPosX(8f, 1f).SetEase(Ease.Linear);

        Moves = level.Moves;
        OnCreate?.Invoke();
    }

    private void UnsubscribeFromEvents(Level level)
    {
        level.LevelFall -= StartNextLevel;
        level.LevelComplete -= () => ShowWinScreen(level);
        level.OnBlockDestroyEvent -= BlockDestroy;
    }

    public void DestroyCurrentLevelAndMoveToNext()
    {
        CurrentLevel.DestroyAll();
        ShowWinScreen(CurrentLevel);
    }

    private void ShowWinScreen(Level level)
    {
        OnLevelComplete?.Invoke();
       _transitionCoroutine =  StartCoroutine(TransitionAndDestroy(level));
    }

    private IEnumerator TransitionAndDestroy(Level level)
    {   
        yield return new WaitForSeconds(1f);
        RectTransform rectTransform = level.GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(-1000f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(level.gameObject);
        });
        yield return null;
    }

    private void BlockDestroy(Transform targetBlock)
    {
        if (Moves <= 0)
        {
            DispatchNoMovesEvent?.Invoke();
            return;
        }

        AudioManager.Instance.Play(SoundType.CLICK);
        Instantiate(destroyEffect, targetBlock.position, transform.rotation);
        Moves--;
        Moves = Mathf.Clamp(Moves, 0, CurrentLevel.Moves);
        OnBoxClick?.Invoke();
        Destroy(targetBlock.gameObject);
        Debug.Log("Destroyed " + targetBlock.name);
        GameObject effect = Instantiate(destroyEffect, targetBlock.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}

public enum BlockType
{
    PLAYER = 0,
    TARGET = 1,
    BLOCK = 2
}
