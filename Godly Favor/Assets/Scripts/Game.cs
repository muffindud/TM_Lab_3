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

    public Sprite pickaxeTool;
    public Sprite axeTool;
    public Sprite shovelTool;
    public Sprite swordTool;

    private int heightMultiplier = 24;
    private int heightAddition = 24;

    private int worldSeed;
    private const float worldNoiseScale = 0.04f;
    private const float caveNoiseScale = 0.08f;
    private Texture2D worldNoise;
    private Texture2D backgroundNoise;

    private const int dirtLayer = 4;
    private const int grassLayer = 1;
    public Tile[,,] tiles = new Tile[Globals.worldWidth, Globals.worldHeight, 2];
    public Tile[] worldBorder = new Tile[2 * Globals.worldWidth + 2 * Globals.worldHeight + 4];
    public Tool[] tools = new Tool[5];

    public PlayerController player;
    public InventoryManager inventoryManager;
    public ToolManager toolManager;

    private const float treeProbability = 0.1f;
    private const float ironOreProbability = 0.05f;

    // Internal use
    private void Start()
    {   
        GenerateWorldSeed();
        GenerateWorldNoise();
        GenerateWorld();
        GenerateBackgroundWorld();
        GenerateWorldBorder();
    }

    private void Update()
    {
        PlaceTileOnClick(stoneTile);
        RemoveTileOnClick();
        SetInventorySlot();
        SetToolSlot();
        // OnHoverTint();
    }

    // External use
    public void GenerateWorldSeed()
    {
        worldSeed = Random.Range(-100000, 100000);
    }

    public void InstantiateTools()
    {
        tools[0] = new Tool("hand", null, 1, 1);
        tools[1] = new Tool("pickaxe", pickaxeTool, 2, 3);
        tools[2] = new Tool("axe", axeTool, 4, 3);
        tools[3] = new Tool("shovel", shovelTool, 2, 4);
        tools[4] = new Tool("sword", swordTool, 6, 1);
    }

    public void GenerateWorldNoise()
    {
        worldNoise = new Texture2D(Globals.worldWidth, Globals.worldHeight);
        backgroundNoise = new Texture2D(Globals.worldWidth, Globals.worldHeight);

        for (int x = 0; x < worldNoise.width; x++)
        {
            for (int y = 0; y < worldNoise.height; y++)
            {
                float v = Mathf.PerlinNoise((x + worldSeed) * caveNoiseScale, (y + worldSeed) * caveNoiseScale);
                worldNoise.SetPixel(x, y, new Color(v, v, v));
                float v2 = Mathf.PerlinNoise((x + worldSeed), (y + worldSeed));
                backgroundNoise.SetPixel(x, y, new Color(v2, v2, v2));
            }
        }

        worldNoise.Apply();
        backgroundNoise.Apply();
    }

    public int FtoI(float f)
    {
        return Mathf.RoundToInt(f);
    }

    public void PlaceTree(float x, float y, bool background = false)
    {
        if (background)
        {
            if (tiles[FtoI(x), FtoI(y), 1] == null)
                tiles[FtoI(x), FtoI(y), 1] = new Tile(new Vector2(x, y), logTile, true);
            if (tiles[FtoI(x), FtoI(y + 1), 1] == null)
                tiles[FtoI(x), FtoI(y + 1), 1] = new Tile(new Vector2(x, y + 1), logTile, true);
            if (tiles[FtoI(x), FtoI(y + 2), 1] == null)
                tiles[FtoI(x), FtoI(y + 2), 1] = new Tile(new Vector2(x, y + 2), logTile, true);
            if (tiles[FtoI(x - 1), FtoI(y + 3), 1] == null)
                tiles[FtoI(x - 1), FtoI(y + 3), 1] = new Tile(new Vector2(x - 1, y + 3), leafTile, true);
            if (tiles[FtoI(x), FtoI(y + 3), 1] == null)
                tiles[FtoI(x), FtoI(y + 3), 1] = new Tile(new Vector2(x, y + 3), leafTile, true);
            if (tiles[FtoI(x + 1), FtoI(y + 3), 1] == null)
                tiles[FtoI(x + 1), FtoI(y + 3), 1] = new Tile(new Vector2(x + 1, y + 3), leafTile, true);
            if (tiles[FtoI(x - 1), FtoI(y + 4), 1] == null)
                tiles[FtoI(x - 1), FtoI(y + 4), 1] = new Tile(new Vector2(x - 1, y + 4), leafTile, true);
            if (tiles[FtoI(x), FtoI(y + 4), 1] == null)
                tiles[FtoI(x), FtoI(y + 4), 1] = new Tile(new Vector2(x, y + 4), leafTile, true);
            if (tiles[FtoI(x + 1), FtoI(y + 4), 1] == null)
                tiles[FtoI(x + 1), FtoI(y + 4), 1] = new Tile(new Vector2(x + 1, y + 4), leafTile, true);
            if (tiles[FtoI(x), FtoI(y + 5), 1] == null)
                tiles[FtoI(x), FtoI(y + 5), 1] = new Tile(new Vector2(x, y + 5), leafTile, true);
        }
        else{
            if (tiles[FtoI(x), FtoI(y), 0] == null)
                tiles[FtoI(x), FtoI(y), 0] = new Tile(new Vector2(x, y), logTile);
            if (tiles[FtoI(x), FtoI(y + 1), 0] == null)
                tiles[FtoI(x), FtoI(y + 1), 0] = new Tile(new Vector2(x, y + 1), logTile);
            if (tiles[FtoI(x), FtoI(y + 2), 0] == null)
                tiles[FtoI(x), FtoI(y + 2), 0] = new Tile(new Vector2(x, y + 2), logTile);
            if (tiles[FtoI(x - 1), FtoI(y + 3), 0] == null)
                tiles[FtoI(x - 1), FtoI(y + 3), 0] = new Tile(new Vector2(x - 1, y + 3), leafTile);
            if (tiles[FtoI(x), FtoI(y + 3), 0] == null)
                tiles[FtoI(x), FtoI(y + 3), 0] = new Tile(new Vector2(x, y + 3), leafTile);
            if (tiles[FtoI(x + 1), FtoI(y + 3), 0] == null)
                tiles[FtoI(x + 1), FtoI(y + 3), 0] = new Tile(new Vector2(x + 1, y + 3), leafTile);
            if (tiles[FtoI(x - 1), FtoI(y + 4), 0] == null)
                tiles[FtoI(x - 1), FtoI(y + 4), 0] = new Tile(new Vector2(x - 1, y + 4), leafTile);
            if (tiles[FtoI(x), FtoI(y + 4), 0] == null)
                tiles[FtoI(x), FtoI(y + 4), 0] = new Tile(new Vector2(x, y + 4), leafTile);
            if (tiles[FtoI(x + 1), FtoI(y + 4), 0] == null)
                tiles[FtoI(x + 1), FtoI(y + 4), 0] = new Tile(new Vector2(x + 1, y + 4), leafTile);
            if (tiles[FtoI(x), FtoI(y + 5), 0] == null)
                tiles[FtoI(x), FtoI(y + 5), 0] = new Tile(new Vector2(x, y + 5), leafTile);
        }
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
                            tiles[FtoI(x), FtoI(y), 0] = new Tile(new Vector2(x, y), ironOreTile);
                        }
                        else
                        {    
                            tiles[FtoI(x), FtoI(y), 0] = new Tile(new Vector2(x, y), stoneTile);
                        }
                    }
                    else if (y < height - grassLayer)
                    {
                        tiles[FtoI(x), FtoI(y), 0] = new Tile(new Vector2(x, y), dirtTile);
                    }
                    else
                    {
                        tiles[FtoI(x), FtoI(y), 0] = new Tile(new Vector2(x, y), grassTile);

                        if (Random.Range(0f, 1f) < treeProbability && x > 1 && x < Globals.worldWidth - 1 && tiles[x, y, 0] != null)
                        {
                            PlaceTree(x, y + 1, false);
                        }
                    }
                }
            }
        }
    }
    
    public void GenerateBackgroundWorld()
    {
        for (int x = 0; x < Globals.worldWidth; x++)
        {
            float height = Mathf.PerlinNoise((x + worldSeed) * worldNoiseScale, worldSeed * worldNoiseScale) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                if (backgroundNoise.GetPixel(x, y - Globals.worldHeight).r > 0.2f)
                {
                    if (y < height - dirtLayer - grassLayer)
                    {
                        if (Random.Range(0f, 1f) < ironOreProbability)
                        {
                            tiles[FtoI(x), FtoI(y), 1] = new Tile(new Vector2(x, y), ironOreTile, true);
                        }
                        else
                        {    
                            tiles[FtoI(x), FtoI(y), 1] = new Tile(new Vector2(x, y), stoneTile, true);
                        }
                    }
                    else if (y < height - grassLayer)
                    {
                        tiles[FtoI(x), FtoI(y), 1] = new Tile(new Vector2(x, y), dirtTile, true);
                    }
                    else
                    {
                        tiles[FtoI(x), FtoI(y), 1] = new Tile(new Vector2(x, y), grassTile, true);
                        if (Random.Range(0f, 1f) < treeProbability && x > 1 && x < Globals.worldWidth - 1 && tiles[x, y, 0] != null)
                        {
                            PlaceTree(x, y + 1, true);
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
        // If left shift is held, remove background tile
        bool leftShift = Input.GetKey(KeyCode.LeftShift); 

        if (Input.GetMouseButton(0) && !leftShift)
        {
            // Remove the tile if there is one there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            float distance = Vector2.Distance(mousePos, player.transform.position);

            if (mousePos.x + 0.5f >= 0 && mousePos.x + 0.5f < Globals.worldWidth && mousePos.y + 0.5f >= 0 && mousePos.y + 0.5f < Globals.worldHeight && distance <= player.interactionDistance)
                if (tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0] != null)
                {
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0].RemoveTile();
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0] = null;
                }
        }
        else if(Input.GetMouseButton(0) && leftShift)
        {
            // Remove the tile if there is one there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            float distance = Vector2.Distance(mousePos, player.transform.position);
            bool isBackground = tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0] == null;

            if (mousePos.x + 0.5f >= 0 && mousePos.x + 0.5f < Globals.worldWidth && mousePos.y + 0.5f >= 0 && mousePos.y + 0.5f < Globals.worldHeight && distance <= player.interactionDistance && isBackground)
                if (tiles[FtoI(mousePos.x), FtoI(mousePos.y), 1] != null)
                {
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y), 1].RemoveTile();
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y), 1] = null;
                }
        }
    }

    public void PlaceTileOnClick(Sprite tile)
    {
        // If left shift is held, place background tile
        bool leftShift = Input.GetKey(KeyCode.LeftShift); 

        if (Input.GetMouseButton(1) && !leftShift)
        {
            // PlaceTile(stoneTile, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);
            // Place a tile if there is not already a tile there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            float distance = Vector2.Distance(mousePos, player.transform.position);

            if (mousePos.x + 0.5f >= 0 && mousePos.x + 0.5f < Globals.worldWidth && mousePos.y + 0.5f >= 0 && mousePos.y + 0.5f < Globals.worldHeight && distance <= player.interactionDistance && distance > 1f)
                if (tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0] == null)
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0] = new Tile(mousePos, tile);
        }
        else if (Input.GetMouseButton(1) && leftShift)
        {
            // PlaceTile(stoneTile, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);
            // Place a tile if there is not already a tile there
            Vector2 mousePos = new Vector2();
            mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
            mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
            float distance = Vector2.Distance(mousePos, player.transform.position);
            bool isBackground = tiles[FtoI(mousePos.x), FtoI(mousePos.y), 0] == null;

            if (mousePos.x + 0.5f >= 0 && mousePos.x + 0.5f < Globals.worldWidth && mousePos.y + 0.5f >= 0 && mousePos.y + 0.5f < Globals.worldHeight && distance <= player.interactionDistance && distance > 1f && isBackground)
                if (tiles[FtoI(mousePos.x), FtoI(mousePos.y), 1] == null)
                    tiles[FtoI(mousePos.x), FtoI(mousePos.y), 1] = new Tile(mousePos, tile, true);
        }
    }

    public void SetInventorySlot()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            inventoryManager.IncreaseInventorySlot();
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            inventoryManager.DecreaseInventorySlot();
    }

    public void SetToolSlot()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            toolManager.SetToolSlot(0);
        else if (Input.GetKey(KeyCode.Alpha2))
            toolManager.SetToolSlot(1);
        else if (Input.GetKey(KeyCode.Alpha3))
            toolManager.SetToolSlot(2);
        else if (Input.GetKey(KeyCode.Alpha4))
            toolManager.SetToolSlot(3);
    }

    // TODO: Add a tint on mouse hover
    // public void OnHoverTint()
    // {
    //     Vector2 mousePos = new Vector2();
    //     mousePos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f;
    //     mousePos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f;
    //     float distance = Vector2.Distance(mousePos, player.transform.position);

    //     if (mousePos.x + 0.5f >= 0 && mousePos.x + 0.5f < Globals.worldWidth && mousePos.y + 0.5f >= 0 && mousePos.y + 0.5f < Globals.worldHeight && distance <= player.interactionDistance)
    //     {
            
    //     }
    // }
}
