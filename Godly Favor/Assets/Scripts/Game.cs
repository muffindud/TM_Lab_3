using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Sprite grassTile;
    public Sprite dirtTile;
    public Sprite stoneTile;
    public Sprite logTile;
    public Sprite leafTile;
    public Sprite ironOreTile;
    public Sprite bedrockTile;

    private int heightMultiplier = 24;
    private int heightAddition = 24;

    private int worldSeed;
    private const float worldNoiseScale = 0.04f;
    private const float caveNoiseScale = 0.08f;
    private Texture2D worldNoise;

    private const int dirtLayer = 4;
    private const int grassLayer = 1;
    public Tile[,] tiles = new Tile[Globals.worldWidth, Globals.worldHeight];
    public Tile[] worldBorder = new Tile[2 * Globals.worldWidth + 2 * Globals.worldHeight + 4];
    
    private const float treeProbability = 0.1f;
    private const float ironOreProbability = 0.05f;

    // Internal use
    private void Start()
    {
        GenerateWorldSeed();
        GenerateWorldNoise();
        GenerateWorld();
        GenerateWorldBorder();
    }

    private void Update()
    {
        PlaceTileOnClick(stoneTile);
        RemoveTileOnClick();
    }

    // External use
    public void GenerateWorldSeed()
    {
        worldSeed = Random.Range(-100000, 100000);
    }

    public void GenerateWorldNoise()
    {
        worldNoise = new Texture2D(Globals.worldWidth, Globals.worldHeight);

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

    public void PlaceTree(float x, float y)
    {
        if (tiles[FtoI(x), FtoI(y)] == null)
            tiles[FtoI(x), FtoI(y)] = new Tile(new Vector2(x, y), logTile);
        if (tiles[FtoI(x), FtoI(y + 1)] == null)
            tiles[FtoI(x), FtoI(y + 1)] = new Tile(new Vector2(x, y + 1), logTile);
        if (tiles[FtoI(x), FtoI(y + 2)] == null)
            tiles[FtoI(x), FtoI(y + 2)] = new Tile(new Vector2(x, y + 2), logTile);
        if (tiles[FtoI(x - 1), FtoI(y + 3)] == null)
            tiles[FtoI(x - 1), FtoI(y + 3)] = new Tile(new Vector2(x - 1, y + 3), leafTile);
        if (tiles[FtoI(x), FtoI(y + 3)] == null)
            tiles[FtoI(x), FtoI(y + 3)] = new Tile(new Vector2(x, y + 3), leafTile);
        if (tiles[FtoI(x + 1), FtoI(y + 3)] == null)
            tiles[FtoI(x + 1), FtoI(y + 3)] = new Tile(new Vector2(x + 1, y + 3), leafTile);
        if (tiles[FtoI(x - 1), FtoI(y + 4)] == null)
            tiles[FtoI(x - 1), FtoI(y + 4)] = new Tile(new Vector2(x - 1, y + 4), leafTile);
        if (tiles[FtoI(x), FtoI(y + 4)] == null)
            tiles[FtoI(x), FtoI(y + 4)] = new Tile(new Vector2(x, y + 4), leafTile);
        if (tiles[FtoI(x + 1), FtoI(y + 4)] == null)
            tiles[FtoI(x + 1), FtoI(y + 4)] = new Tile(new Vector2(x + 1, y + 4), leafTile);
        if (tiles[FtoI(x), FtoI(y + 5)] == null)
            tiles[FtoI(x), FtoI(y + 5)] = new Tile(new Vector2(x, y + 5), leafTile);
    } 

    public void GenerateWorld()
    {
        for (int x = 0; x < Globals.worldWidth; x++)
        {
            float height = Mathf.PerlinNoise((x + worldSeed) * worldNoiseScale, worldSeed * worldNoiseScale) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                if (worldNoise.GetPixel(x, y - Globals.worldHeight).r > 0.2f)
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

                        if (Random.Range(0f, 1f) < treeProbability && x > 1 && x < Globals.worldWidth - 1 && tiles[x, y] != null)
                        {
                            PlaceTree(x, y + 1);
                        }
                    }
                }
            }
        }
    }

    public void GenerateWorldBorder()
    {
        int i = 0;

        for (int x = 0; x < Globals.worldWidth; x++)
        {
            worldBorder[i] = new Tile(new Vector2(x, -1), bedrockTile);
            worldBorder[i + 1] = new Tile(new Vector2(x, Globals.worldHeight), bedrockTile);
            i += 2;
        }
        for (int x = 0; x < Globals.worldHeight; x++)
        {
            worldBorder[i] = new Tile(new Vector2(-1, x), bedrockTile);
            worldBorder[i + 1] = new Tile(new Vector2(Globals.worldWidth, x), bedrockTile);
            i += 2;
        }
        worldBorder[i] = new Tile(new Vector2(-1, -1), bedrockTile);
        worldBorder[i + 1] = new Tile(new Vector2(Globals.worldWidth, -1), bedrockTile);
        worldBorder[i + 2] = new Tile(new Vector2(-1, Globals.worldHeight), bedrockTile);
        worldBorder[i + 3] = new Tile(new Vector2(Globals.worldWidth, Globals.worldHeight), bedrockTile);
    }

    public void RemoveTileOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Remove the tile if there is one there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            if (mousePos.x >= 0 && mousePos.x < Globals.worldWidth && mousePos.y >= 0 && mousePos.y < Globals.worldHeight)
                if (tiles[FtoI(mousePos.x), FtoI(mousePos.y)] != null)
                {
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y)].RemoveTile();
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y)] = null;
                }
        }
    }

    public void PlaceTileOnClick(Sprite tile)
    {
        if (Input.GetMouseButtonDown(1))
        {
            // PlaceTile(stoneTile, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);
            // Place a tile if there is not already a tile there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            if (mousePos.x >= 0 && mousePos.x < Globals.worldWidth && mousePos.y >= 0 && mousePos.y < Globals.worldHeight)
                if (tiles[FtoI(mousePos.x), FtoI(mousePos.y)] == null)
                {
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y)] = new Tile(mousePos, tile);
                }
        }
    }
}
