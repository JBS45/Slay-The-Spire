using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerDataAsset : ScriptableObject
{

    [Header("Genaral Info")]

    CharacterType _CharType;
    public CharacterType CharType { get => _CharType; set => _CharType = value; }
    int _MaxHp;
    public int MaxHp { get => _MaxHp; set { _MaxHp = value; } }
    int _CurrentHp;
    public int CurrentHp { get => _CurrentHp; set => _CurrentHp = value; }
    int _CurrentFloor;
    public int CurrentFloor { get => _CurrentFloor; set => _CurrentFloor = value; }
    int _CurrentFloorIndex;
    public int CurrentFloorIndex { get => _CurrentFloorIndex; set => _CurrentFloorIndex = value; }
    int _CurrentMoney;
    public int CurrentMoney { get => _CurrentMoney; set => _CurrentMoney = value; }
    int _MaxPotions;
    public int MaxPotions { get => _MaxPotions; set => _MaxPotions = value; }
    int _DrawPerTurn = 5;
    public int DrawPerTurn { get => _DrawPerTurn; set => _DrawPerTurn = value; }
    int _EnergeyPerTurn = 3;
    public int EnergeyPerTurn { get => _EnergeyPerTurn; set => _EnergeyPerTurn = value; }
    int _CardRemove = 0;
    public int CardRemoveCount { get => _CardRemove; set => _CardRemove = value; }
    int _Seed;
    public int Seed { get => _Seed; set => _Seed = value; }

    public int Monster;
    public int Elite;
    public int Boss;

    public List<CardData> OriginDecks { get; set; }
    public List<IObservers> Observers { get; set; }
    public List<IDeckChange> DeckChanges { get; set; }
    public List<string> PastEvents = new List<string>();

    public List<RelicData> Relics;

    
    public void Notify()
    {
        if (Observers == null)
        {
            Observers = new List<IObservers>();
        }
        foreach (var observer in Observers)
        {
            observer.UpdateData();
        }
    }
    public void Notify(CardData data)
    {
        if (DeckChanges == null)
        {
            DeckChanges = new List<IDeckChange>();
        }
        foreach (var observer in DeckChanges)
        {
            observer.OriginDeckChange(data);
        }
    }
    public void Attach(IObservers observer)
    {
        if (Observers == null)
        {
            Observers = new List<IObservers>();
        }
        Observers.Add(observer);
    }
    public void Detach(IObservers observer)
    {
        Observers.Remove(observer);
    }
    public void Clear()
    {
        OriginDecks?.Clear();
        Observers?.Clear();
        DeckChanges?.Clear();
        Relics?.Clear();
    }
    public void AddCard(CardData data)
    {
        CardData tmp = new CardData(data);
        OriginDecks.Add(tmp);
        Notify(data);
    }
    public void RemoveCard(CardData data)
    {
        OriginDecks.Remove(data);
        Notify(data);
    }
    public void AddRelic(Relic data)
    {
        RelicData tmpData = new RelicData(data);
        Relics.Add(tmpData);
        GameObject tmp = Instantiate(data.Prefab);
        tmp.transform.SetParent(MainSceneController.Instance.UIControl.RelicBarPos);
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localScale = Vector3.one;
        tmp.GetComponent<RelicScript>().SetData(tmpData);
        tmpData.Attach(tmp.GetComponent<RelicScript>());
    }
    public void AddRelic(RelicData data)
    {
        Relics.Add(data);
        GameObject tmp = Instantiate(data.Prefab);
        tmp.transform.SetParent(MainSceneController.Instance.UIControl.RelicBarPos);
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localScale = Vector3.one;
        tmp.GetComponent<RelicScript>().SetData(data);
        data.Attach(tmp.GetComponent<RelicScript>());
    }
    public int RemoveCardGold()
    {
        int tmp = 75 + (CardRemoveCount * 25);
        return tmp;
    }
}
