using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Sandtrap : CourseComponent
{
    public Hull hull;

    public SandtrapOptions options;
    public Vector3 centroid;

    public float radius;

    public Sandtrap(Vector3 point, Transform parent, SandtrapOptions options) : base(parent)
    {
        //Setup options
        this.options = options;

        //Centroid
        this.centroid = point;

        //Get radius
        this.radius = Random.Range(options.minRadius, options.maxRadius);

        //Make the hull
        this.GenerateHull();
    }

    public void GenerateHull()
    {
        var points = new List<Vector3>();

        for(int i = 0; i < options.hullPoints; i++)
        {
            //p is initially the centre point
            var p = this.centroid;

            //Offset a random xz
            p += MathfEx.Polar2CartXZ(Random.value * 360f, Random.Range(options.minRadius, options.maxRadius), Quaternion.identity);

            //Add to the list of points
            points.Add(p);
        }

        //Make a new hull with these points
        this.hull = new Hull(points);
    }

    public override bool isPointInside(Vector3 point)
    {
        return hull.ContainsPoint(point);
    }

    public void DrawGizmos()
    {
        // GizmosEx.DrawCircle(centroid, radius, Vector3.up);
        GizmosEx.DrawPolygon(hull.points);
    }
}