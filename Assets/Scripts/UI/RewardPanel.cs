using Lean.Gui;
using System;
using TMPro;
using UnityEngine;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private TMP_Text _rewardDescriptionText;

    [SerializeField] private LeanButton _nextButton;

    public event Action OnNext;

    private void Start()
    {
        _nextButton.OnClick.AddListener(Hide);
    }

    public void Hide()
    {
        OnNext?.Invoke();
        gameObject.SetActive(false);
    }

    public void Show(int reward)
    {
        _rewardText.SetText("+" + reward);
        _rewardDescriptionText.SetText($"Congratulations, you have won {reward} coins today! Don't forget to come back tomorrow for a chance to win even more rewards.");
        SpinRewardManager.Instance.SaveSpinReward(reward);
    }
}
