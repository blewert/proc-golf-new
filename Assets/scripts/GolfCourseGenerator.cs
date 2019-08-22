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

    public SandtrapOptions sandtrapOptions;
    public List<Sandtrap> sandtraps = new List<Sandtrap>();

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

        //Generate sandtraps
        this.GenerateSandtraps();

        //Generate the terrain
        terrainGenerator = new TerrainGenerator(terrain, splatOptions, treeOptions);
        terrainGenerator.setMaps(sandtraps, fairway, terrain);

        //Move the flag up a bit
        fairway.flagObject.transform.Translate(Vector3.up * sandtrapOptions.sinkDepth * terrain.terrainData.size.y);

        //Now spawn the trees
        terrainGenerator.spawnTrees(fairway);

        //Return it
        RNGStateManager.Pop();
    }

    public void GenerateSandtraps()
    {
        //How many do we need to spawn?
        var amountToSpawn = Random.Range(sandtrapOptions.minSandtrapCount, sandtrapOptions.maxSandtrapCount);

        //Choose the smallest -- either the number of fairways or the number of 
        //possible points which they can be spawned at.. this prevents infinite while loops
        amountToSpawn = Mathf.Min(amountToSpawn, fairway.points.Count - 1);

        //The chosen points
        var chosenIdxs = new int[amountToSpawn];

        //Number of tries, number of successful tries
        int tries = 0, spawned = 0;

        while(spawned < amountToSpawn)
        {
            //Choose a random node in the fairway which isnt:
            // a) the green
            // b) the tee
            // c) chosen from an existing fairway point

            //This is taking a long time.. so stop it
            if(++tries > (amountToSpawn * 10))
                break;

            //Compute random index
            int idx = Random.Range(1, fairway.points.Count);

            //Already contains it? try again..
            if (chosenIdxs.Contains(idx))
                continue;

            //Add the index
            chosenIdxs[spawned] = idx;

            //Choose a random node
            var randomNode = fairway.points[idx];

            //Calculate dist
            var dist = (randomNode.radius * Random.Range(sandtrapOptions.minRadiusDist, 1f));

            //Find offset
            var offset = randomNode.position + MathfEx.Polar2CartXZ(Random.value * 360f, dist, Quaternion.identity);

            //Calculate position
            var pos = offset;

            //Make a sandtrap here
            sandtraps.Add(new Sandtrap(pos, transform, sandtrapOptions));

            //Increment the count
            spawned++;
        }
    }


    void Update()
    {

    }

    void OnDrawGizmos()
    {
        // if(fairway != null)
        //    fairway.DrawGizmos();

        if(sandtraps != null)
        {
            foreach(var sandtrap in sandtraps)
                sandtrap.DrawGizmos();
        }
    }

}
