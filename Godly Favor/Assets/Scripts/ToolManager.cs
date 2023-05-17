using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public Game game;

    public int selectedSlot = 0;

    public bool hasSword = false;
    public bool hasPickaxe = false;
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
}
