using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public enum TurnState
    {
        None = 0,
        PlayerTurnBegin, PlayerTurn, PlayerTurnEnd,
        EnemyTurnBegin, EnemyTurn, EnemyTurnEnd
    }
    [SerializeField]
    List<ICardObserver> Observers;

    TurnState m_TurnState;

    //뽑을 카드 더미
    [SerializeField]
    List<CardAsset> BattleDeck;
    //버려진 카드 더미
    [SerializeField]
    List<CardAsset> DiscardDeck;
    //소멸한 카드
    List<CardAsset> Extinction;
    //손패
    List<CardAsset> Hand;

    List<GameObject> Monsters;




    int CurrEnergy;

    private void Awake()
    {
        m_TurnState = TurnState.None;

        Observers = new List<ICardObserver>();

        BattleDeck = new List<CardAsset>();
        DiscardDeck = new List<CardAsset>();
        Extinction = new List<CardAsset>();

        Hand = new List<CardAsset>();


        Monsters = new List<GameObject>();


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DeckShuffle()
    {
        for(int i = 0; i < BattleDeck.Count; ++i)
        {
            int rand1 = Random.Range(0, BattleDeck.Count);
            int rand2;
            do
            {
                rand2 = Random.Range(0, BattleDeck.Count);
            } while (rand1 == rand2);

            CardAsset tmp;
            tmp = BattleDeck[rand1];
            BattleDeck[rand1] = BattleDeck[rand2];
            BattleDeck[rand2] = tmp;

        }
    }
    public void BattleInit()
    {
        BattleDeck = MainSceneController.Instance.m_PlayerInfo.GetOriginDeck();

        //전투로 넘어가기전에 시드값 초기화로 항상 세이브 로드해도 같은 결과가 나오게 함
        Random.InitState(MainSceneController.Instance.m_PlayerInfo.GetCurFloor() + MainSceneController.Instance.m_PlayerInfo.GetSeed());
        MainSceneController.Instance.m_PlayerInfo.SetSeed(MainSceneController.Instance.m_PlayerInfo.GetCurFloor() + MainSceneController.Instance.m_PlayerInfo.GetSeed());

        DeckShuffle();
        ChangeTurnState(TurnState.PlayerTurnBegin);
        
    }
    public void ClearData()
    {
        m_TurnState = TurnState.None;
         
        //만약 몹이 남아있다면 전부 제거
        if (Monsters.Count > 0)
        {
            foreach(var Monster in Monsters)
            {
                Destroy(Monster);
            }
            Monsters.Clear();
        }
    }
    public void DrawCard(int CardCount)
    {
        for (int i = 0; i < CardCount; ++i)
        {
            if (BattleDeck.Count == 0) {
                foreach(var card in DiscardDeck)
                {
                    BattleDeck.Add(card);
                    DiscardDeck.Remove(card);
                }
            }
            //손패 10장 이상이면 뽑는 카드들은 버려짐
            if (Hand.Count >= 10)
            {
                DiscardDeck.Add(BattleDeck[0]);
                BattleDeck.Remove(BattleDeck[0]);
            }
            //10장이하면 손패에 들어옴
            else
            {
                Hand.Add(BattleDeck[0]);

                if (Observers.Count > 0)
                {
                    foreach (var observer in Observers)
                    {
                        observer.DrawCard(BattleDeck[0]);
                    }
                }

                BattleDeck.Remove(BattleDeck[0]);
            }
        }
    }
    public List<GameObject> GetMonsterData()
    {
        return Monsters;
    }

    public List<CardAsset> GetDeckData()
    {
        return BattleDeck;
    }
    public List<CardAsset> GetDiscardData()
    {
        return DiscardDeck;
    }

    public List<CardAsset> GetExtinctionData()
    {
        return Extinction;
    }

    public void Attach(ICardObserver observer)
    {
        Observers.Add(observer);
    }
    public void Detach(ICardObserver observer)
    {
        Observers.Remove(observer);
    }
    public void Notify()
    {
        if (Observers.Count > 0)
        {
            foreach (var observer in Observers)
            {
                observer.UpdateData(BattleDeck.Count, DiscardDeck.Count, Extinction.Count, Hand.Count, CurrEnergy);
            }
        }
    }
    public void ChangeTurnState(TurnState Turn)
    {
        if (m_TurnState == Turn) return;
        m_TurnState = Turn;

        switch (m_TurnState)
        {
            case TurnState.PlayerTurnBegin:
                CurrEnergy = 3;
                Notify();
                DrawCard(MainSceneController.Instance.m_PlayerInfo.GetDrawPerTurn());
                ChangeTurnState(TurnState.PlayerTurn);
                break;
            case TurnState.PlayerTurn:
                Notify();
                break;
            case TurnState.PlayerTurnEnd:
                Notify();
                break;
            case TurnState.EnemyTurnBegin:
                break;
            case TurnState.EnemyTurn:
                break;
            case TurnState.EnemyTurnEnd:
                break;
        }
    }
}
