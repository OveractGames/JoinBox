
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : Singleton<AdManager>
{
    public float currentTime;
    public float maxAdDelay;

    public bool showAd = false;
    public bool adInitiated = false;
    private void Update()
    {
        if (adInitiated)
            return;
        currentTime += Time.deltaTime;
        showAd = currentTime >= maxAdDelay;
    }
    public void AdIsShow()
    {
        adInitiated = true;
        currentTime = 0;
    }
    public void ResetManager()
    {
        adInitiated = false;
    }
}
