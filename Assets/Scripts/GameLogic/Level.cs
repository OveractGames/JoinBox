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
        foreach(Transform t in transform)
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
        if (target)
        {
            target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
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
        if (target)
        {
            target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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
