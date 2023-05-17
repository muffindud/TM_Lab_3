using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool
{
    public string name;
    public Sprite sprite;
    public int damage;
    public int speed;

    public Tool(string name, Sprite sprite, int damage, int speed)
    {
        this.name = name;
        this.sprite = sprite;
        this.damage = damage;
        this.speed = speed;
    }
}
