using Lean.Gui;
using ScriptUtils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public LeanWindow parentWindow;
    public TextMeshProUGUI buttonDiscriptionText;
    public ObjectSequenceSystem lockSequence;
    public ObjectSequenceSystem arrowSequence;

    public int completed;
    public int max;

    public int from;
    public int to;

    public string rangeText;
    public bool chapterFinished = false;
    public LevelButton lastChapter;
    public int index;

    private void DoClose()
    {
        GetComponent<LeanButton>().OnClick.RemoveListener(DoClose);
        LeanWindow window = transform.parent.parent.parent.parent.GetComponent<LeanWindow>();
        window.TurnOff();
    }

    private void Start()
    {
        parentWindow.OnOn.AddListener(Execute);
    }
    public void Execute()
    {
        chapterFinished = isFinished();
        arrowSequence.setCurrentChildIndex(chapterFinished ? 1 : 0);
        if (rangeText == "Welcome")
        {
            if (!chapterFinished)
                lockSequence.setCurrentChildIndex(2);
            else
                lockSequence.setCurrentChildIndex(1);
        }
        else
        {
            if (lastChapter.chapterFinished)
            {
                if (!chapterFinished)
                    lockSequence.setCurrentChildIndex(2);
                else
                    lockSequence.setCurrentChildIndex(1);
            }
            else
                lockSequence.setCurrentChildIndex(0);
        }
        if (lockSequence.CurrentChildIndex == 2)
            GetComponent<LeanButton>().OnClick.AddListener(DoClose);
        calcCurrentIndex();
        buttonDiscriptionText.SetText(!chapterFinished ? rangeText + "\n" + completed + "/" + max : rangeText + "\n" + (completed + 1) + "/" + max + " COMPLETED");
    }

    private int calcCurrentIndex()
    {
        completed = 0;
        int from = this.from;
        int to = this.to;
        for (int i = from; i <= to; i++)
        {
            string key = "level" + i.ToString();
            if (PlayerPrefs.HasKey(key))
                completed++;
        }
        return completed;
    }

    private bool isFinished()
    {
        string key = "level" + to;
        return PlayerPrefs.HasKey(key);
    }
}
