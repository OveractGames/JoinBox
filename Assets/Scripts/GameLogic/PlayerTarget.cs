using System;
using UnityEngine;

public class PlayerTarget : GameplayElement
{
    public event Action OnTargetFound;

    public override event Action<GameObject> OnFall;

    private Rigidbody2D _rb;
    private Rigidbody2D _targetRb;
    private bool _isFrozen = false;

    private const float MIN_Y = -10f;

    public void Init(Transform targetInstance)
    {
        _rb = GetComponent<Rigidbody2D>();
        _targetRb = targetInstance.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isFrozen) { return; }

        if (collision.gameObject.CompareTag("target"))
        {
            _isFrozen = true;
            Debug.Log("Target found!");
            Freeze();
            OnTargetFound?.Invoke();
        }
    }

    public override void Freeze()
    {
        _isFrozen = true;
        _rb.bodyType = RigidbodyType2D.Static;

        if (_targetRb != null)
        {
            _targetRb.bodyType = RigidbodyType2D.Static;
        }
    }

    public override void Fall(GameObject target)
    {
        Freeze();
        OnFall?.Invoke(target);
    }

    private void Update()
    {
        if (_isFrozen) { return; }
        if (transform.position.y < MIN_Y)
        {
            Fall(gameObject);
        }
        else if (_targetRb != null)
        {
            if (_targetRb.position.y < MIN_Y)
            {
                Fall(_targetRb.gameObject);
            }
        }
    }

    public override void Unfreeze()
    {
        _isFrozen = false;
        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        _rb.bodyType = RigidbodyType2D.Dynamic;
        if (_targetRb != null)
        {
            _targetRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
