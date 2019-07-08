using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FairwayOptions
{
    public float minRadius = 1f;
    public float maxRadius = 5f;

    public int pointCount = 3;

    public float distance = 1f;    

    public float angleRange = 10f;

    public int circleFidelity = 16;

    public float holeScaleFactor = 1.5f;

    [Range(0f, 2f)]
    public float holeRadius = 0.6f;
    public int holeSampleCount = 15;

    [Range(0f, 1f)]
    public float offsetRadius = 0.0f;
}
