using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2 position;
    public Sprite sprite;

    public Tile(Vector2 position, Sprite sprite)
    {
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        this.position = position;
        this.sprite = sprite;

        PlaceTile();
    }

    public void PlaceTile()
    {
        GameObject newTile = new GameObject();
        // newTile.transform.parent = this.transform;
        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = this.sprite;
        newTile.name = sprite.name + " " + position.x + " " + position.y;
        newTile.transform.position = new Vector2(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y) + 0.5f);
    }

    public void RemoveTile()
    {
        GameObject tile = GameObject.Find(sprite.name + " " + position.x + " " + position.y);
        GameObject.Destroy(tile);
    }
}