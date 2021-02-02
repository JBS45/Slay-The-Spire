using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataStruct : MonoBehaviour
{
    public bool IsSave = false;
    public int MapGenrateSeed;

    public CharacterType Character;

    public int MaxHP;
    public int CurHP;

    public int CurFloor;
    public int CurFloorIndex;
    public int CurMoney;

    public int CardRemove;

    public List<CardData> Card;
    public List<Relic> Relic;

}
