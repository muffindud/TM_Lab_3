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

    // private float absoluteMiddleStartX = 60.625f;
    // private float absoluteMiddleStartY = 55.25f;
    // private float absoluteTRStartX = 60.25f;
    // private float absoluteTRStartY = 55.75f;
    // private float absoluteBLStartX = 61.125f;
    // private float absoluteBLStartY = 54.75f;

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
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        Debug.Log(gameObject.name);
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
            if (i >= availableRecipies.Count - 1)
                break;
            for (int x = 0; x < 4; x++)
            {
                if (i >= availableRecipies.Count - 1)
                    break;
                GameObject craftableItem = new GameObject();
                craftableItem.transform.parent = interfaceObj.transform;
                craftableItem.AddComponent<SpriteRenderer>();
                craftableItem.GetComponent<SpriteRenderer>().sprite = availableRecipies[i].interfaceSprite;
                craftableItem.GetComponent<SpriteRenderer>().sortingOrder = 5;
                craftableItem.AddComponent<BoxCollider2D>();
                craftableItem.tag = "Crafting";
                craftableItem.transform.localPosition = new Vector3(startX + (deltaX * x), startY - (deltaY * y), 0);
                // craftableItem.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                craftableItem.name = i.ToString();
                i++;
            }
        }
    }

    public void Craft(string objName)
    {   
        GameObject obj = GameObject.Find(objName);
        int index = int.Parse(objName);
        // Debug.Log("Crafting " + availableRecipies[index].result.name);
        // Remove the ingredients from the inventory
        Sprite result = availableRecipies[index].result;
        for (int i = 0; i < availableRecipies[index].ingredients.Length; i++)
        {
            if (
                (!game.toolManager.hasPickaxeWood && !game.toolManager.hasPickaxeStone && !game.toolManager.hasPickaxeIron && result.name == "pickaxe_item_wood") ||
                (!game.toolManager.hasPickaxeStone && !game.toolManager.hasPickaxeIron && result.name == "pickaxe_item_stone") ||
                (!game.toolManager.hasPickaxeIron && result.name == "pickaxe_item_iron") ||
                (!game.toolManager.hasAxe && result.name == "axe_item") ||
                (!game.toolManager.hasShovel && result.name == "shovel_item") ||
                (!game.toolManager.hasSword && result.name == "sword_item")
            )
            {
                inventoryManager.RemoveItem(availableRecipies[index].ingredients[i], availableRecipies[index].ingredientAmounts[i]);
                game.IncreaseEventumScore(-0.05f);
                game.IncreaseConsiliumScore(0.05f);
            }
        }
        // Add the result to the inventory
        switch (result.name)
        {
            case "pickaxe_item_iron":
                game.toolManager.AquirePickaxeIron();
                break;
            case "pickaxe_item_stone":
                if (!game.toolManager.hasPickaxeIron)
                    game.toolManager.AquirePickaxeStone();
                break;
            case "pickaxe_item_wood":
                if (!game.toolManager.hasPickaxeStone && !game.toolManager.hasPickaxeIron)
                    game.toolManager.AquirePickaxeWood();
                break;
            case "axe_item":
                game.toolManager.AquireAxe();
                break;
            case "shovel_item":
                game.toolManager.AquireShovel();
                break;
            case "sword_item":
                game.toolManager.AquireSword();
                break;
            default:
                inventoryManager.PickUp(result, availableRecipies[index].resultAmount);
                break;
        }
        CloseCraftingMenu();
        OpenCraftingMenu();
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
