
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
    public List<GameObject> enemies = new List<GameObject>();
    public Player player;
    public event UnityAction OnLevelDone;
    SoundGameManager sManager;
    Randomizer spriteRandom;
    public Sprite[] BlockSprites;
    public int clickCount = 0;
    public bool hasTutorial = false;

    private Vector3 handStartPos;
    private int tutorialIndex = 0;
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
        if (!PlayerPrefs.HasKey("IMUNE"))
        {
            bool hasImune = GameObject.FindGameObjectWithTag("imune");
            if (hasImune)
            {
                PlayerPrefs.SetInt("IMUNE", 1);
                DataManager.Instance.imuneNotificationPanel.SetActive(true);
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
        if (hasTutorial)
        {
            Tutorial.Instance.SetState(true);
            Tutorial.Instance.SetPosition(enemies[0].transform);
        }
        DataManager.Instance.Init(clicks);
    }

    private void Player_onLevelFailing()
    {
        DataManager.Instance.failingModal.TurnOn();
    }

    private void box_click(GameObject target)
    {
        ParticleSystem effect = null;
        if (hasTutorial)
        {
            if (target != enemies[tutorialIndex])
                return;
            if (tutorialIndex < enemies.Count - 1)
                tutorialIndex++;
            Tutorial.Instance.SetPosition(enemies[tutorialIndex].transform);
        }
        if (PlayerPrefs.HasKey("HAS_MOVES"))
        {
            if (DataManager.Instance.points >= 1)
            {
                clickCount++;
                target.SetActive(false);
                effect = Instantiate(particles[target.GetComponent<EnemyBox>().effectIndex],
                    new Vector3(target.transform.position.x, target.transform.position.y, -1.5f), Quaternion.identity) as ParticleSystem;
                Destroy(effect.gameObject, 1.5f);
                DataManager.Instance.takePoints();
            }
            else
                DataManager.Instance.leanPanel.TurnOn();

        }
        else
        {
            target.SetActive(false);
            effect = Instantiate(particles[target.GetComponent<EnemyBox>().effectIndex],
                new Vector3(target.transform.position.x, target.transform.position.y, -1.5f), Quaternion.identity) as ParticleSystem;
            Destroy(effect.gameObject, 1.5f);
        }

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
        if (hasTutorial)
            Tutorial.Instance.ResetPosition();
        else
            Tutorial.Instance.SetState(false);
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
