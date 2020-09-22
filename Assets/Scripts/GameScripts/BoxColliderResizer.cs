using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BoxColliderResizer : MonoBehaviour
{
    BoxCollider2D m_Collider;
    RectTransform m_rect;
    float m_ScaleX, m_ScaleY;
    float offset = 2.5f;
    void Start()
    {
        m_rect = GetComponent<RectTransform>();
        m_Collider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        m_ScaleX = m_rect.rect.width;
        m_ScaleY = m_rect.rect.height;
        m_Collider.size = new Vector2(m_ScaleX - offset, m_ScaleY - offset);
        //Debug.Log("Current BoxCollider Size : " + m_Collider.size);
    }
}
