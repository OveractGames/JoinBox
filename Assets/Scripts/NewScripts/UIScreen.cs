using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _root;

    public virtual void Show()
    {
        _root.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _root.gameObject.SetActive(false);
    }
}
