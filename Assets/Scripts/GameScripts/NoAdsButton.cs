using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : Singleton<NoAdsButton>
{
    public bool noAds = false;
    private new void Awake()
    {
        noAds = PlayerPrefs.HasKey("NO_ADS");
        gameObject.SetActive(!noAds);
    }
    public void RemoveAds()
    {
        noAds = PlayerPrefs.HasKey("NO_ADS");
        gameObject.SetActive(!noAds);
    }
}
