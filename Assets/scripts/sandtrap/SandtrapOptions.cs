using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SandtrapOptions
{
    public int maxSandtrapCount = 5;
    public int minSandtrapCount = 1;

    [Range(0f, 1f)]
    public float minRadiusDist = 0.8f;

    public float minRadius = 0.5f;
    public float maxRadius = 1f;

    public int hullPoints = 10;

    [Range(0f, 1f)]
    public float sinkDepth = 0.05f;
}