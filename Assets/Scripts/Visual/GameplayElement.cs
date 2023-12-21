using System;
using UnityEngine;

public abstract class GameplayElement : MonoBehaviour
{
    public abstract event Action<GameObject> OnFall;

    public abstract void Fall(GameObject target);

    public abstract void Freeze();

    public abstract void Unfreeze();
}
