using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public int recipiesAmount = 8;
    public bool isCraftingMenuOpen = false;

    public Game game;
    public InventoryManager inventoryManager;
    
    public Recipie[] recipies = new Recipie[8];
    public List<Recipie> availableRecipies = new List<Recipie>();

    public Sprite interfacePlank;
    public Sprite interfaceIronIngot;
    public Sprite interfacePickaxeWood;
    public Sprite interfacePickaxeStone;
    public Sprite interfacePickaxeIron;
    public Sprite interfaceSword;
    public Sprite interfaceAxe;
    public Sprite interfaceShovel;

    private GameObject interfaceObj;
    
    private float deltaX = 1.25f;
    private float deltaY = 1.25f;
    private float startX = -1.875f;
    private float startY = 1.25f;

    private float absoluteMiddleStartX = 60.625f;
    private float absoluteMiddleStartY = 55.25f;
    private float absoluteTRStartX = 60.25f;
    private float absoluteTRStartY = 55.75f;
    private float absoluteBLStartX = 61.125f;
    private float absoluteBLStartY = 54.75f;

    void Start()
    {
        recipies[0] = new Recipie(
            new Sprite[] {game.logTile},
            new int[] {1}, 
            game.plankTile, 
            4,
            interfacePlank
        );
        recipies[1] = new Recipie(
            new Sprite[] {game.ironOreTile},
            new int[] {1}, 
            game.ironIngotItem, 
            1,
            interfaceIronIngot
        );
        recipies[2] = new Recipie(
            new Sprite[] {game.plankTile},
            new int[] {5}, 
            game.pickaxeToolWood, 
            1,
            interfacePickaxeWood
        );
        recipies[3] = new Recipie(
            new Sprite[] {game.plankTile, game.stoneTile},
            new int[] {2, 3}, 
            game.pickaxeToolStone, 
            1,
            interfacePickaxeStone
        );
        recipies[4] = new Recipie(
            new Sprite[] {game.plankTile, game.ironIngotItem},
            new int[] {2, 3}, 
            game.pickaxeToolIron, 
            1,
            interfacePickaxeIron
        );
        recipies[5] = new Recipie(
            new Sprite[] {game.plankTile, game.ironIngotItem},
            new int[] {1, 2}, 
            game.swordTool, 
            1,
            interfaceSword
        );
        recipies[6] = new Recipie(
            new Sprite[] {game.plankTile, game.ironIngotItem},
            new int[] {2, 1}, 
            game.shovelTool, 
            1,
            interfaceShovel
        );
        recipies[7] = new Recipie(
            new Sprite[] {game.plankTile, game.ironIngotItem},
            new int[] {2, 3}, 
            game.axeTool, 
            1,
            interfaceAxe
        );
        // TODO: Add more recipies
        // TODO: Increment recipiesAmount when adding a new recipie
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCraftingMenuOpen)
                OpenCraftingMenu();
            else
                CloseCraftingMenu();
        }
        Craft();
    }

    public void CheckRecipie(Recipie recipie)
    {
        int requiredItemsAmount = recipie.ingredients.Length;
        bool hasAllItems = true;

        for (int j = 0; j < requiredItemsAmount; j++)
        {
            int slotIndex = inventoryManager.GetItemSlotIndex(recipie.ingredients[j]);
            if (slotIndex == -1)
            {   
                hasAllItems = false;
                break;
            }
            else if (recipie.ingredientAmounts[j] > inventoryManager.slotAmounts[slotIndex])
            {
                hasAllItems = false;
                break;
            }
        }

        if (hasAllItems)
            availableRecipies.Add(recipie);
    }

    public void GetAvailableRecipies()
    {
        for (int i = 0; i < recipiesAmount; i++)
        {
            CheckRecipie(recipies[i]);
        }
    }

    public void OpenCraftingMenu()
    {
        isCraftingMenuOpen = true;
        GetAvailableRecipies();
        interfaceObj = GameObject.Find("crafting_interface");
        interfaceObj.GetComponent<SpriteRenderer>().enabled = true;
        int i = 0;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                GameObject craftableItem = new GameObject();
                craftableItem.transform.parent = interfaceObj.transform;
                craftableItem.AddComponent<SpriteRenderer>();
                craftableItem.GetComponent<SpriteRenderer>().sprite = availableRecipies[i].interfaceSprite;
                craftableItem.GetComponent<SpriteRenderer>().sprite = availableRecipies[i].result;
                craftableItem.GetComponent<SpriteRenderer>().sortingOrder = 5;
                craftableItem.transform.localPosition = new Vector3(startX + (deltaX * x), startY - (deltaY * y), 0);
                float xcoord = startX + (deltaX * x);
                float ycoord = startY - (deltaY * y);
                craftableItem.name = xcoord.ToString() + " " + ycoord.ToString();
                i++;
                if (i >= availableRecipies.Count)
                    break;
            }
            if (i >= availableRecipies.Count)
                break;
        }
    }

    public void Craft()
    {   
        if (isCraftingMenuOpen && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (mousePos.x > absoluteTRStartX + (deltaX * x) && mousePos.y < absoluteTRStartY - (deltaY * y) && mousePos.x < absoluteBLStartX + (deltaX * x) && mousePos.y > absoluteBLStartY - (deltaY * y))
                    {
                        Debug.Log("Clicked on " + x.ToString() + " " + y.ToString()); // YOU STOPPED HERE
                    }
                }
            }
        }
    }

    public void CloseCraftingMenu()
    {
        // destroy all children of interfaceObj
        foreach (Transform child in interfaceObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        interfaceObj.GetComponent<SpriteRenderer>().enabled = false;
        isCraftingMenuOpen = false;
        availableRecipies.Clear(); 
    }
}
