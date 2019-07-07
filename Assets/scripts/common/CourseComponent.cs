using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseComponent
{
    protected Transform transform;

    protected Vector3 origin 
    {
        get { return transform.position; }
    }

    public CourseComponent(Transform transform)
    {
        this.transform = transform;
    }
}