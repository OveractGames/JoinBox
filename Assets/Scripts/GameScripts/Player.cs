using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction onPlayerHitTarget;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "target")
        {
            Debug.Log("Level done");
            if (onPlayerHitTarget != null)
                onPlayerHitTarget.Invoke();
        }
    }
}
