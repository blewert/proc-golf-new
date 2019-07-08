using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CourseComponent
{
    protected Transform transform;

    protected Vector3 origin 
    {
        get { return transform.position; }
    }

    public abstract bool isPointInside(Vector3 point);

    public CourseComponent(Transform transform)
    {
        this.transform = transform;
    }
}