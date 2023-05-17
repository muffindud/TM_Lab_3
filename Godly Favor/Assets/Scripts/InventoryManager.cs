using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Game game;
    // public int[,] slotCoords = new int[16, 2];
    public int selectedSlot = 0;

    public Sprite[] slotItems = new Sprite[16];
    GameObject selector;

    public float[,] slotCoords = {
        {-2.750f, 2.750f}, {-1.375f, 2.750f}, {0.000f, 2.750f}, {1.375f, 2.750f}, 
        {-2.750f, 1.375f}, {-1.375f, 1.375f}, {0.000f, 1.375f}, {1.375f, 1.375f}, 
        {-2.750f, 0.000f}, {-1.375f, 0.000f}, {0.000f, 0.000f}, {1.375f, 0.000f}, 
        {-2.750f, -1.375f}, {-1.375f, -1.375f}, {0.000f, -1.375f}, {1.375f, -1.375f}
    };

    public float[,] itemSlotCoords = {
        {-2.0625f, 2.0625f}, {-0.6875f, 2.0625f}, {0.6875f, 2.0625f}, {2.0685f, 2.0625f}, 
        {-2.0625f, 0.6875f}, {-0.6875f, 0.6875f}, {0.6875f, 0.6875f}, {2.0685f, 0.6875f}, 
        {-2.0625f, -0.6875f}, {-0.6875f, -0.6875f}, {0.6875f, -0.6875f}, {2.0685f, -0.6875f}, 
        {-2.0625f, -2.0625f}, {-0.6875f, -2.0625f}, {0.6875f, -2.0625f}, {2.0685f, -2.0625f}
    };

    public int[] slotAmounts = {
        0, 0, 0, 0, 
        0, 0, 0, 0, 
        0, 0, 0, 0, 
        0, 0, 0, 0
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

    public void PickUp(Sprite item)
    {
        if (GetItemSlotIndex(item) == -1)
        {
            int itemIndex = GetItemSlotIndex();
            slotItems[itemIndex] = item;
            slotAmounts[itemIndex]++;

            GameObject newItem = new GameObject();
            newItem.name = item.name + "_inventory";
            newItem.AddComponent<SpriteRenderer>();
            newItem.GetComponent<SpriteRenderer>().sprite = item;
            newItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
            newItem.transform.parent = GameObject.Find("inventory_grid").transform;
            newItem.transform.localPosition = new Vector3(
                itemSlotCoords[itemIndex, 0], 
                itemSlotCoords[itemIndex, 1], 
                0
            );
        }
        else
        {
            slotAmounts[GetItemSlotIndex(item)]++;
        }
    }

    public int GetItemSlotIndex(Sprite item = null)
    {
        int ind = -1;
        
        if (item == null)
        {
            for (int i = 0; i < 16; i++)
            {
                if (slotItems[i] == null)
                {
                    ind = i;
                    break;
                }
            }
        }
        else
        {
            ind = Array.IndexOf(slotItems, item);
        }

        return ind;
    }

    public bool Place()
    {
        if (slotItems[selectedSlot] != null)
        {
            slotAmounts[selectedSlot]--;
            if (slotAmounts[selectedSlot] == 0)
            {
                slotItems[selectedSlot] = null;
                Destroy(GameObject.Find(slotItems[selectedSlot].name + "_inventory"));
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
