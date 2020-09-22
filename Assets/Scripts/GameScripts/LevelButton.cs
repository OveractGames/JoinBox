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
    public int currentIndex;
    public int startIndex;
    public int endIndex;
    public int lastLevel;
    public string rangeText;
    public bool isFinished = false;
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
        currentIndex = 0;
        isFinished = DataManager.Instance.currentLevelIndex >= lastLevel;
        for (int i = rangeText == "Welcome" ? 0 : lastChapter.endIndex; i <= lastLevel; i++)
        {
            if (DataManager.Instance.currentLevelIndex >= i+1)
                currentIndex++;
        }
        buttonDiscriptionText.SetText(rangeText + "\n" + currentIndex + "/" + endIndex);
        //buttonDiscriptionText.SetText(isFinished ? rangeText + "\n" + "<size=-14>COMPLETED" : rangeText + "\n" + endIndex + "<size=-14> levels to complete!");
        arrowSequence.setCurrentChildIndex(isFinished ? 1 : 0);
        if (rangeText == "Welcome")
        {
            if (!isFinished)
                lockSequence.setCurrentChildIndex(2);
            else
                lockSequence.setCurrentChildIndex(1);
        }
        else
        {
            if (lastChapter.isFinished)
            {
                if (!isFinished)
                    lockSequence.setCurrentChildIndex(2);
                else
                    lockSequence.setCurrentChildIndex(1);
            }
            else
                lockSequence.setCurrentChildIndex(0);
        }
        if (lockSequence.CurrentChildIndex == 2)
            GetComponent<LeanButton>().OnClick.AddListener(DoClose);
    }
}
