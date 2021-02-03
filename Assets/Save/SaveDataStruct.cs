using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataStruct
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

    public List<SaveCardData> Card = new List<SaveCardData>();
    public List<SaveRelic> Relic = new List<SaveRelic>();
    public List<string> PastEvents = new List<string>();
    public List<int> Path = new List<int>();



    public void Save(PlayerDataAsset PlayerData)
    {
        IsSave = true;
        MapGenrateSeed = PlayerData.Seed;
        Character = PlayerData.CharType;

        MaxHP = PlayerData.MaxHp;
        CurHP = PlayerData.CurrentHp;

        CurFloor = PlayerData.CurrentFloor;
        CurFloorIndex = PlayerData.CurrentFloorIndex;
        CurMoney = PlayerData.CurrentMoney;

        CardRemove = PlayerData.CardRemoveCount;

        Card.Clear();
        foreach (var item in PlayerData.OriginDecks)
        {
            SaveCardData tmp = new SaveCardData(item);
            Card.Add(tmp);
        }
        Relic.Clear();
        foreach(var item in PlayerData.Relics)
        {
            SaveRelic tmp = new SaveRelic(item);
            Relic.Add(tmp);
        }
        if (Path.Count <= CurFloor)
        {
            Path.Add(CurFloorIndex);
        }
    }
    public void Save(PlayerDataAsset PlayerData,string EventKey)
    {
        IsSave = true;
        MapGenrateSeed = PlayerData.Seed;
        Character = PlayerData.CharType;

        MaxHP = PlayerData.MaxHp;
        CurHP = PlayerData.CurrentHp;

        CurFloor = PlayerData.CurrentFloor;
        CurFloorIndex = PlayerData.CurrentFloorIndex;
        CurMoney = PlayerData.CurrentMoney;

        CardRemove = PlayerData.CardRemoveCount;

        Card.Clear();
        foreach (var item in PlayerData.OriginDecks)
        {
            SaveCardData tmp = new SaveCardData(item);
            Card.Add(tmp);
        }
        Relic.Clear();
        foreach (var item in PlayerData.Relics)
        {
            SaveRelic tmp = new SaveRelic(item);
            Relic.Add(tmp);
        }
        PastEvents.Add(EventKey);
        if (Path.Count <= CurFloor)
        {
            Path.Add(CurFloorIndex);
        }
    }
}
