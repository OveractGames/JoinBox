using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private Canvas canvas;

    private CanvasGroup canvasGroup;

    private RectTransform rectTransform;

    public event Action<ObjectDragDrop> OnBoxClick;

    public Transform parent;

    public enum BoxType
    {
        T100 = 0,
        T200= 1,
        T300= 2,
        T400= 3,
        t200_vert = 4,
        t_300_vert = 5,
        t_400_vert = 6,
        t_100_indistructible = 7,
        t_200_indistructible = 8
    }

    public BoxType type;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(gameObject);
        }
        else
        {
            OnBoxClick?.Invoke(this);
        }
    }

    public void SetType(int type, Sprite star)
    {
        if(type == 0)
        {
            gameObject.tag = "target";
            //set as target
        }else
        {
            gameObject.tag = "Player";
            gameObject.AddComponent<PlayerTarget>();
            //set as player
        }
        foreach(Transform t in transform)
        {
            if (t.name.StartsWith("star"))
            {
                return;
            }
        }
        GameObject go = new GameObject("star");
        go.transform.parent = transform;
        RectTransform rc = go.AddComponent<RectTransform>();
        rc.sizeDelta = new Vector2(50f, 50f);
        go.AddComponent<Image>().sprite = star;
        rc.localScale = Vector3.one;
        rc.anchoredPosition = Vector3.zero;
    }
}
