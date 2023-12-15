using System;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _root;

    public event Action OnClose;
    public event Action OnShow;

    public virtual void Show()
    {
        _root.gameObject.SetActive(true);
        OnShow?.Invoke();
    }

    public virtual void Hide()
    {
        _root.gameObject.SetActive(false);
        OnClose?.Invoke();
    }
}
