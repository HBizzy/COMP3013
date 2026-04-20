using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPCType")]
public class NPCType : ScriptableObject
{
    public string npcName;
    public float minPatienceTime;
    public float maxPatienceTime;
    public float tipModifier;
    public PersonalityTag personalityTag;

}

public enum PersonalityTag
{
    None,
    Impatient,
    Perfectionist,
    Generous,
    Cheap,
    Picky,
    Regular,

    //etc
}