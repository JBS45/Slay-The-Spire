using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TargetOptions
{
    NoTarget=0,
    AllEnemy,
    Enemy,
    Buff
}

public enum RarityOptions
{
    Basic=0,Common,Rare
}

public enum CardType
{
    Neutral=0,Attack,Skill,Power,Curse,Condition
}

public enum CardOptions
{
    Basic=0, Extinction,Nature,Volatile,NoUse,Preservation,Enhance,Duplication,Allcost
}
[CreateAssetMenu(menuName = "CardAsset")]
public class CardAsset : ScriptableObject, IComparable<CardAsset>
{
    [Header("Genaral Info")]
    public CharacterType charType;
    [TextArea(2, 3)]
    public string Description;
    [TextArea(2, 3)]
    public string CardName ;
    public CardType cardType;
    public RarityOptions Rarity;
    [PreviewSprite]
    public Sprite CardImage;
    public int Cost;
    public int LimitOfCardInDeck = -1;
    public int AttackCount;
    public bool MultipleEnhance = false;
    public bool Token = false;

    [Header("CardInfo")]
    public string CardScriptName;
    public TargetOptions Targets;

    public int CompareTo(CardAsset other)
    {
        if (other.Rarity < this.Rarity)
        {
            return 1;
        }
        else if(other.Rarity > this.Rarity)
        {
            return -1;
        }
        else
        {
            return name.CompareTo(other.name);
        }
    }
}
