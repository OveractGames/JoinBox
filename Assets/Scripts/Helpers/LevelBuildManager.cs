using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class LevelBuildManager : MonoBehaviour
{
    [SerializeField] private GameObject _levelTransform;
    [SerializeField] private GameObject _prefabToInstantiate;

    public void InstantiatePrefabWithSize(float width, float height)
    {
        if (_prefabToInstantiate != null)
        {
            GameObject instantiatedPrefab = Instantiate(_prefabToInstantiate, _levelTransform.transform.position, Quaternion.identity);
            instantiatedPrefab.transform.localScale = new Vector3(width, height, 1f);
            // You might want to set other properties of the instantiated prefab here
        }
    }
}
