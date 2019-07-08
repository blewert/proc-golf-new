using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EPPZ.Geometry;
using EPPZ.Geometry.Model;

public class TerrainGenerator
{
    public SplatmapOptions splatOptions;
    public Terrain terrain;

    public TerrainGenerator(Terrain terrain, SplatmapOptions splatOptions)
    {
        this.splatOptions = splatOptions;
        this.terrain = terrain;
    }

    public void setSplatmaps(Fairway fairway, Terrain terrain, SplatmapOptions options)
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

        //Get the maps
        float[,,] maps = new float[tdata.alphamapWidth, tdata.alphamapHeight, 3];

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
                if(fairway.isPointInsideHole(point))
                    maps[y, x, 2] = 1.0f;

                else if(fairway.isPointInside(point))
                    maps[y, x, 1] = 1.0f;

                else
                    maps[y, x, 0] = 1.0f;
            }
        }

        //And set the alphamaps
        tdata.SetAlphamaps(0, 0, maps);
        terrain.Flush();
    }
}