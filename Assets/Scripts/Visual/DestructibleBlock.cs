using System;
using UnityEngine;

public class DestructibleBlock : GameplayElement
{
    public bool IsDestructible = true;

    public event Action<GameObject> OnBlockClickEvent;

    public override event Action<GameObject> OnFall;

    public bool IsBlockFrozen = false;

    private void Start()
    {
        if(GetComponent<ObjectDragDrop>() != null)
        {
            return;
        }
        gameObject.AddComponent<MouseEventSystem>().MouseEvent += OnBlockClick;
    }

    private void OnBlockClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK && !IsBlockFrozen)
        {
            if (IsDestructible)
            {
                OnBlockClickEvent?.Invoke(this.gameObject);
            }
        }
    }

    public override void Freeze()
    {
        IsBlockFrozen = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    public override void Fall(GameObject target)
    {
        OnFall?.Invoke(gameObject);
    }
}
