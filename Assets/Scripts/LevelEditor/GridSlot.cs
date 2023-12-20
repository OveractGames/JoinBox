using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler
{
    public RectTransform rectTransform1;
    public RectTransform rectTransform2;

    private void Awake()
    {
        rectTransform1 = GetComponent<RectTransform>();
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
            Vector3 pos = new Vector3(rectTransform1.anchoredPosition.x + offsetX, rectTransform1.anchoredPosition.y + offsetY, 0f);
            rectTransform.anchoredPosition = pos;
            //rectTransform.SetParent(rectTransform1);
            target.parent = rectTransform1;
        }
    }

    private bool CheckRectOverlap(RectTransform rectTransform1, RectTransform rectTransform2)
    {
        Rect rect1 = GetScreenRect(rectTransform1);
        Rect rect2 = GetScreenRect(rectTransform2);

        return rect1.Overlaps(rect2);
    }

    private Rect GetScreenRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Rect rect = new Rect(corners[0], corners[2] - corners[0]);
        return rect;
    }
}