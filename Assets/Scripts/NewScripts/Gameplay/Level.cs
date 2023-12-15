using System;
using UnityEngine;

public interface IBlockDestroyListener
{
    void OnBlockDestroy(Transform blockTransform);
}

public interface ILevelCompleteListener
{
    void OnLevelComplete();
}

public interface ILevelFallListener
{
    void OnLevelFall();
}

public class Level : MonoBehaviour, IBlockDestroyListener, ILevelCompleteListener, ILevelFallListener
{
    [SerializeField] private DestructibleBlock[] blocks;
    [SerializeField] private PlayerTarget player;
    [SerializeField] private GameObject target;

    public event Action<Transform> OnBlockDestroyEvent;
    public event Action LevelComplete;
    public event Action LevelFall;

    private void Awake()
    {
        blocks = GetComponentsInChildren<DestructibleBlock>();
        player = GetComponentInChildren<PlayerTarget>();
        target = GameObject.FindGameObjectWithTag("target");
    }

    private void Start()
    {
        foreach (DestructibleBlock block in blocks)
        {
            block.OnBlockClickEvent += OnBlockClick;
            block.OnFall += Fall;
        }
        player.Init(target.transform);
        player.OnFall += Fall;
        player.OnTargetFound += OnLevelDone;
    }

    public void OnBlockDestroy(Transform blockTransform)
    {
        OnBlockDestroyEvent?.Invoke(blockTransform);
    }

    public void OnLevelComplete()
    {
        foreach (DestructibleBlock block in blocks)
        {
            if (block != null)
            {
                block.Freeze();
            }
        }
        LevelComplete?.Invoke();
    }

    private void OnBlockClick(GameObject block)
    {
        Destroy(block);
        OnBlockDestroyEvent?.Invoke(block.transform);
    }

    private void OnLevelDone()
    {
        FreezeAll();
        LevelComplete?.Invoke();
    }

    public void FreezeAll()
    {
        foreach (DestructibleBlock block in blocks)
        {
            if (block != null)
            {
                block.Freeze();
            }
        }
    }

    public void Unfreeze()
    {
        foreach (DestructibleBlock block in blocks)
        {
            if (block != null)
            {
                block.IsBlockFrozen = false;
            }
        }
    }

    private void Fall(GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("target"))
        {
            Debug.Log($"{target.name} falls!");
            foreach (DestructibleBlock block in blocks)
            {
                if (block != null)
                {
                    block.Freeze();
                }
            }
            OnLevelFall();
        }
        Destroy(target);
    }

    public void OnLevelFall()
    {
        LevelFall?.Invoke();
    }
}
