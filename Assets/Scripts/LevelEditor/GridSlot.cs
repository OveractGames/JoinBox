using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
{
    public RectTransform rectTransform;

    public event Action<GridSlot> OnClick;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        ObjectDragDrop target = eventData.pointerDrag.GetComponent<ObjectDragDrop>();
        if (target == null) return;

        float offsetX = 0f;
        float offsetY = 0f;

        switch (target.type)
        {
            case ObjectDragDrop.BoxType.t200_vert:
            case ObjectDragDrop.BoxType.t_400_vert:
                offsetY = 50f;
                break;

            case ObjectDragDrop.BoxType.T200:
            case ObjectDragDrop.BoxType.T400:
                offsetX = 50f;
                break;

            case ObjectDragDrop.BoxType.t_200_indistructible:
                offsetX = 50f;
                offsetY = 50f;
                break;
        }

        RectTransform rectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector3 pos = new Vector3(this.rectTransform.anchoredPosition.x + offsetX, this.rectTransform.anchoredPosition.y + offsetY, 0f);
            rectTransform.anchoredPosition = pos;
            //rectTransform.SetParent(rectTransform1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}