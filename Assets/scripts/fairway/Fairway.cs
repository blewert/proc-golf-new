using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Fairway : CourseComponent
{
    private FairwayOptions options;

    public struct FairwayPoint
    {
        public Vector3 position;
        public float radius;
    }

    public List<FairwayPoint> points = new List<FairwayPoint>();
    public List<Hull> hulls = new List<Hull>();

    public Hull holeHull;

    public Fairway(Transform parent, FairwayOptions options) : base(parent)
    {
        //Set up options!
        this.options = options;

        //Generate fairway points
        this.GenerateFairwayPoints();

        //Generate hulls
        this.GenerateFairwayHulls();

        //Generate hole hull
        this.GenerateHole();
    }

    public override bool isPointInside(Vector3 point)
    {
        //Does any hull contain this point?
        return hulls.Any(x => x.ContainsPoint(point));
    }

    public bool isPointInsideHole(Vector3 point)
    {
        //Does it contain a point?
        return holeHull.ContainsPoint(point);
    }

    private void GenerateHole()
    {
        //Find the last point
        var lastPoint = points.Last();

        //Choose n random points around this point
        var samplePoints = new List<Vector3>();

        //Find offset point
        var offset = MathfEx.OffsetPointByAngleXZ(Vector3.zero, Random.value * 360f, options.offsetRadius * lastPoint.radius, transform.rotation);

        for(int i = 0; i < options.holeSampleCount; i++)
        {
            //Choose angle and distance
            float randAngle = Random.Range(0f, Mathf.PI * 2);
            float randDistance = Random.Range(0f, options.holeScaleFactor * (lastPoint.radius / 2f));

            //Add this point
            samplePoints.Add(MathfEx.OffsetPointByAngleXZ(offset + lastPoint.position, randAngle, randDistance, transform.rotation, true));
        }

        //Find the convex hull of these points
        holeHull = new Hull(samplePoints);
    }

    private void GenerateFairwayHulls()
    {
        for(int i = 1; i < points.Count; i++)
        {
            FairwayPoint prev = points[i-1];
            FairwayPoint next = points[i];

            var prevPoints = MathfEx.CircleCoordinatesXZ(prev.position, prev.radius, transform.rotation, options.circleFidelity);
            var nextPoints = MathfEx.CircleCoordinatesXZ(next.position, next.radius, transform.rotation, options.circleFidelity);

            hulls.Add(new Hull(prevPoints, nextPoints));
        }
    }

    private void GenerateFairwayPoints()
    {
        float rad = Random.Range(options.minRadius, options.maxRadius);

        //First add the origin point
        points.Add(new FairwayPoint
        {
            position = origin,
            radius = rad
        });

        //There must be at least one
        options.pointCount = Mathf.Max(options.pointCount, 1);

        float angle = 0f;

        //Run from 0 to n - 1
        for(int i = 0; i < (options.pointCount - 1); i++)
        {
            //Find where to offset from
            Vector3 originPoint = points[i].position;

            //Compute the random angle
            angle += Random.Range(-options.angleRange, options.angleRange);

            //Calculate the new point
            Vector3 newPoint = MathfEx.OffsetPointByAngleXZ(originPoint, angle, options.distance, transform.rotation);

            rad = Random.Range(options.minRadius, options.maxRadius);

            if(i == (options.pointCount - 2))
                rad = options.maxRadius * options.holeScaleFactor;

            //And add it into the list of points
            points.Add(new FairwayPoint
            {
                position = newPoint,
                radius = rad
            });
        }
    }

    public void DrawGizmos()
    {
        foreach(var fairwayPoint in points)
        {
            // GizmosEx.DrawCircle(fairwayPoint.position, fairwayPoint.radius, transform.up);

            // var coords = MathfEx.CircleCoordinatesXZ(fairwayPoint.position, fairwayPoint.radius, transform.rotation);

            // foreach(var coord in coords)
            //     Gizmos.DrawWireSphere(coord, 0.1f);
            
        }

        Gizmos.color = Color.white;

        foreach(var hull in hulls)
            GizmosEx.DrawPolygon(hull.points);        

        Gizmos.color = Color.yellow;    
        GizmosEx.DrawPolygon(holeHull.points);
    }
}
