using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAction
{
    public bool OnlyOnceAction;
    public IntentType Intent;
    public List<FunctionModule> Function;
    public int Repeat = 1;
}


public enum MonsterFunctionType
{
    Attack,Defend,Weak,Fragile
}

[CreateAssetMenu(menuName = "MonsterAsset")]
public class MonsterAsset : ScriptableObject
{
    [Header("Genaral Info")]

    public string MonsterName;
    [TextArea(2, 3)]
    public string Description;

    public GameObject Prefab;
    [Header("HP")]
    public int MinHp;
    public int MaxHp;

    [Header("Pattern")]
    public List<MonsterAction> Pattern;
}
