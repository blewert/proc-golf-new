/**
 * @file   MathfExRandOffset.cs
 * @author Benjamin Williams <bwilliams@lincoln.ac.uk>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static partial class MathfEx
{
    public static Vector3 RandomPointOnTerrain(Terrain terrain = default(Terrain))
    {
        if(terrain == default(Terrain))
            terrain = Terrain.activeTerrain;

        var randPoint = new Vector3(Random.value, 0, Random.value);
        
        randPoint.Scale(terrain.terrainData.size);

        randPoint.y = terrain.SampleHeight(randPoint);

        return randPoint;
    }

    public static Vector3 Polar2CartXZ(float angle, float distance, Quaternion orientation, bool radians = false)
    {
        //Multiplier for changing angle modes
        float convMultiplier = (radians) ? (1f) : (Mathf.Deg2Rad);

        //The result
        Vector3 result = Vector3.zero;

        //Calculate x and z
        result.x = Mathf.Cos(angle * convMultiplier) * distance;
        result.z = Mathf.Sin(angle * convMultiplier) * distance;

        //Transform them into the space of the given orientation
        return orientation * result;
    }

    public static List<Vector3> CircleCoordinatesXZ(Vector3 position, float radius, Quaternion orientation, int fidelity = 16)
    {
        //A list of results
        List<Vector3> results = new List<Vector3>();

        //The amount of radians to increment by
        float angleInc = (Mathf.PI * 2f) / fidelity;

        for(int i = 0; i < fidelity; i++)
        {
            //Find the angle
            float angle = angleInc * i;

            //Offset a point 
            Vector3 point = OffsetPointByAngleXZ(position, angle, radius, orientation, true);

            //Save it
            results.Add(point);
        }

        //And return the results
        return results;
    }

    public static Vector3 OffsetPointByAngleXZ(Vector3 point, float angle, float distance, Quaternion orientation, bool radians = false)
    {
        //Just return p + offset
        return point + Polar2CartXZ(angle, distance, orientation, radians);
    }

    public static Vector3 OffsetPointByAngleXZ(Vector3 point, float angle, float distance, Vector3 orientation = default(Vector3))
    {
        //Nothing passed? set to up vector
        if(orientation == default(Vector3))
            orientation = Vector3.up;

        //Find quaternion along this up vector
        Quaternion rot = Quaternion.LookRotation(orientation, Vector3.up);

        //And call the same function but with a quat
        return OffsetPointByAngleXZ(point, angle, distance, rot);
    }
}