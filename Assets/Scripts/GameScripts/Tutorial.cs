using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Singleton<Tutorial>
{
    public Transform handPrefab;
    private Transform newHand;
    public void SetPosition(Transform parent)
    {
        newHand = Instantiate(handPrefab, parent.position, Quaternion.identity);
        newHand.transform.SetParent(parent);
        newHand.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        newHand.transform.localPosition = parent.transform.position;
    }
    public void ResetPosition()
    {
        if (newHand != null)
            Destroy(newHand.gameObject);
    }
    public void SetState(bool enabled)
    {
        //gameObject.SetActive(enabled);
    }
}
