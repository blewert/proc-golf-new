using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSmoothHull : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    public Transform centre;
    public Hull hull;
    public List<Vector3> smoothHull;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 25; i++)    
            points.Add(centre.position + MathfEx.Polar2CartXZ(Random.value * 360, Random.value * 2f, Quaternion.identity));

        hull = new Hull(points);
        // smoothHull = MathfEx.smoothPolygon(hull.points);
    }

    public void OnDrawGizmos()
    {
        if(points == null)
            return;

        foreach(var point in points)
            Gizmos.DrawCube(point, Vector3.one * 0.1f);

        GizmosEx.DrawPolygon(hull.points);
    }
}
