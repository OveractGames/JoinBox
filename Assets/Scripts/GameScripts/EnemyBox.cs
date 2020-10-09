
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBox : MonoBehaviour
{
    Image enemyImage;
    public int effectIndex;
    RectTransform enemyRect;
    private void Start()
    {
        enemyRect = GetComponent<RectTransform>();
    }
    public void SetSprite(Sprite sprite, int effectIndex)
    {
        this.effectIndex = effectIndex;
        enemyImage = GetComponent<Image>();
        enemyImage.sprite = sprite;
        enemyImage.color = Color.white;
        enemyImage.type = Image.Type.Tiled;
    }
    private void Update()
    {
        if (enemyRect.anchoredPosition.y <= -720f)
            Destroy(gameObject);
    }
}
