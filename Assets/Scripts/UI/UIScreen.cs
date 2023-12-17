using System;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _root;

    private bool _isShown = false;

    public bool IsActive { get => _isShown; private set => _isShown = value; }

    public event Action OnClose;
    public event Action OnShow;

    public virtual void Show()
    {
        _root.gameObject.SetActive(true);
        OnShow?.Invoke();
        _isShown = true;
    }

    public virtual void Hide()
    {
        _root.gameObject.SetActive(false);
        OnClose?.Invoke();
        _isShown = false;
    }
}
