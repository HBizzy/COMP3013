using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int cost;
    public UpgradeType upgradeType;
    public float valueModifier; // percentage after boost e.g. +10% 1.1  -20% 0.8
}

public enum UpgradeType
{
    TicketTimeBoost,
    MistakeForgiveness,
    PatienceBoost,
    AlcoholQuality,
    RentReduction,
}