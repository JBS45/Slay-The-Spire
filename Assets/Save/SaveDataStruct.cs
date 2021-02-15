using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataStruct
{
    public bool IsSave = false;
    public bool IsClear = false;
    public int MapGenrateSeed;

    public CharacterType Character;

    public int MaxHP;
    public int CurHP;

    public int CurFloor;
    public int CurFloorIndex;
    public int CurMoney;

    public int Monster;
    public int Elite;
    public int Boss;


    public int CardRemove;

    public List<SaveCardData> Card = new List<SaveCardData>();
    public List<SaveRelic> Relic = new List<SaveRelic>();
    public List<string> PastEvents = new List<string>();
    public List<int> Path = new List<int>();


    public void Save(PlayerDataAsset PlayerData)
    {
        IsSave = true;
        IsClear = false;
        MapGenrateSeed = PlayerData.Seed;
        Character = PlayerData.CharType;

        MaxHP = PlayerData.MaxHp;
        CurHP = PlayerData.CurrentHp;

        CurFloor = PlayerData.CurrentFloor;
        CurFloorIndex = PlayerData.CurrentFloorIndex;
        CurMoney = PlayerData.CurrentMoney;

        CardRemove = PlayerData.CardRemoveCount;

        Monster = PlayerData.Monster;
        Elite = PlayerData.Elite;
        Boss = PlayerData.Boss; 

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
        PastEvents.Clear();
        foreach (var item in PlayerData.PastEvents)
        {
            string tmp = item;
            PastEvents.Add(tmp);
        }
    }
    public void Save(PlayerDataAsset PlayerData,bool clear)
    {
        IsSave = true;
        IsClear = clear;
        MapGenrateSeed = PlayerData.Seed;
        Character = PlayerData.CharType;

        MaxHP = PlayerData.MaxHp;
        CurHP = PlayerData.CurrentHp;

        CurFloor = PlayerData.CurrentFloor;
        CurFloorIndex = PlayerData.CurrentFloorIndex;
        CurMoney = PlayerData.CurrentMoney;

        CardRemove = PlayerData.CardRemoveCount;

        Monster = PlayerData.Monster;
        Elite = PlayerData.Elite;
        Boss = PlayerData.Boss;


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
        if (Path.Count <= CurFloor)
        {
            Path.Add(CurFloorIndex);
        }
        PastEvents.Clear();
        foreach (var item in PlayerData.PastEvents)
        {
            string tmp = item;
            PastEvents.Add(tmp);
        }
    }
    public void Save(PlayerDataAsset PlayerData,string EventKey)
    {
        IsSave = true;
        IsClear = false;
        MapGenrateSeed = PlayerData.Seed;
        Character = PlayerData.CharType;

        MaxHP = PlayerData.MaxHp;
        CurHP = PlayerData.CurrentHp;

        CurFloor = PlayerData.CurrentFloor;
        CurFloorIndex = PlayerData.CurrentFloorIndex;
        CurMoney = PlayerData.CurrentMoney;

        CardRemove = PlayerData.CardRemoveCount;

        Monster = PlayerData.Monster;
        Elite = PlayerData.Elite;
        Boss = PlayerData.Boss;


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
        PastEvents.Clear();
        foreach (var item in PlayerData.PastEvents)
        {
            string tmp = item;
            PastEvents.Add(tmp);
        }

    }
}
