using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int worldWidth = 128;
    public int worldHeight = 64;

    public Sprite grassTile;
    public Sprite dirtTile;
    public Sprite stoneTile;
    public Sprite logTile;
    public Sprite leafTile;
    public Sprite ironOreTile;

    public int heightMultiplier = 24;
    public int heightAddition = 24;

    public int worldSeed;
    public float worldNoiseScale = 0.04f;
    public float caveNoiseScale = 0.08f;
    public Texture2D worldNoise;

    public int dirtLayer = 4;
    public int grassLayer = 1;
    public Tile[,] tiles = new Tile[128, 64];
    
    public float treeProbability = 0.1f;
    public float ironOreProbability = 0.05f;

    // Internal use
    private void Start()
    {
        
        GenerateWorldSeed();
        GenerateWorldNoise();
        GenerateWorld();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // PlaceTile(stoneTile, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);
            // Place a tile if there is not already a tile there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            if (tiles[FtoI(mousePos.x), FtoI(mousePos.y)] == null)
            {
                tiles[FtoI(mousePos.x), FtoI(mousePos.y)] = new Tile(mousePos, stoneTile);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            // Remove the tile if there is one there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            if (tiles[FtoI(mousePos.x), FtoI(mousePos.y)] != null)
            {
                tiles[FtoI(mousePos.x), FtoI(mousePos.y)].RemoveTile();
                tiles[FtoI(mousePos.x), FtoI(mousePos.y)] = null;
            }
        }
    }

    // External use
    public void GenerateWorldSeed()
    {
        worldSeed = Random.Range(-100000, 100000);
    }

    public void GenerateWorldNoise()
    {
        worldNoise = new Texture2D(worldWidth, worldHeight);

        for (int x = 0; x < worldNoise.width; x++)
        {
            for (int y = 0; y < worldNoise.height; y++)
            {
                float v = Mathf.PerlinNoise((x + worldSeed) * caveNoiseScale, (y + worldSeed) * caveNoiseScale);
                worldNoise.SetPixel(x, y, new Color(v, v, v));
            }
        }

        worldNoise.Apply();
    }

    public int FtoI(float f)
    {
        return Mathf.RoundToInt(f);
    }

    // public void PlaceTile(Sprite tile, float x, float y)
    // {
    //     GameObject newTile = new GameObject(name = "Tile");
    //     newTile.transform.parent = this.transform;
    //     newTile.AddComponent<SpriteRenderer>();
    //     newTile.GetComponent<SpriteRenderer>().sprite = tile;
    //     newTile.name = tile.name;
    //     newTile.transform.position = new Vector2(Mathf.RoundToInt(x) + 0.5f, Mathf.RoundToInt(y) + 0.5f);
    // }

    public void PlaceTree(float x, float y)
    {
        // PlaceTile(logTile, x, y);
        // PlaceTile(logTile, x, y + 1);
        // PlaceTile(logTile, x, y + 2);
        // PlaceTile(leafTile, x - 1, y + 3);
        // PlaceTile(leafTile, x, y + 3);
        // PlaceTile(leafTile, x + 1, y + 3);
        // PlaceTile(leafTile, x - 1, y + 4);
        // PlaceTile(leafTile, x, y + 4);
        // PlaceTile(leafTile, x + 1, y + 4);
        // PlaceTile(leafTile, x, y + 5);
        tiles[FtoI(x), FtoI(y)] = new Tile(new Vector2(x, y), logTile);
        tiles[FtoI(x), FtoI(y + 1)] = new Tile(new Vector2(x, y + 1), logTile);
        tiles[FtoI(x), FtoI(y + 2)] = new Tile(new Vector2(x, y + 2), logTile);
        tiles[FtoI(x - 1), FtoI(y + 3)] = new Tile(new Vector2(x - 1, y + 3), leafTile);
        tiles[FtoI(x), FtoI(y + 3)] = new Tile(new Vector2(x, y + 3), leafTile);
        tiles[FtoI(x + 1), FtoI(y + 3)] = new Tile(new Vector2(x + 1, y + 3), leafTile);
        tiles[FtoI(x - 1), FtoI(y + 4)] = new Tile(new Vector2(x - 1, y + 4), leafTile);
        tiles[FtoI(x), FtoI(y + 4)] = new Tile(new Vector2(x, y + 4), leafTile);
        tiles[FtoI(x + 1), FtoI(y + 4)] = new Tile(new Vector2(x + 1, y + 4), leafTile);
        tiles[FtoI(x), FtoI(y + 5)] = new Tile(new Vector2(x, y + 5), leafTile);
    } 

    public void GenerateWorld()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            float height = Mathf.PerlinNoise((x + worldSeed) * worldNoiseScale, worldSeed * worldNoiseScale) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                if (worldNoise.GetPixel(x, y - worldHeight).r > 0.2f)
                {
                    if (y < height - dirtLayer - grassLayer)
                    {
                        if (Random.Range(0f, 1f) < ironOreProbability)
                        {
                            tiles[FtoI(x), FtoI(y)] = new Tile(new Vector2(x, y), ironOreTile);
                        }
                        else
                        {    
                            tiles[FtoI(x), FtoI(y)] = new Tile(new Vector2(x, y), stoneTile);
                        }
                    }
                    else if (y < height - grassLayer)
                    {
                        tiles[FtoI(x), FtoI(y)] = new Tile(new Vector2(x, y), dirtTile);
                    }
                    else
                    {
                        tiles[FtoI(x), FtoI(y)] = new Tile(new Vector2(x, y), grassTile);

                        if (Random.Range(0f, 1f) < treeProbability && x > 1 && x < worldWidth - 1 && tiles[x, y] != null)
                        {
                            PlaceTree(x, y + 1);
                        }
                    }
                }
            }
        }
    }
}
