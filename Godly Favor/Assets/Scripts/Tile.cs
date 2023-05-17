using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public Vector2 position;
    public Sprite sprite;
    public int hardness;
    public string tool;
    public bool handBreakable;
    public bool brakable = true;
    public bool background;

    public Tile(Vector2 position, Sprite sprite, bool background=false)
    {
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        this.position = position;
        this.sprite = sprite;
        this.background = background;

        switch (sprite.name)
        {
            case "grass_block":
                hardness = 1;
                tool = "shovel";
                handBreakable = true;
                break;
            case "dirt_block":
                hardness = 1;
                tool = "shovel";
                handBreakable = true;
                break;
            case "log_block":
                hardness = 2;
                tool = "axe";
                handBreakable = true;
                break;
            case "leaf_block":
                hardness = 1;
                tool = "axe";
                handBreakable = true;
                break;
            case "stone_block":
                hardness = 3;
                tool = "pickaxe";
                handBreakable = false;
                break;
            case "iron_ore_block":
                hardness = 4;
                tool = "pickaxe";
                handBreakable = false;
                break;
            case "bedrock_block":
                hardness = 999;
                tool = "pickaxe";
                brakable = false;
                break;
            default:
                hardness = 1;
                tool = "pickaxe";
                handBreakable = true;
                break;
        }

        PlaceTile();
    }

    public void PlaceTile()
    {
        GameObject newTile = new GameObject();
        // newTile.transform.parent = this.transform;
        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = this.sprite;
        if (background)
        {
            newTile.name = sprite.name + " " + position.x + " " + position.y + " 1";
            // set the tint of the background tiles
            newTile.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else if (!background)
        {
            newTile.name = sprite.name + " " + position.x + " " + position.y + " 0";
            newTile.tag = "Ground";
            newTile.AddComponent<BoxCollider2D>();
            // newTile.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        newTile.transform.position = new Vector2(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y) + 0.5f);
    }

    public void RemoveTile()
    {
        if (background)
        {
            GameObject tile = GameObject.Find(sprite.name + " " + position.x + " " + position.y + " 1");
            if (tile.name != "bedrock_block " + position.x + " " + position.y + " 1")
                GameObject.Destroy(tile);
        }
        else
        {
            GameObject tile = GameObject.Find(sprite.name + " " + position.x + " " + position.y + " 0");
            if (tile.name != "bedrock_block " + position.x + " " + position.y + " 0")
                GameObject.Destroy(tile);
        }
    }
}