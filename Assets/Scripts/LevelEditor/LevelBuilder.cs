using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Level level;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private GameObject tilesPanel;
    [SerializeField] private Button tileOpenButton;
    [SerializeField] private Sprite star;
    [SerializeField] private GameObject Panel;
    [SerializeField] private Transform parent;
    [SerializeField] private RectTransform SpawnPoint;
    [SerializeField] private ObjectDragDrop[] _boxPrefabs;
    [SerializeField] private Button[] _boxSpawnButtons;

    private int levelIndex;
    private void Start()
    {
        for (int i = 0; i < _boxSpawnButtons.Length; i++)
        {
            int k = i;
            _boxSpawnButtons[i].onClick.AddListener(delegate { ButtonClick(k); });
        }
        tileOpenButton.onClick.AddListener(() => tilesPanel.SetActive(true));
        saveButton.onClick.AddListener(Save);
        clearButton.onClick.AddListener(ClearLevel);
    }

    private void ClearLevel()
    {
        DestructibleBlock[] blocks = level.GetComponentsInChildren<DestructibleBlock>();
        foreach (DestructibleBlock block in blocks)
        {
            Destroy(block.gameObject);
        }
    }

    private async void Save()
    {
        if (level != null)
        {
            int lastSavedIndex = PlayerPrefs.GetInt("SAVED_LEVEL", 0);
            lastSavedIndex++;

            ObjectDragDrop[] boxes = GameObject.FindObjectsOfType<ObjectDragDrop>();
            foreach (ObjectDragDrop obj in boxes)
            {
                Destroy(obj.GetComponent<ObjectDragDrop>());
                Destroy(obj.GetComponent<CanvasGroup>());
                obj.transform.SetParent(level.transform);
            }
            await Task.Delay(100);
            Level lvl = Instantiate(level, canvas.transform);
            lvl.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            await Task.Delay(100);
            foreach(Transform t in lvl.transform)
            {
                if (t.name.StartsWith("cell"))
                {
                    Destroy(t.gameObject);
                }
                else
                {
                    if(t.CompareTag("enemy"))
                    {
                        t.gameObject.AddComponent<DestructibleBlock>();
                    }else if(t.CompareTag("Player"))
                    {
                        t.gameObject.AddComponent<PlayerTarget>();
                    }
                }
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(lvl.gameObject, $"Assets/Resources/NewLevels/Level{lastSavedIndex}.prefab");
            if (prefab != null)
            {
                Debug.Log("Prefab saved successfully at: " + AssetDatabase.GetAssetPath(prefab));
                PlayerPrefs.SetInt("SAVED_LEVEL", lastSavedIndex);
                Destroy(lvl.gameObject);
                SceneManager.LoadScene("LevelBuilder");
            }
            else
            {
                Debug.Log("Prefab creation failed!");
            }
        }
        else
        {
            Debug.LogWarning("Please assign a GameObject to objectToSave!");
        }
    }

    private void ButtonClick(int index)
    {
        ObjectDragDrop obj = Instantiate(_boxPrefabs[index]);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        obj.transform.SetParent(parent, false);
        obj.OnBoxClick += BoxClicked;
        tilesPanel.SetActive(false);
    }

    private void BoxClicked(ObjectDragDrop target)
    {
        if (target.type == ObjectDragDrop.BoxType.T100)
        {
            Panel.SetActive(true);
            Button[] btns = Panel.GetComponentsInChildren<Button>();
            for (int i = 0; i < btns.Length; i++)
            {
                int k = i;
                Button b = btns[i];
                b.onClick.AddListener(() =>
                {
                    target.SetType(k, star);
                    Panel.SetActive(false);
                });
            }
        }
    }
}
