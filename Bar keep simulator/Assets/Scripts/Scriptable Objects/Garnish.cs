using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drink/Garnish")]
public class Garnish : ScriptableObject
{
    public string garnishName;
    public GarnishType garnishType;
    public FlavourProfile flavourTag;

}
