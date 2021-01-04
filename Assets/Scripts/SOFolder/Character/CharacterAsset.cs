using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    None=0,Ironclad,Silent,Defect,Watcher
}
[CreateAssetMenu(menuName = "CharacterAsset")]
public class CharacterAsset : ScriptableObject
{
    [Header("Genaral Info")]

    [TextArea(2, 3)]
    public string CharacterName;
    [TextArea(2, 3)]
    public string Description;
    //스타트 유물
    public GameObject Prefab;
    public int Hp;
    public int Gold;

    public CardAsset[] StartDeck;
}
