using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelBuilderEditor : EditorWindow
{
    [MenuItem("Window/Level builder editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilderEditor>("Level Builder");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create block 100x100", GUILayout.Height(50f)))
        {
            CreateBlock(new Vector2(100f, 100f));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create block 200x100", GUILayout.Height(50f)))
        {
            CreateBlock(new Vector2(200f, 100f));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create block 300x100", GUILayout.Height(50f)))
        {
            CreateBlock(new Vector2(200f, 100f));
        }
        GUILayout.EndHorizontal();

    }

    private void CreateBlock(Vector2 size)
    {
        GameObject buttonPrefab = Resources.Load<GameObject>("ButtonPrefab");
        if (buttonPrefab != null)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab);
            Selection.activeGameObject = buttonInstance;
        }
        else
        {
            Debug.LogError("Button prefab not found in Resources folder.");
        }
    }
}
