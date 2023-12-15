using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelBuildManager))]
public class LevelSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelBuildManager levelBuildManager = (LevelBuildManager)target;

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

        GUILayout.Label("Horizontal blocks", EditorStyles.boldLabel);

        if (GUILayout.Button("100x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(100f, 100f);
        }
        if (GUILayout.Button("200x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(200f, 100f);
        }
        if (GUILayout.Button("300x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(300f, 100f);
        }
        if (GUILayout.Button("400x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(400f, 100f);
        }

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

        GUILayout.Label("Vertical blocks", EditorStyles.boldLabel);

        if (GUILayout.Button("200x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(100f, 200f);
        }
        if (GUILayout.Button("300x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(100f, 300f);
        }
        if (GUILayout.Button("400x100"))
        {
            levelBuildManager.InstantiatePrefabWithSize(100f, 400f);
        }
    }
}

