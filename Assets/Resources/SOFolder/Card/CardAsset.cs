using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum TargetOptions
{
    NoTarget=0,
    AllEnemy,
    Random,
    Enemy,
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
public class CardAsset : ScriptableObject
{
    [Header("Genaral Info")]
    public bool IsExtinct = false;
    public bool IsAllCost = false;
    public CharacterType charType;
    [TextArea(2, 3)]
    public string CardName ;
    public CardType cardType;
    public RarityOptions Rarity;
    public TargetOptions Targets;

    public int Cost;
    public int Repeat;

    public CardEnchantData EnchantData;

    [Header("Audio"), Space(10.0f)]
    public AudioClip CardAudio;

    [Header("Sprite"),Space(10.0f),PreviewSprite]
    public Sprite CardImage;

    [Header("Enchant"),Space(10.0f)]
    public bool MultipleEnchant = false;

    [Header("Function")]
    public List<FunctionModule> FunctionAndValue;

}

