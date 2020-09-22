using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickText : MonoBehaviour
{
    TextMeshProUGUI txt;
    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    public void SetText(string text)
    {
        txt.SetText(text);
    }
}
