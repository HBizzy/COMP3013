using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drink/Drink Ingredient")]
public class DrinkIngredient : ScriptableObject
{
    public string ingredientName;
    public IngredientType ingredientType;
    public GarnishType garnishType; //optional if garnish
    public FlavourProfile flavourProfile;

    public Sprite icon;
}

public enum IngredientType
{
    Liquid,
    Ice,
    Garnish,
    Decoration,
    None,
}

public enum FlavourProfile
{
    Sweet,
    Sour,
    None,
    //etc
}

public enum GarnishType
{
    Mint,
    LemonSlice,
    LimeSlice,
    Olive,
    None,
    //etc
}