using System;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField] private int moves;

    public bool IsComplete { get; private set; }

    public int Moves { get => 20; private set => moves = value; }

    public event Action<Transform> OnBlockDestroyEvent;
    public event Action LevelComplete;
    public event Action LevelFall;

    private void Awake()
    {
        blocks = GetComponentsInChildren<DestructibleBlock>();
        player = GetComponentInChildren<PlayerTarget>();
        target = GameObject.FindGameObjectWithTag("target");
        Moves = 20;//testing only
        foreach (Transform t in transform)
        {
            if (t.name.StartsWith("Grid") || t.name.StartsWith("grid"))
            {
                t.gameObject.SetActive(false);
            }
        }
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
        FreezeAll();
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
        IsComplete = true;
        LevelComplete?.Invoke();
    }

    public void DestroyAll()
    {
        player.OnTargetFound -= OnLevelDone;
        foreach (DestructibleBlock block in blocks)
        {
            if (block)
            {
                block.OnBlockClickEvent -= OnBlockClick;
                Destroy(block.gameObject);
            }
        }
    }

    private void OnBlockClick(GameObject block)
    {
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
        player.Freeze();
    }

    public void UnfreezeLevel()
    {
        foreach (DestructibleBlock block in blocks)
        {
            if (block != null)
            {
                block.Unfreeze();
            }
        }
        player.Unfreeze();
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
        //Destroy(target);
    }

    public void OnLevelFall()
    {
        LevelFall?.Invoke();
    }
}
