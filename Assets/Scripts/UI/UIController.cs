using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private Dictionary<Type, UIScreen> screens = new Dictionary<Type, UIScreen>();
    private UIScreen currentScreen;

    public UIScreen CurrentScreen => currentScreen;

    private new void Awake()
    {
        FindAndCacheScreens();
    }

    private void FindAndCacheScreens()
    {
        UIScreen[] foundScreens = GetComponentsInChildren<UIScreen>(true);
        foreach (UIScreen screen in foundScreens)
        {
            Type screenType = screen.GetType();
            if (!screens.ContainsKey(screenType))
            {
                screens.Add(screenType, screen);
                screen.Hide();
            }
            else
            {
                Debug.LogWarning("Duplicate screen type found: " + screenType);
            }
        }
    }

    public UIScreen ShowScreen<T>() where T : UIScreen
    {
        Type screenType = typeof(T);

        if (currentScreen != null && currentScreen.GetType() == screenType)
        {
            return null;
        }

        if (currentScreen != null)
        {
            currentScreen.Hide();
        }

        if (screens.TryGetValue(screenType, out UIScreen newScreen))
        {
            newScreen.Show();
            currentScreen = newScreen;

            return currentScreen;
        }

        Debug.LogError("Screen not found: " + screenType);
        return null;
    }
}
