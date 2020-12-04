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

    [Header("Noise Settings")]
    [Range(0,1000)] public int seed;
    public bool randomize;
    [Tooltip("This value determines where the surface noise ends and the ground surface begins.")]
    [Range(0, 14)]  public int heightDivisor;
    [Range(35,60)]  public int filledUndergroundPercentage;
    [Range(0,30)]   public float surfaceNoiseScale;
    
    [Header("Tile Settings")]
    public Tilemap undergroundTileMap;
    public Tilemap backUndergroundTileMap;
    public Tile underground;
    public Tile backUnderground;
    #endregion

    void Start()
    {
        if(randomize)
            seed = System.DateTime.Now.GetHashCode() % 1000;   

        undergroundTileMap = GetComponentInChildren<Tilemap>();
        RenderMap(backUndergroundTileMap, backUnderground, new int[width, height]);
        RenderMapWithTile(undergroundTileMap, underground, GenerateMap(width, height, seed, heightDivisor, surfaceNoiseScale, filledUndergroundPercentage));
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
                if(map[x,y] == 1)
                    tilemap.SetTile(new Vector3Int(x,y,0), tile);

                yield return new WaitForEndOfFrame();
            }
    }

    void RenderMapWithTile(Tilemap tilemap, Tile tile, int[,] map)
    {
        for(int y = 0; y <= map.GetUpperBound(1); y++)
            for(int x = 0; x <= map.GetUpperBound(0); x++)
                if(map[x,y] == 1)
                    tilemap.SetTile(new Vector3Int(x,y,0), tile);
    }

    int[,] GenerateEmptyMap(int width, int height)
    {
        int[,] map = new int[width, height];

        for(int y = 0; y < height; y++)
            for(int x = 0; x < width; x++)
                    map[x,y] = 1;

        return map;
    }

    int [,] GenerateMooreCAMap(int[,] map, int seed, int fillPercentage, int maxHeight)
    {
        float clampedHeight = map.GetUpperBound(1) - maxHeight;
        GenerateRandomTiles();
        GenerateMooreNeighbourhood();

        return map;

        void GenerateRandomTiles()
        {
            System.Random rnd = new System.Random(seed);
            
            for(int y = 0; y <= clampedHeight; y++)
                for(int x = 0; x < map.GetUpperBound(0); x++)
                    // Edges will have tiles.
                    if(x == 0 || y == 0 || x == width-1 || y == clampedHeight)
                        map[x,y] = 1;
                    else
                        map[x,y] = rnd.Next(0,100) < fillPercentage ? 1 : 0;
        }

        void GenerateMooreNeighbourhood()
        {
            for(int y = 1; y <= clampedHeight; y++)
                for(int x = 1; x < map.GetUpperBound(0); x++)
                {
                    int neighbours = GetSurroundingTiles(x, y);

                    // Moore rules.
                    if(neighbours > 4)
                        map[x,y] = 1;
                    else if(neighbours < 4)
                        map[x,y] = 0;
                }
        }

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

    void GenerateHeightMap(int[,] map, int seed, float surfaceNoiseScale, int surfaceNoiseDepth)
    {
        for(int x = 0; x <= map.GetUpperBound(0); x++)
        {
            float pos = (x + seed)/surfaceNoiseScale;

            // Random value from [0, 1]
            float rndSurfaceDepth = Mathf.PerlinNoise(pos, seed);
            
            // Map it from [0, maxSurfaceNoiseHeight]
            rndSurfaceDepth *= surfaceNoiseDepth;

            // Convert to int
            rndSurfaceDepth = Mathf.FloorToInt(rndSurfaceDepth);

            for(int y = 0; y < (map.GetUpperBound(1)+1) - rndSurfaceDepth; y++)
                map[x,y] = 1;
        }
    }

    int [,] GenerateMap(int width, int height, int seed, int heightDivisor, float surfaceNoiseScale, int filledUndergroundPercentage)
    {
        int[,] map = new int[width, height];

        GenerateHeightMap(map, seed, surfaceNoiseScale, heightDivisor);
        GenerateMooreCAMap(map, seed, filledUndergroundPercentage, heightDivisor);
        
        return map;
    }
}
