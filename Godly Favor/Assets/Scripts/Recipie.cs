using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipie
{
    public Sprite[] ingredients;
    public int[] ingredientAmounts;
    public Sprite result;
    public int resultAmount;

    public Recipie(Sprite[] ingredients, int[] ingredientAmounts, Sprite result, int resultAmount)
    {
        this.ingredients = ingredients;
        this.ingredientAmounts = ingredientAmounts;
        this.result = result;
        this.resultAmount = resultAmount;
    }    
}
