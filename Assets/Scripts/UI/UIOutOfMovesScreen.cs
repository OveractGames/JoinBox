using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType
{
    MOVES = 0,
    RELOADS = 1,
    BOMBS =2
}

public class UIOutOfMovesScreen : UIScreen
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _adButton;
    [SerializeField] private Button _closeButton;

    private RewardType _rewardType;
    private void Start()
    {
        _closeButton.onClick.AddListener(() => Hide());
        _adButton.onClick.AddListener(() =>
        {
            //show ad
        });
    }

    public void Show(string text, RewardType type)
    {
        Show();
        _text.SetText(text);
        _rewardType = type;
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
