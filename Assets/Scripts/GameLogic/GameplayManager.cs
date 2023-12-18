using DG.Tweening;
using ScriptUtils.Interface;
using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Transform levelContainer;

    [SerializeField] private GameObject destroyEffect;

    private Level currentLevel;
    private int moves;

    public Level CurrentLevel { get => currentLevel; private set => currentLevel = value; }
    public int Moves { get => moves; private set => moves = value; }
    public bool IsOneLevelGame { get => _isOneLevelGame; private set => _isOneLevelGame = value; }
    public int LevelIndex { get => _levelIndex; private set => _levelIndex = value; }

    public event Action OnBoxClick;
    public event Action OnCreate;
    public event Action OnLevelComplete;
    public event Action<string, RewardType> DispatchNoMovesEvent;

    private Coroutine _transitionCoroutine;

    private bool _isOneLevelGame = false;

    private int _levelIndex;

    public void PlayLevel(int levelIndex)
    {
        IsOneLevelGame = true;
        if (CurrentLevel != null)
        {
            UnsubscribeFromEvents(CurrentLevel);
            Destroy(CurrentLevel.gameObject);
        }
        SpawnLevel(levelIndex);
        SetUpLevel(CurrentLevel);
    }

    private void SpawnLevel(int index)
    {
        _levelIndex = index;
        Level level = Resources.Load<Level>($"Levels/Level{index}");
        if (level == null)
        {
            Debug.Log($"Cannont find level{index}");
            return;
        }
        CurrentLevel = Instantiate(level, levelContainer);
    }

    public void StartNextLevel()
    {
        Timer.Instance.ResetTimer();
        Timer.Instance.StartTimer();
        if(IsOneLevelGame)
        {
            //Navigator.getInstance().LoadLevel("Game");
            return;
        }
        IsOneLevelGame = false;
        if (CurrentLevel != null)
        {
            UnsubscribeFromEvents(CurrentLevel);
            Destroy(CurrentLevel.gameObject);
        }
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }
        SpawnLevel(PlayerPrefsManager.Instance.LevelIndex);
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
        _transitionCoroutine = StartCoroutine(TransitionAndDestroy(level));
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
            string txt = $"Watch a short video to get +2 moves.";
            DispatchNoMovesEvent?.Invoke(txt, RewardType.MOVES);
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
