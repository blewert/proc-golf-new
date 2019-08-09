using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestForceProfile : MonoBehaviour
{
    public Vector3 hitDirection;
    public ForceProfile profile;
    public Transform ball;
    public float testVelocity = 1f;

    [Range(0.01f, 0.1f)]
    public float timestep = 0.1f;

    public void OnDrawGizmos()
    {
        var offset = profile.TransformDirection(hitDirection);

        this.drawBallArc(offset);
        Gizmos.DrawLine(ball.position, ball.position + hitDirection * 3f);
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
    }

    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timestep, float maxTime)
    {
        Vector3 prev = start;
        for (int i = 1; i < 50; i++)
        {
            float t = timestep * i;
            if (t > maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, t);
            if (Physics.Linecast(prev, pos)) break;

            if(i % 2 == 0)
                Gizmos.DrawLine(prev, pos);

            prev = pos;
        }
    }

    public void drawBallArc(Vector3 direction)
    {
        //Direction is now in world space
        //..

        this.PlotTrajectory(ball.position, profile.TransformDirection(hitDirection) * testVelocity, timestep, 2f);
    }
}
