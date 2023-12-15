using Lean.Gui;
using System;
using UnityEngine;

public class UIShopScreen : UIScreen
{
    [SerializeField] private CoinsPackButton[] _coinsPackButtons;

    [SerializeField] private LeanButton _watchAdGetCoinsButton;
    [SerializeField] private LeanButton _buyMeCoffeeButton;
    [SerializeField] private LeanButton _closeButton;

    private int _coinsBought = 0;

    private void CloseShop()
    {
        UIController.Instance.ShowScreen<UIMainScreen>();
    }

    private void BuyMeCoffe()
    {
        //to do
    }

    private void WatchAdGetCoins()
    {
        //to do
    }

    private void BuyPack(int index, int coins)
    {
        _coinsBought = coins;
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
        foreach (CoinsPackButton coinsButton in _coinsPackButtons)
        {
            coinsButton.OnPackClick += BuyPack;
        }
        _watchAdGetCoinsButton.OnClick.AddListener(WatchAdGetCoins);
        _buyMeCoffeeButton.OnClick.AddListener(BuyMeCoffe);
        _closeButton.OnClick.AddListener(CloseShop);
    }

    public override void Hide()
    {
        base.Hide();
    }
}
