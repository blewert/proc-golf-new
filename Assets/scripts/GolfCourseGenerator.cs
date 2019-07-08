using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public Terrain terrain;
    public TerrainGenerator terrainGenerator;
    public SplatmapOptions splatOptions;

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

        //Generate the terrain
        terrainGenerator = new TerrainGenerator(terrain, splatOptions);
        terrainGenerator.setSplatmaps(fairway, terrain, splatOptions);

        //Return it
        RNGStateManager.Pop();
    }

    public Transform sample;

    void Update()
    {
        foreach(var hull in fairway.hulls)
        {
            var points = hull.points.Select(x => new Vector2(x.x, x.z)).ToList();

            if(MathfEx.PolyContainsPoint(points, new Vector2(sample.position.x, sample.position.z)))
                Debug.Log("yep");
        }
    }

    void OnDrawGizmos()
    {
        if(fairway != null)
            fairway.DrawGizmos();
    }

}
