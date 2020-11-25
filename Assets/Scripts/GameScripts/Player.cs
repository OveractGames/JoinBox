using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction onPlayerHitTarget;
    public event UnityAction onLevelFailing;

    public bool grounded = false;
    public bool targetHit = false;

    public float minDistance = 1f;
    public float difference;
    public float velocity;
    public Transform target;
    private RectTransform rTransform;

    private bool failing = false;
    private Rigidbody2D playerRb;

    public float delay = 1.0f;
    private void Start()
    {
        if (playerRb == null)
            playerRb = GetComponent<Rigidbody2D>();
        if (target == null)
            target = GameObject.Find("target").transform;
        if (playerRb == null)
            Debug.LogError("The object does not contain the component" + typeof(Rigidbody2D));
        rTransform = GetComponent<RectTransform>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "target")
        {
            Debug.Log("Level done");
            if (onPlayerHitTarget != null)
                onPlayerHitTarget.Invoke();
            targetHit = true;
        }
        if (collision.gameObject.tag == "ground")
            grounded = true;
    }

    public void Update()
    {
        if (target == null)
            target = GameObject.Find("target").transform;
        if (failing || targetHit)
            return;
        if (grounded)
        {
            velocity = playerRb.velocity.x;
            if (playerRb.velocity.x == 0)
            {
                delay -= Time.deltaTime;
                if (delay <= 0)
                {
                    failing = true;
                    if (onLevelFailing != null)
                        onLevelFailing.Invoke();
                }
            }
        }
        if(rTransform.anchoredPosition.y <= -450f)
        {
            failing = true;
            if (onLevelFailing != null)
                onLevelFailing.Invoke();
        }
    }
}
