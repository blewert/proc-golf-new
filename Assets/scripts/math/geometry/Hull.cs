using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Hull
{
    public List<Vector3> points = new List<Vector3>();

    public Hull(List<Vector3> pointsA, List<Vector3> pointsB)
    {
        //Union the two together -- not the best approach but its 2am
        List<Vector3> allPoints = pointsA.Union(pointsB).ToList();

        //Compute the hull and save it
        points = ConvexHull.Compute(allPoints, true);
    }

    public void Add(Vector3 point)
    {
        this.points.Add(point);
    }
}