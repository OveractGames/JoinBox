using System;
using UnityEngine;

public class DestructibleBlock : GameplayElement
{
    public bool IsDestructible = true;

    public event Action<GameObject> OnBlockClickEvent;

    public override event Action<GameObject> OnFall;

    public bool IsBlockFrozen = false;

    public bool IsDynamic = false;

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        UpdateColliderSize(rectTransform, boxCollider2D);
    }
    private void UpdateColliderSize(RectTransform rectTransform, BoxCollider2D boxCollider)
    {
        Vector2 newSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        boxCollider.size = newSize;
    }

    private void Start()
    {
        gameObject.AddComponent<MouseEventSystem>().MouseEvent += OnBlockClick;
    }

    private void Update()
    {
        if(transform.position.y < -15f)
        {
            gameObject.SetActive(false);
        }
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
    public override void Unfreeze()
    {
        IsBlockFrozen = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Fall(GameObject target)
    {
        OnFall?.Invoke(gameObject);
    }
}
