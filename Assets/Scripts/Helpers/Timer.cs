using System;
using TMPro;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    public TMP_Text _timeText;
    private bool _isRunning;
    private float _elapsedSeconds;

    public float ElapsedSeconds { get => _elapsedSeconds; private set => _elapsedSeconds = value; }

    void Update()
    {
        if (!_isRunning) return;

        ElapsedSeconds += Time.deltaTime;
        var timeSpan = TimeSpan.FromSeconds(ElapsedSeconds);

        if (_timeText != null)
        {
            if (timeSpan.Minutes > 60f)
                _timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            else
                _timeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    public void StartTimer()
    {
        _isRunning = true;
    }

    public void ResetTimer()
    {
        ElapsedSeconds = 0;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }
}
