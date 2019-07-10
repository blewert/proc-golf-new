using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeOptions 
{
    public int numberOfTrees = 100;

    public List<GameObject> treePrefabs;

    public float treeScaleVariance = 0.3f;
    
    public GameObject randomTreePrefab
    {
        get { return treePrefabs[Random.Range(0, treePrefabs.Count)]; }
    }
}