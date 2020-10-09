using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptUtils;
[ExecuteInEditMode]
public class Cell : MonoBehaviour
{
    int siblingIndex;
    void Start()
    {
        siblingIndex = transform.GetSiblingIndex();
        GetComponent<ObjectSequenceSystem>().setCurrentChildIndex(siblingIndex % 2 == 0 ? 0 : 1);
    }
}
