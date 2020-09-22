
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBox : MonoBehaviour
{
    Image enemyImage;
    public void SetSprite(Sprite sprite)
    {
        enemyImage = GetComponent<Image>();
        enemyImage.sprite = sprite;
        enemyImage.color = Color.white;
        enemyImage.type = Image.Type.Tiled;
    }
}
