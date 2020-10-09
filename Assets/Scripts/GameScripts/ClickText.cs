using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickText : Singleton<ClickText>
{
    TextMeshProUGUI txt;
    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    public void SetText(int count)
    {
        txt.SetText(count.ToString());
    }
}
