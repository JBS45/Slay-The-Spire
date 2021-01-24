using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattlCardData")]
public class BattleDataAsset : ScriptableObject
{
    private const string FilePath = "SOFolder/BattleCardData";

    static BattleDataAsset _instance = null;
    public static BattleDataAsset Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BattleDataAsset>(FilePath);
            }
            return _instance;
        }
    }
    List<CardData> _Deck;
    List<CardData> _Discard;
    List<CardData> _Extinction;
    List<CardData> _Hand;
    int _Energy;

    public List<CardData> Deck { get => _Deck; set => _Deck = value; }
    public List<CardData> Discard { get => _Discard; set => _Discard = value; }
    public List<CardData> Extinction { get => _Extinction; set => _Extinction = value; }
    public List<CardData> Hand { get => _Hand; set => _Hand = value; }
    public int Energy { get => _Energy; set => _Energy = value; }
    public List<IObservers> Observers { get; set; }

    public void Init()
    {
        _Deck = new List<CardData>();
        _Discard = new List<CardData>();
        _Extinction = new List<CardData>();
        _Hand= new List<CardData>();
        _Energy = 3;
    }
    public void Clear()
    {
        _Deck.Clear();
        _Discard.Clear();
        _Extinction.Clear();
        _Hand.Clear();
    }
    public void Notify()
    {
        if (Observers == null)
        {
            return;
        }
        foreach (var observer in Observers)
        {
            observer.UpdateData();
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

}
