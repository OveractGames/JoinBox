using Lean.Gui;
using System;
using UnityEngine;

public class CoinsPackButton : LeanButton
{
    [SerializeField] private int _coins;

    [SerializeField] private int _id;

    public event Action<int, int> OnPackClick;

    protected override void Start()
    {
        OnClick.AddListener(() => OnPackClick?.Invoke(_id, _coins));
    }
}
