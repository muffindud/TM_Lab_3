using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public Game game;

    public int selectedSlot = 0;

    public bool hasSword = false;
    public bool hasPickaxeIron = false;
    public bool hasPickaxeStone = false;
    public bool hasPickaxeWood = false;
    public bool hasShovel = false;
    public bool hasAxe = false;

    GameObject selector;

    public float[,] slotCoords = {
        {0.000f, 2.750f},
        {0.000f, 1.375f},
        {0.000f, 0.000f},
        {0.000f, -1.375f}
    };

    void Start()
    {
        selectedSlot = 0;
        selector = GameObject.Find("tool_selector");
        UpdateSlot();
        GameObject.Find("sword_item").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("pickaxe_item_iron").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("pickaxe_item_stone").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("pickaxe_item_wood").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("shovel_item").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("axe_item").GetComponent<SpriteRenderer>().enabled = false;
    }
    
    public void UpdateSlot()
    {
        selector.transform.localPosition = new Vector3(
            slotCoords[selectedSlot, 0], 
            slotCoords[selectedSlot, 1], 
            0
        );
    }

    public void SetToolSlot(int slot)
    {
        selectedSlot = slot;
        UpdateSlot();
    }

    public void AquireSword()
    {
        hasSword = true;

        GameObject.Find("sword_item").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void AquirePickaxeIron()
    {
        hasPickaxeIron = true;
        hasPickaxeStone = false;
        hasPickaxeWood = false;

        GameObject.Find("pickaxe_item_iron").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("pickaxe_item_stone").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("pickaxe_item_wood").GetComponent<SpriteRenderer>().enabled = false;
    }

    public void AquirePickaxeStone()
    {
        hasPickaxeStone = true;
        hasPickaxeWood = false;

        GameObject.Find("pickaxe_item_stone").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("pickaxe_item_wood").GetComponent<SpriteRenderer>().enabled = false;
    }

    public void AquirePickaxeWood()
    {
        hasPickaxeWood = true;
        
        GameObject.Find("pickaxe_item_wood").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void AquireShovel()
    {
        hasShovel = true;

        GameObject.Find("shovel_item").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void AquireAxe()
    {
        hasAxe = true;

        GameObject.Find("axe_item").GetComponent<SpriteRenderer>().enabled = true;
    }
}
