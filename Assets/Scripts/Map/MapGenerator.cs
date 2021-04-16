using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * There can be different types of world generation
 * Play with the values, create "World Types".
 */

[RequireComponent(typeof(Grid))]
public class MapGenerator : MonoBehaviour
{
    
    #region Variables
    [Header("Map Settings")]
    [Range(0,100)]  public int width;
    [Range(0,100)]  public int height;
    [Range(0,1)]    public float hazardConcentration;
    public bool gradualGeneration;

    [Header("Noise Settings")]
    [Range(0,1000)] public int seed;
    public bool randomize;

    [Range(35,60)]  public int filledUndergroundPercentage;
    [Range(0,30)]   public float surfaceNoiseScale;
    
    [Header("Tile Settings")]
    public Tilemap undergroundTileMap;
    public Tilemap backUndergroundTileMap;
    public Tile underground;
    public TileBase hazardous;
    public Tile backUnderground;
    public Tile[] gems;

    System.Random rnd;

    #endregion

    void Start()
    {
        rnd = new System.Random(RandomSeed.seed);
        
        undergroundTileMap = GetComponentInChildren<Tilemap>();
        RenderMap(backUndergroundTileMap, backUnderground, new int[width, height]);

        if(gradualGeneration)
        {
            StartCoroutine(GradualRenderMapWithTile(undergroundTileMap, underground, GenerateMap(width, height, seed, surfaceNoiseScale, filledUndergroundPercentage)));
        }
        else
        {
            RenderMapWithTile(undergroundTileMap, underground, GenerateMap(width, height, seed, surfaceNoiseScale, filledUndergroundPercentage));
        }
    }

    int [,] GenerateMap(int width, int height, int seed, float surfaceNoiseScale, int filledUndergroundPercentage)
    {
        int[,] map = new int[width, height];

        for(int y = height-2; y < height; y++)
            for(int x = 0; x <= map.GetUpperBound(0); x++)
                map[x,y] = 1;

        GenerateMooreCAMap(map, seed, filledUndergroundPercentage, 2);
     
        // Generates Hazards
        GenerateVeinedTiles(map, 2, seed, hazardConcentration);

        GenerateGems(map, 3, rnd.Next(3,5));

        return map;
    }

    int [,] GenerateMooreCAMap(int[,] map, int seed, int fillPercentage, int maxHeight)
    {
        float clampedHeight = map.GetUpperBound(1) - maxHeight;
        float width = map.GetUpperBound(0);

        // Generate noise tiles
        for(int y = 0; y <= clampedHeight; y++)
        {
            for(int x = 0; x <= width; x++)
            {
                // Edges tiles.
                if(x == 0 || y == 0 || x == width || y == clampedHeight)
                {
                    map[x,y] = 1;
                    continue;
                }
                else
                {
                    map[x,y] = rnd.Next(0,100) < fillPercentage ? 1 : 0;
                }
            }
        }

        // Generate Moore Neighborhood
        for(int y = 1; y <= clampedHeight; y++)
        {
            for(int x = 1; x < width; x++)
            {
                int neighbours = GetSurroundingTiles(x, y);

                // Moore rules.
                if(neighbours > 4)
                    map[x,y] = 1;
                else if(neighbours < 4)
                    map[x,y] = 0;
            }
        }

        return map;

        int GetSurroundingTiles(int x, int y)
        {
            int neighbours = 0;

            for(int dy = y-1; dy <= y+1; dy++)
                for(int dx = x-1; dx <= x+1; dx++)
                    if(dx != x || dy != y)
                        if(map[dx, dy] != 0)
                            neighbours++;

            return neighbours;
        }
    }

    void GenerateGems(int[,] map, int tag, int amount)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);
        
        for (int i = 0; i < amount; i++)
        {
            int x = rnd.Next(5,w-5); 
            int y = rnd.Next(5,h-5); 
         
            map[x, y] = tag;
        }
    }

    void GenerateVeinedTiles(int[,] map, int index, int seed, float concentration)
    {
        int w = map.GetUpperBound(0);
        int h = map.GetUpperBound(1);

        int offset = rnd.Next() % 1000;

        for(int y = 1; y < h-1; y++)
        {
            for(int x = 1; x < w-1; x++)
            {
                float rnd = Mathf.PerlinNoise((float)(x+offset)/15, (float)(y+offset)/15);

                if(rnd < concentration)
                    map[x,y] = index;
            }
        }
        
    }

    void RenderMap(Tilemap tilemap, Tile tile, int[,] map)
    {
        for(int y = 0; y <= map.GetUpperBound(1); y++)
            for(int x = 0; x <= map.GetUpperBound(0); x++)
                    tilemap.SetTile(new Vector3Int(x,y,0), tile);
    }
    
    IEnumerator GradualRenderMapWithTile(Tilemap tilemap, Tile tile, int[,] map)
    {
        for(int y = 0; y <= map.GetUpperBound(1); y++)
            for(int x = 0; x <= map.GetUpperBound(0); x++)
            {
                if(map[x,y] == 0)
                    tilemap.SetTile(new Vector3Int(x,y,0), null);
                else if(map[x,y] == 1)
                    tilemap.SetTile(new Vector3Int(x,y,0), tile);
                else if(map[x,y] == 2)
                    tilemap.SetTile(new Vector3Int(x,y,0), hazardous);

                yield return new WaitForEndOfFrame();
            }
    }

    void RenderMapWithTile(Tilemap tilemap, Tile tile, int[,] map)
    {
        for(int y = 0; y <= map.GetUpperBound(1); y++)
            for(int x = 0; x <= map.GetUpperBound(0); x++)
                if(map[x,y] == 0)
                    tilemap.SetTile(new Vector3Int(x,y,0), null);
                else if(map[x,y] == 1)
                    tilemap.SetTile(new Vector3Int(x,y,0), tile);
                else if(map[x,y] == 2)
                    tilemap.SetTile(new Vector3Int(x,y,0), hazardous);
                else if(map[x,y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), gems[UnityEngine.Random.Range(0,gems.Length)]);
                }
    }

}
