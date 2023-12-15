using DG.Tweening;
using Lean.Gui;
using System;
using UnityEngine;

public class UIDailySpinScreen : UIScreen
{
    [SerializeField] private LeanButton _closeButton;

    [SerializeField] private Transform wheel;

    [SerializeField] private float spinTime;

    [SerializeField] private RewardPanel _rewardPanel;

    [SerializeField]
    private SpinReward[] SpinRewards =
        {
        new SpinReward(0, 750),
        new SpinReward(36, 500),
        new SpinReward(72, 250),
        new SpinReward(108, 1000),
        new SpinReward(144, 750),
        new SpinReward(180, 250),
        new SpinReward(216, 250),
        new SpinReward(252, 250),
        new SpinReward(288, 500),
        new SpinReward(324, 500)
    };

    [SerializeField] private LeanButton _spinButton;

    public event Action OnSpinComplete;

    void Start()
    {
        _spinButton.OnClick.AddListener(Spin);
        _closeButton.OnClick.AddListener(() => OnSpinComplete?.Invoke());
        _rewardPanel.OnNext += () => OnSpinComplete?.Invoke();
    }

    public void Spin()
    {
        int numRotations = UnityEngine.Random.Range(5, 10);
        int randomIndex = UnityEngine.Random.Range(0, SpinRewards.Length);
        int randomRotation = SpinRewards[randomIndex].Rotation;
        int totalRotation = numRotations * 360 + randomRotation;

        wheel.DORotate(new Vector3(0f, 0f, totalRotation), spinTime, RotateMode.FastBeyond360)
            .OnComplete(() =>
            {
                _rewardPanel.gameObject.SetActive(true);
                _rewardPanel.Show(SpinRewards[randomIndex].Reward);
            });
        _closeButton.gameObject.SetActive(false);
        _spinButton.gameObject.SetActive(false);
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

[Serializable]
public class SpinReward
{
    public int Rotation;
    public int Reward;

    public SpinReward(int rotation, int reward)
    {
        Rotation = rotation;
        Reward = reward;
    }
}
