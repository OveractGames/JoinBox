using Lean.Gui;
using ScriptUtils.Interface;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWinScreen : UIScreen
{
    [SerializeField] private LeanButton _nextButton;

    [SerializeField] private GameObject _fireworks;

    [SerializeField] private GameplayManager _gameplayController;

    private Coroutine _sourceCoroutine;

    private void Start()
    {
        _nextButton.OnClick.AddListener(MoveNext);
        _gameplayController.OnLevelComplete += Show;
    }

    private void MoveNext()
    {
        Hide();
        if (_gameplayController.IsOneLevelGame)
        {
            PlayerPrefs.SetInt("BOOT", 0);
            Navigator.getInstance().LoadLevel("Game");
            return;
        }
        _gameplayController.StartNextLevel();
    }

    public override void Show()
    {
        _nextButton.gameObject.SetActive(false);
        int bombsToAdd;
        int reloadsToAdd = 0;
        if (PlayerPrefsManager.Instance.BombCount >= 2)
        {
            bombsToAdd = UnityEngine.Random.value > 0.85f ? 1 : 0;
        }
        else
        {
            bombsToAdd = UnityEngine.Random.value > 0.5f ? 1 : 0;
        }
        if (PlayerPrefsManager.Instance.ReloadsCount >= 3)
        {
            reloadsToAdd = UnityEngine.Random.value > 0.75f ? 1 : 0;
        }
        else
        {
            reloadsToAdd += UnityEngine.Random.Range(1, 3);
        }
        float elapsedTime = Timer.Instance.ElapsedSeconds;
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        PlayerPrefsManager.Instance.SaveLevelTime(timeSpan);
        
        PlayerPrefsManager.Instance.AddBomb(bombsToAdd);
        PlayerPrefsManager.Instance.AddReloads(reloadsToAdd);

        _gameplayController.CurrentLevel.FreezeAll();
        _fireworks.SetActive(true);
        AudioManager.Instance.Play(SoundType.WIN);
        _sourceCoroutine = StartCoroutine(PlayFireworkSound());
        base.Show();
    }

    private IEnumerator PlayFireworkSound()
    {
        yield return new WaitForSeconds(1f);
        int count = 0;
        while (true)
        {
            AudioManager.Instance.Play(SoundType.FIREWORK);
            yield return new WaitForSeconds(1.5f);
            count++;
            if (count >= 2)
            {
                if (!_nextButton.gameObject.activeSelf)
                {
                    _nextButton.gameObject.SetActive(true);
                }
            }
        }
    }

    public override void Hide()
    {
        if (_sourceCoroutine != null)
        {
            StopCoroutine(_sourceCoroutine);
        }
        _fireworks.SetActive(false);
        base.Hide();
    }
}
