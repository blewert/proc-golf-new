using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using EPPZ.Geometry;
using EPPZ.Geometry.Model;

public class TerrainGenerator
{
    public SplatmapOptions splatOptions;
    public TreeOptions treeOptions;
    public Terrain terrain;

    public TerrainGenerator(Terrain terrain, SplatmapOptions splatOptions, TreeOptions treeOptions)
    {
        this.splatOptions = splatOptions;
        this.treeOptions = treeOptions;
        this.terrain = terrain;
    }

    public void spawnTrees(Fairway fairway)
    {
        for(int i = 0; i < treeOptions.numberOfTrees; i++)
        {
            var pos = MathfEx.RandomPointOnTerrain(terrain);

            if (fairway.isPointInsideOuterHull(pos))
                continue;

            var randPrefab = treeOptions.randomTreePrefab;

            Quaternion rot = Quaternion.identity * Quaternion.Euler(0, Random.value * 360, 0);

            var obj = GameObject.Instantiate(randPrefab, pos, rot);

            obj.transform.localScale = Vector3.one * (1f + Random.Range(-treeOptions.treeScaleVariance, treeOptions.treeScaleVariance));
        }
    }

    public void setMaps(List<Sandtrap> sandtraps, Fairway fairway, Terrain terrain)
    {
        //Alias for terrain data
        TerrainData tdata = terrain.terrainData;

        //Get terrain detail size
        int terrainDetailSize = tdata.alphamapWidth;

        //Not the best way to deal with this, but it is being done before
        //the game starts.. so not that bad

        //Outside = 0 (rough)
        //Inside = 1 (green)
        //Inside, last node = 2 (green hole)
        //Inside, sandtrap = 3  (sandtrap)

        //Array for height map
        var heightMap = tdata.GetHeights(0, 0, tdata.heightmapResolution, tdata.heightmapResolution);

        //Array for detail maps, get the first layer
        var detailLayer = new int[tdata.detailWidth, tdata.detailHeight];

        //Get the maps
        float[,,] maps = new float[tdata.alphamapWidth, tdata.alphamapHeight, 4];

        for(int y = 0; y < terrainDetailSize; y++)
        {
            //Get normalised y
            var normY = (y / (float)terrain.terrainData.alphamapHeight);

            for(int x = 0; x < terrainDetailSize; x++)
            {
                //Get normalised x
                var normX = (x / (float)terrain.terrainData.alphamapWidth);

                //Calculate world pos
                var nx = terrain.transform.position.x + normX * Terrain.activeTerrain.terrainData.size.x;
                var ny = terrain.transform.position.z + normY * Terrain.activeTerrain.terrainData.size.z;

                var point = new Vector3(nx, 0, ny);

                //Running through each (x, y) in the detail map
                //..

                heightMap[y, x] = sandtraps[0].options.sinkDepth;

                //Is it inside a sandtrap?
                if (sandtraps.Any(s => s.isPointInside(point)))
                {
                    maps[y, x, 3] = 1.0f;

                    //Make it a bit lower
                    heightMap[y, x] -= sandtraps[0].options.sinkDepth;
                }

                //Is the point inside the hole?
                else if(fairway.isPointInsideHole(point))
                    maps[y, x, 2] = 1.0f;

                //Otherwise, is it inside the fairway?
                else if(fairway.isPointInside(point))
                    maps[y, x, 1] = 1.0f;

                //It's not in the hole, fairway or sandtraps
                else
                {
                    maps[y, x, 0] = 1.0f;

                    if(!fairway.isPointInsideOuterHull(point))
                        detailLayer[y, x] = splatOptions.densityPerPixel;
                    // else
                    //     detailLayer[y, x] = 1;
                }
            }
        }

        //Set the detail layer map
        tdata.SetDetailLayer(0, 0, 0, detailLayer);

        //Set the heightmap
        tdata.SetHeights(0, 0, heightMap);

        //And set the alphamaps
        tdata.SetAlphamaps(0, 0, maps);
        terrain.Flush();
    }
}