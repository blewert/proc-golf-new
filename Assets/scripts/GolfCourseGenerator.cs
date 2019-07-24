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
    public TreeOptions treeOptions;

    public Transform player;

    /// <summary>
    /// When the game runs, generate the course
    /// </summary>
    void Start()
    {
        this.GenerateCourse();

        this.SetupPlayer();
    }

    private void SetupPlayer()
    {
        //Is the player set in the inspector?
        if(player == null)
            throw new UnityException("Player object not set in the inspector");


        var playerPos = fairway.playerPosition + Vector3.up * player.GetComponent<CharacterController>().height;

        //Set their position
        player.position = playerPos;

        //And look towards the flag
        player.LookAt(fairway.flagPosition);
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
        terrainGenerator = new TerrainGenerator(terrain, splatOptions, treeOptions);
        terrainGenerator.setMaps(fairway, terrain);

        //Now spawn the trees
        terrainGenerator.spawnTrees(fairway);

        //Return it
        RNGStateManager.Pop();
    }

    public Transform sample;

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if(fairway != null)
            fairway.DrawGizmos();
    }

}
