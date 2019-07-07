using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosEx
{
#if UNITY_EDITOR
    public static void DrawCircle(Vector3 position, float radius, Quaternion orientation)
    {
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.CircleHandleCap(-1, position, orientation, radius, EventType.Repaint);
    }
#else
    public static void DrawCircle(Vector3 position, float radius, Quaternion orientation, int fidelity = 32, bool drawPoints = false)
    {
        Vector3 cpos = default(Vector3);

        float circumference  = Mathf.PI * 2f;
        float angleIncrement = circumference / (float)fidelity;

        for(int i = 0; i <= fidelity; i++)
        {
            //Compute angle for this particular point
            float a = ((i % fidelity) * angleIncrement);

            //Get last position 
            Vector3 lpos = cpos;

            //Just do polar -> cart conversion
            cpos.x = Mathf.Cos(a) * radius;
            cpos.y = Mathf.Sin(a) * radius;

            //Convert into world space with orientation and position
            //passed
            Vector3 transformedCPos = (orientation) * cpos + position;
            Vector3 transformedLPos = (orientation) * lpos + position;

            if(i != 0)
                Gizmos.DrawLine(transformedLPos, transformedCPos);

            if(drawPoints)
                Gizmos.DrawWireSphere(transformedCPos, radius / (float)fidelity);
        }
    }
#endif

    public static void DrawCircle(Vector3 position, float radius, Vector3 worldUp)
    {
        Quaternion rot = Quaternion.LookRotation(worldUp, Vector3.up);

        DrawCircle(position, radius, rot);
    }

    public static void DrawPolygon(List<Vector3> points)
    {
        for(int i = 1; i < points.Count; i++)
        {
            var prev = points[i-1];
            var next = points[i];

            Gizmos.DrawLine(prev, next);
        }

        Gizmos.DrawLine(points[0], points[points.Count - 1]);
    }
}