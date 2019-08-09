using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForceProfile
{
    public float angleUp = 10f;

    public Vector3 forceDirection
    {
        get
        {   
            var forceVec = Vector3.zero;

            forceVec.z = Mathf.Sin(angleUp * Mathf.Deg2Rad) * 1f;
            forceVec.y = Mathf.Cos(angleUp * Mathf.Deg2Rad) * 1f;

            return forceVec;
        }
    }

    public Vector3 TransformDirection(Vector3 vec)
    {
        return Quaternion.LookRotation(vec) * this.forceDirection;
    }
}
