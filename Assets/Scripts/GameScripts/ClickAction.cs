using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    public bool interactable = true;
    public bool levelDone = false;
    public event UnityAction ClickEvent; 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelDone)
            return;
        if (!interactable)
        {
            DataManager.Instance.ShowHelpPanel();
            return;
        }
        gameObject.SetActive(false);
        if (ClickEvent != null)
            ClickEvent.Invoke();
    }
}
