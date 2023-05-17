using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public int recipiesAmount = 9;
    public bool isCraftingMenuOpen = false;

    public Game game;
    public InventoryManager inventoryManager;
    
    public Recipie[] recipies = new Recipie[9];
    public List<Recipie> availableRecipies = new List<Recipie>();

    void Start()
    {
        recipies[0] = new Recipie(
            new Sprite[] {game.logTile},
            new int[] {1}, 
            game.plankTile, 
            4
        );
        recipies[1] = new Recipie(
            new Sprite[] {game.plankTile},
            new int[] {1}, 
            game.stickItem, 
            2
        );
        recipies[2] = new Recipie(
            new Sprite[] {game.ironOreTile},
            new int[] {1}, 
            game.ironIngotItem, 
            1
        );
        recipies[3] = new Recipie(
            new Sprite[] {game.stickItem, game.plankTile},
            new int[] {2, 3}, 
            game.pickaxeToolWood, 
            1
        );
        recipies[4] = new Recipie(
            new Sprite[] {game.stickItem, game.stoneTile},
            new int[] {2, 3}, 
            game.pickaxeToolStone, 
            1
        );
        recipies[5] = new Recipie(
            new Sprite[] {game.stickItem, game.ironIngotItem},
            new int[] {2, 3}, 
            game.pickaxeToolIron, 
            1
        );
        recipies[6] = new Recipie(
            new Sprite[] {game.stickItem, game.ironIngotItem},
            new int[] {1, 2}, 
            game.swordTool, 
            1
        );
        recipies[7] = new Recipie(
            new Sprite[] {game.stickItem, game.ironIngotItem},
            new int[] {2, 1}, 
            game.shovelTool, 
            1
        );
        recipies[8] = new Recipie(
            new Sprite[] {game.stickItem, game.ironIngotItem},
            new int[] {2, 3}, 
            game.axeTool, 
            1
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


    }

    public void CloseCraftingMenu()
    {


        isCraftingMenuOpen = false;
        availableRecipies.Clear(); 
    }
}
