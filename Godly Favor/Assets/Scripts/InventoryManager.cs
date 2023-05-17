using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Game game;
    // public int[,] slotCoords = new int[16, 2];
    public int selectedSlot = 0;

    public Sprite[] slotItems = new Sprite[16];
    public int[] slotAmounts = new int[16];
    GameObject selector;

    public float[,] slotCoords = {
        {-2.750f, 2.750f}, {-1.375f, 2.750f}, {0.000f, 2.750f}, {1.375f, 2.750f}, 
        {-2.750f, 1.375f}, {-1.375f, 1.375f}, {0.000f, 1.375f}, {1.375f, 1.375f}, 
        {-2.750f, 0.000f}, {-1.375f, 0.000f}, {0.000f, 0.000f}, {1.375f, 0.000f}, 
        {-2.750f, -1.375f}, {-1.375f, -1.375f}, {0.000f, -1.375f}, {1.375f, -1.375f}
    };

    void Start()
    {
        selectedSlot = 0;
        selector = GameObject.Find("inventory_selector");
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

    public void IncreaseInventorySlot()
    {
        if (selectedSlot < 15)
        {
            selectedSlot++;
        }
        else
        {
            selectedSlot = 0;
        }
        UpdateSlot();
    }

    public void DecreaseInventorySlot()
    {
        if (selectedSlot > 0)
        {
            selectedSlot--;
        }
        else
        {
            selectedSlot = 15;
        }
        UpdateSlot();
    }
}
