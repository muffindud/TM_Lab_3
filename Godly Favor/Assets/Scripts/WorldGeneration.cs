using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int worldWidth = 128;
    public int worldHeight = 64;

    public Sprite grassTile;
    public Sprite dirtTile;
    public Sprite stoneTile;

    public int heightMultiplier = 24;
    public int heightAddition = 24;

    public int worldSeed;
    public float worldNoiseScale = 0.04f;
    public float caveNoiseScale = 0.08f;
    public Texture2D worldNoise;

    private void Start()
    {
        SetTextures();
        GenerateWorldSeed();
        GenerateWorldNoise();
        GenerateWorld();
    }

    public void SetTextures()
    {
        // grassTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Graphics/grass_block.png");
        // dirtTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Graphics/dirt_block.png");
        // stoneTile = Resources.Load<Sprite>("Assets/Graphics/stone_block.png");
    }

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

    public void GenerateWorld()
    {
        for (int x = -worldWidth / 2; x <= worldWidth / 2; x++)
        {
            float height = Mathf.PerlinNoise((x + worldSeed) * worldNoiseScale, worldSeed * worldNoiseScale) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                if (worldNoise.GetPixel(x + worldWidth / 2, y - worldHeight).r > 0.2f)
                {    
                    GameObject newTile = new GameObject(name = "Tile");
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    newTile.GetComponent<SpriteRenderer>().sprite = stoneTile;
                    newTile.transform.position = new Vector2(x, y);
                }
            }
        }
    }
}
