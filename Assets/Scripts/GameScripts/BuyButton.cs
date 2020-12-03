using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if HAS_IAP
public class BuyButton : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    private string defaultText;
    void Start()
    {
        defaultText = priceText.text;
        StartCoroutine(LoadPriceRoutine());
    }
    public void ClickBuy()
    {
        IAPManager.Instance.BuyNoAds();
    }

    private IEnumerator LoadPriceRoutine()
    {
        while (!IAPManager.Instance.IsInitialized())
            yield return null;
        string loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.NO_ADS);
        priceText.SetText(defaultText + " " + loadedPrice);
    }
}
#endif
