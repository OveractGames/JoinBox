
using Lean.Gui;
using System;
using TMPro;
using UnityEngine;

public class UIShopScreen : UIScreen
{
    [SerializeField] private CoinsPackButton[] _coinsPackButtons;

    [SerializeField] private TMP_Text _bombCountText;
    [SerializeField] private TMP_Text _reloadsCountText;
#if HOME
    [SerializeField] private RewardedAdsButton _rewardedAdsButton_1;
    [SerializeField] private RewardedAdsButton _rewardedAdsButton_2;
#endif

    [SerializeField] private LeanButton _buyMeCoffeeButton;
    [SerializeField] private LeanButton _closeButton;

    private void Start()
    {
#if HOME
        _rewardedAdsButton_1.OnComplete += Show;
        _rewardedAdsButton_2.OnComplete += Show;
#endif
        _buyMeCoffeeButton.OnClick.AddListener(BuyMeCoffe);
        _closeButton.OnClick.AddListener(CloseShop);
        foreach (CoinsPackButton coinsButton in _coinsPackButtons)
        {
            coinsButton.OnPackClick += BuyPack;
        }
    }

    private void CloseShop()
    {
        UIController.Instance.ShowScreen<UIMainScreen>();
    }

    private void BuyMeCoffe()
    {
        //to do
    }

    private void BuyPack(int index, int coins)
    {
        // IAPManager.Instance.BuyProduct(index);
        // IAPManager.Instance.OnPurchaseComplete += PackPurchaseCompleted;
    }

    private void PackPurchaseCompleted()
    {
        //to do
        //add bought coins to data
    }

    public override void Show()
    {
        base.Show();
        #if HOME
        _rewardedAdsButton_1.LoadAd();
        _rewardedAdsButton_2.LoadAd();
#endif
        _bombCountText.SetText("x" + PlayerPrefsManager.Instance.BombCount.ToString());
        _reloadsCountText.SetText("x" + PlayerPrefsManager.Instance.ReloadsCount.ToString());
    }

    public override void Hide()
    {
        base.Hide();
    }
}