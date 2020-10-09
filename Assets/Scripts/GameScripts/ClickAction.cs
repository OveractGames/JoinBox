using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    public bool levelDone = false;
    public bool interactable = true;
    public event UnityAction<GameObject> ClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelDone || !interactable)
            return;
        if (ClickEvent != null)
            ClickEvent.Invoke(this.gameObject);
    }
}
