using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Singleton<Tutorial>
{
    public GameObject[] blocks;
    public GameObject hand;

    public int index = 0;
    void Start()
    {
        if (hand == null)
            hand = GameObject.FindGameObjectWithTag("hand");
        StartCoroutine(loadBlocks());
    }

    private IEnumerator loadBlocks()
    {
        while (FindObjectOfType<Level>() == null)
            yield return null;
        blocks = GameObject.FindGameObjectsWithTag("enemy");
        index = FindObjectOfType<Level>().clickCount;
        MoveHand(index);
    }

    public void MoveHand(int index)
    {
        this.index = index;
        if (index >= blocks.Length)
            return;
        Vector3 pos = blocks[index].transform.position;
        hand.transform.position = pos;
    }

    public GameObject getCurrentTarget()
    {
        return blocks[index];
    }
}
