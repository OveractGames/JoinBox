
using ScriptUtils.GameUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public ParticleSystem[] particles;
    public int clicks = 5;
    public AudioClip clickFX;
    List<GameObject> enemies = new List<GameObject>();
    public Player player;
    public event UnityAction OnLevelDone;
    SoundGameManager sManager;
    Randomizer spriteRandom;
    public Sprite[] BlockSprites;
    public int clickCount = 0;
    private void Awake()
    {
        if (transform.GetChild(0).name == "Grid")
            transform.GetChild(0).gameObject.SetActive(false);
    }
    void Start()
    {
        BlockSprites = Resources.LoadAll<Sprite>("Blocks");
        var s = Resources.Load<AudioClip>("squash");
        clickFX = s;
        sManager = FindObjectOfType<SoundGameManager>();
        spriteRandom = new Randomizer(BlockSprites, true);
        particles = Resources.LoadAll<ParticleSystem>("Effects");
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "enemy")
                enemies.Add(child);
            if (child.tag == "Player")
            {
                if (child.GetComponent<Player>() == null)
                    child.AddComponent<Player>();
                player = child.GetComponent<Player>();
                player.onPlayerHitTarget += Player_onPlayerHitTarget;
                player.onLevelFailing += Player_onLevelFailing;
            }
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.AddComponent<ClickAction>();
            enemy.AddComponent<EnemyBox>().SetSprite(BlockSprites[spriteRandom.getRandom()], spriteRandom.randomRule.getLastBlock());
            enemy.GetComponent<ClickAction>().ClickEvent += box_click;
            if (enemy.GetComponent<BoxColliderResizer>() == null)
                enemy.AddComponent<BoxColliderResizer>();
        }
        DataManager.Instance.Init(clicks);
    }

    private void Player_onLevelFailing()
    {
        DataManager.Instance.failingModal.TurnOn();
    }

    private void box_click(GameObject target)
    {
        if (DataManager.Instance.points >= 1)
        {
            clickCount++;
            target.SetActive(false);
            ParticleSystem effect = Instantiate(particles[target.GetComponent<EnemyBox>().effectIndex],
                new Vector3(target.transform.position.x, target.transform.position.y, -1.5f), Quaternion.identity) as ParticleSystem;
            Destroy(effect.gameObject, 1.5f);
            DataManager.Instance.takePoints();
        }
        else
            DataManager.Instance.leanPanel.TurnOn();
        var status = PlayerPrefs.GetInt("SFX");
        if (status == 1)
            sManager.PlaySound(clickFX);
    }
    private void Player_onPlayerHitTarget()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.GetComponent<ClickAction>().levelDone = true;
        }
        if (OnLevelDone != null)
            OnLevelDone.Invoke();
    }
    private void Update()
    {
        if (DataManager.Instance.failingModal.On)
            DisableInput();
    }


    private void DisableInput()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                ClickAction action = enemy.GetComponent<ClickAction>();
                action.interactable = false;
            }             
        }
    }
}
