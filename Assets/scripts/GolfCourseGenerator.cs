using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCourseGenerator : MonoBehaviour
{
    /// <summary>
    /// The course seed
    /// </summary>
    public int seed = -1;

    /// <summary>
    /// Options for the generated fairway.
    /// </summary>
    public FairwayOptions fairwayOptions;
    public Fairway fairway;

    /// <summary>
    /// When the game runs, generate the course
    /// </summary>
    void Start()
    {
        this.GenerateCourse();
    }

    /// <summary>
    /// Generates a random golf course
    /// </summary>
    private void GenerateCourse()
    {
        //Save the seed
        RNGStateManager.Push(seed);

        //Generate a new fairway
        fairway = new Fairway(transform, fairwayOptions);

        //Return it
        RNGStateManager.Pop();
    }

    void OnDrawGizmos()
    {
        if(fairway != null)
            fairway.DrawGizmos();
    }

}
