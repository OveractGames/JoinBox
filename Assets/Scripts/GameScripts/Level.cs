
using ScriptUtils.GameUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public int clicks = 5;
    public AudioClip clickFX;
    List<GameObject> enemies = new List<GameObject>();
    public Player player;
    public event UnityAction OnLevelDone;

    SoundGameManager sManager;
    Randomizer spriteRandom;

    public Sprite[] BlockSprites;
    void Start()
    {
        BlockSprites = Resources.LoadAll<Sprite>("Blocks");
        var s = Resources.Load<AudioClip>("squash");
        clickFX = s;
        sManager = FindObjectOfType<SoundGameManager>();
        spriteRandom = new Randomizer(BlockSprites, true);
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "enemy")
                enemies.Add(child);
            if (child.tag == "Player")
            {
                child.AddComponent<Player>();
                player = child.GetComponent<Player>();
                player.onPlayerHitTarget += Player_onPlayerHitTarget;
            }
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.AddComponent<ClickAction>();
            enemy.AddComponent<EnemyBox>().SetSprite(BlockSprites[spriteRandom.getRandom()]);
            enemy.GetComponent<ClickAction>().ClickEvent += box_click;
            if (enemy.GetComponent<BoxColliderResizer>() == null)
                enemy.AddComponent<BoxColliderResizer>();
        }
        DataManager.Instance.clickText.SetText("x" + clicks.ToString());
        DataManager.Instance.helpPanel.gameObject.SetActive(true);
        DataManager.Instance.helpPanel.TurnOff();
    }

    private void box_click()
    {
        if (clicks - 1 >= 0)
        {
            clicks--;
            DataManager.Instance.clickText.SetText("x" + clicks.ToString());
            if (clicks == 0)
            {
                foreach (GameObject enemy in enemies)
                    enemy.GetComponent<ClickAction>().interactable = false;
            }
        }
        var status = PlayerPrefs.GetInt("SFX");
        if (status == 1)
            sManager.PlaySound(clickFX);
    }
    public void EnableMouseEvents()
    {
        clicks++;
        DataManager.Instance.clickText.SetText("x" + clicks.ToString());
        foreach (GameObject enemy in enemies)
            enemy.GetComponent<ClickAction>().interactable = true;
    }
    private void Player_onPlayerHitTarget()
    {
        DataManager.Instance.helpPanel.gameObject.SetActive(false);
        foreach (GameObject enemy in enemies)
            enemy.GetComponent<ClickAction>().levelDone = true;
        if (OnLevelDone != null)
            OnLevelDone.Invoke();
    }
}
