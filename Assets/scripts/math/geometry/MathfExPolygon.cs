using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public partial class MathfEx 
{
    public static bool PolyContainsPoint(List<Vector2> polyPoints, Vector2 p)
    {
        var j = polyPoints.Count - 1;
        var inside = false;

        for (var i = 0; i < polyPoints.Count; j = i++)
        {
            if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
                (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
                inside = !inside;
        }
        return inside;
    }
}