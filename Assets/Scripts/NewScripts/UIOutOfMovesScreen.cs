using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOutOfMovesScreen : UIScreen
{
    [SerializeField] private Button _adButton;
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(() => Hide());
        _adButton.onClick.AddListener(() =>
        {
            //show ad
        });
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
