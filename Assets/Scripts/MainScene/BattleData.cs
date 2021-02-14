using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BattleStateDel(BattleDataState state);

public enum BattleDataState
{
    None,Init,Battle,Win,Lose
}
public enum Turn
{
    None,Player,Enemy
}
public enum TurnState
{
    None,TurnBegin, Turn, TurnEnd,
}
public class BattleData : MonoBehaviour
{

    List<IBattleCard> BattleCard;
    List<IDrawEvent> DrawEvents;

    [SerializeField]
    BattleDataAsset m_CardData;
    public BattleDataAsset CardData { get => m_CardData; }

    bool _IsCardUsing;
    public bool IsCardUsing { get => _IsCardUsing; set => _IsCardUsing = value; }

    BattleDataState m_BattelState = BattleDataState.None;
    public BattleDataState CurrentBattelState { get => m_BattelState; }

    Turn m_Turn;
    public Turn CurrentTurn { get => m_Turn; }

    TurnState m_TurnState;
    public TurnState CurrentTurnState { get => m_TurnState; }

    int TurnCount;

    GameObject Player;

    List<GameObject> _Monsters;
    public List<GameObject> Monsters { get => _Monsters; set => _Monsters = value; }

    BattleUIScript BattleUI;

    bool _IsClear;
    public bool IsClear { get => _IsClear; set => _IsClear = value; }

    bool IsDraw;
    int CurrEnergy;
    public int CurrentEnergy { get => CurrEnergy; set => CurrEnergy = value; }

    private void Awake()
    {
        m_BattelState = BattleDataState.None;

        BattleCard = new List<IBattleCard>();
        DrawEvents = new List<IDrawEvent>();

        Monsters = new List<GameObject>();

        _IsCardUsing = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeBattleState(BattleDataState state)
    {
        if (m_BattelState == state) return;
        m_BattelState = state;

        switch (state)
        {
            case BattleDataState.Init:
                m_CardData.Init();
                BattleInit();
                break;
            case BattleDataState.Battle:
                foreach (var relic in MainSceneController.Instance.PlayerData.Relics)
                {
                    relic.BattleStart();
                }
                ChangeTurn(Turn.Player);
                break;
            case BattleDataState.Win:
                MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.Win);
                MainSceneController.Instance.DispathMonster();
                foreach (var relic in MainSceneController.Instance.PlayerData.Relics)
                {
                    relic.BattleEnd();
                }
                Player.GetComponentInChildren<CharacterStat>().BattleEnd();
                ClearData();
                if (MainSceneController.Instance.CurrentNode == MapNodeType.Monster)
                {
                    Invoke("ShowRewardWindow", 0.5f);
                }
                else if (MainSceneController.Instance.CurrentNode == MapNodeType.Elite)
                {
                    Invoke("ShowRewardWindow", 0.5f);
                }
                else
                {
                    //보스전 종료시 window;
                    MainSceneController.Instance.PlayerData.Boss++;
                    MainSceneController.Instance.BGMManager.PlayBGM(4);
                    MainSceneController.Instance.Background.ChangeGameOver();
                    MainSceneController.Instance.UIControl.RemoveCurUI();
                    MainSceneController.Instance.UIControl.MakeGameOver();
                    MainSceneController.Instance.UIControl.GetCurUI().GetComponent<GameOverUI>().SetUI(true);
                    MainSceneController.Instance.UIControl.RemoveAllTagUI();
                }
                break;
            case BattleDataState.Lose:
                Player.GetComponentInChildren<CharacterStat>().BattleEnd();
                MainSceneController.Instance.Background.ChangeGameOver();
                MainSceneController.Instance.UIControl.RemoveCurUI();
                MainSceneController.Instance.UIControl.MakeGameOver();
                MainSceneController.Instance.UIControl.GetCurUI().GetComponent<GameOverUI>().SetUI(false);
                MainSceneController.Instance.UIControl.RemoveAllTagUI();
                StopAllCoroutines();
                break;
        }
    }
    void BattleInit()
    {
        m_Turn = Turn.None;
        m_TurnState = TurnState.None;
        //플레이어 캐릭터 받아서 세팅
        Player = MainSceneController.Instance.Character;
        Player.GetComponentInChildren<CharacterStat>().SetUp(MainSceneController.Instance.PlayerData);

       
        //카드 데이터 세팅
        m_CardData.Clear();
        foreach(var card in MainSceneController.Instance.PlayerData.OriginDecks)
        {
            m_CardData.Deck.Add(card);
        }

        //덱셔플
        DeckShuffle();

        //전투용 UI만들어줌
        BattleUI = MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>();
        BattleUI.UIInit(m_CardData);
        Attach(BattleUI);

        TurnCount = 0;

        //UI 배틀 시작시에 움직임
        //종료시 Battle로 상태전환
        if (IsClear)
        {
            ChangeBattleState(BattleDataState.Win);
        }
        BattleUI.StartBattle(ChangeBattleState, BattleDataState.Battle);
    }
    public void ClearData()
    {
        m_BattelState = BattleDataState.None;
         
        //만약 몹이 남아있다면 전부 제거
        if (Monsters.Count > 0)
        {
            foreach(var Monster in Monsters)
            {
                Destroy(Monster);
            }
            Monsters.Clear();
        }
        BattleCard.Clear();
        DrawEvents.Clear();
    }

    //드로우 해야하는 경우네는 얘를 호출
    public void DrawCard(int CardCount,Delvoid del)
    {
        StartCoroutine(Draw(CardCount,del));
    }
    IEnumerator Draw(int CardCount,Delvoid del)
    {
        for (int i = 0; i < CardCount; ++i)
        {
            if (m_CardData.Deck.Count == 0) {
                while(m_CardData.Discard.Count>0)
                {
                    m_CardData.Deck.Add(m_CardData.Discard[0]);
                    m_CardData.Discard.RemoveAt(0);
                }
                DeckShuffle();
            }
            //손패 10장 이상이면 뽑는 카드들은 버려짐
            if (m_CardData.Hand.Count >= 10)
            {
                m_CardData.Discard.Add(m_CardData.Deck[0]);
                m_CardData.Deck.RemoveAt(0);
            }
            //10장이하면 손패에 들어옴
            else
            {
                //손패에 들어오는 카드들은 사용가능 형태로 변경
                m_CardData.Deck[0].IsEnable = true;

                //드로우할때 작동하는 파워는 이자리에서 처리
                foreach(var power in Player.GetComponentInChildren<Stat>().Powers)
                {
                    power.DrawCard(m_CardData.Deck[0]);
                }
                DrawNotify(m_CardData.Deck[0]);

                m_CardData.Hand.Add(m_CardData.Deck[0]);
                m_CardData.Deck.RemoveAt(0);
            }
            m_CardData.Notify();
            yield return new WaitForSeconds(0.2f);
        }

        del?.Invoke();
        BattleCardNotify();
    }
    void DeckShuffle()
    {
        for (int i = 0; i < m_CardData.Deck.Count; ++i)
        {
            int rand1 = Random.Range(0, m_CardData.Deck.Count);
            int rand2;
            do
            {
                rand2 = Random.Range(0, m_CardData.Deck.Count);
            } while (rand1 == rand2);

            CardData tmp;
            tmp = m_CardData.Deck[rand1];
            m_CardData.Deck[rand1] = m_CardData.Deck[rand2];
            m_CardData.Deck[rand2] = tmp;

        }
    }

    void ChangeTurn(Turn turn)
    {
        if (m_Turn == turn) return;
        m_Turn = turn;

        if (TurnCount == 0)
        {
            //대충 카드 선천성 같은거
        }

        TurnCount++;
        switch (m_Turn)
        {
            case Turn.Player:
                MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.PlayerTurn);
                PlayerTurnProgress(TurnState.TurnBegin);
                break;
            case Turn.Enemy:
                EnemyTurnProgress(TurnState.TurnBegin);
                break;
        }
    }
    public void PlayerTurnProgress(TurnState turnstate)
    {
        if (m_TurnState == turnstate) return;
        m_TurnState = turnstate;

        switch (m_TurnState)
        {
            case TurnState.TurnBegin:
                PlayerTurnBegin();
                break;
            case TurnState.Turn:
                PlayerTurn();
                break;
            case TurnState.TurnEnd:
                PlayerTurnEnd();
                break;
        }
    }
    //플레이어 턴 시작전에 해야하는거
    void PlayerTurnBegin()
    {
        //플레이어 턴 시작전에 실행되야 파워(유물)하는 노티파이

        //몬스터 행동 결정
        SelectMonsterIntent();

        //플레이어 데이터의 매턴 회복되는 에너지 만큼 회복
        CurrEnergy = MainSceneController.Instance.PlayerData.EnergeyPerTurn;

        int PowerCount = Player.GetComponentInChildren<Stat>().Powers.Count;
        for (int i=0;i< PowerCount; ++i)
        {
            Player.GetComponentInChildren<Stat>().Powers[i].TurnBegin();
        }
        Player.GetComponentInChildren<Stat>().Powers.RemoveAll(item => item.IsEnable == false);

        BattleUI.PlayerTurnBegin();
        //카드 5장 드로우
        DrawCard(5, () => { PlayerTurnProgress(TurnState.Turn); });
    }
    void SelectMonsterIntent()
    {
        foreach (var monster in Monsters)
        {
            monster.GetComponentInChildren<IMonsterPatten>().SetIntent();
        }
    }
    //플레이어 턴에 해야하는거
    void PlayerTurn()
    {
        //UI 사용가능
        BattleUI.SetEnable(true);
        //사용 할수 있는 카드 확인
        BattleCardNotify();
        m_CardData.Notify();
    } 

    //플레이어 턴 종료시 해야하는거
    void PlayerTurnEnd()
    {
        //턴 종료시 값이 바뀌는 파워(유물)
        foreach (var power in Player.GetComponentInChildren<Stat>().Powers)
        {
            power.TurnEnd();
            
        }
        Player.GetComponentInChildren<Stat>().PowerRefresh();

        //몬스터들의 방어력 전부 초기화
        foreach (var monster in Monsters)
        {
            monster.GetComponentInChildren<Stat>().ResetDefence();
        }

        //남은 카드 버리기
        BattleCard.Clear();
        while (m_CardData.Hand.Count > 0)
        {
            m_CardData.Hand[0].IsEnable = false;
            m_CardData.Discard.Add(m_CardData.Hand[0]);
            m_CardData.Hand.RemoveAt(0);
        }
        StartCoroutine(CheckFinishPower(true));
    }
    //적 턴
    public void EnemyTurnProgress(TurnState turnstate)
    {
        if (m_TurnState == turnstate) return;
        m_TurnState = turnstate;

        switch (m_TurnState)
        {
            case TurnState.TurnBegin:
                EnemyTurnBegin();
                break;
            case TurnState.Turn:
                EnemyTurn();
                break;
            case TurnState.TurnEnd:
                EnemyTurnEnd();
                break;
        }
    }
    //적 턴시작시 처리
    void EnemyTurnBegin()
    {
        //몬스터들 턴시작시 파워
        foreach (var Monster in Monsters)
        {
            for(int i=0;i<Monster.GetComponentInChildren<Stat>().Powers.Count;++i)
            {
                Monster.GetComponentInChildren<Stat>().Powers[i].TurnBegin();
            }
            Monster.GetComponentInChildren<Stat>().Powers.RemoveAll(item => item.IsEnable == false);
        }

        EnemyTurnProgress(TurnState.Turn);
    }
    void EnemyTurn()
    {
        //패턴에 따른 몬스터의 행동
        StartCoroutine(MonsterAction());
    }
    IEnumerator MonsterAction()
    {
        int tmp = Monsters.Count;
        for (int i = 0; i < tmp; ++i)
        {
            StartCoroutine(Monsters[i].GetComponentInChildren<IMonsterPatten>().Action());
            while (!Monsters[i].GetComponentInChildren<IMonsterPatten>().GetAttackEnd())
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.5f);
        Monsters.RemoveAll(item => item == null);
        EnemyTurnProgress(TurnState.TurnEnd);
    }
    IEnumerator CheckFinishPower(bool IsPlayer)
    {
        if (IsPlayer)
        {
            while (Player.GetComponentInChildren<Stat>().FinishList.Count > 0)
            {
                Player.GetComponentInChildren<Stat>().FinishList.RemoveAll(item => item == null);
                yield return null;
            }
            ChangeTurn(Turn.Enemy);
        }
        else
        {
            foreach (var Monster in Monsters)
            {
                while (Monster.GetComponentInChildren<Stat>().FinishList.Count > 0)
                {
                    Monster.GetComponentInChildren<Stat>().FinishList.RemoveAll(item => item == null);
                    yield return null;
                }
            }
            ChangeTurn(Turn.Player);
        }
    }
    void EnemyTurnEnd()
    {
        //턴 종료시 처리 되야 될것
        foreach (var Monster in Monsters)
        {
            foreach (var power in Monster.GetComponentInChildren<Stat>().Powers)
            {
                power.TurnEnd();
            }
            Monster.GetComponentInChildren<Stat>().PowerRefresh();
        }

        //플레이어 쉴드 리셋
        Player.GetComponentInChildren<Stat>().ResetDefence();

        StartCoroutine(CheckFinishPower(false));
    }
    public void AllEnemyTargetOn()
    {
        foreach(var Monster in Monsters)
        {
            Monster.GetComponentInChildren<Stat>().TargetOn();
        }
        
    }
    public void EnemyTargetOn()
    {
        foreach (var Monster in Monsters)
        {
            Monster.GetComponentInChildren<Stat>().SetIsEnableTarget(true);

        }
    }
    public void NoTargetTargetOn()
    {
        Player.GetComponentInChildren<Stat>().TargetOn();
        
    }
    public void TargetOff()
    {
        foreach (var Monster in Monsters)
        {
            Monster.GetComponentInChildren<Stat>().TargetOff();
            Monster.GetComponentInChildren<Stat>().SetIsEnableTarget(false);
        }
        Player.GetComponentInChildren<Stat>().TargetOff();
    }

    public void UseCard(int num)
    {
        if (m_CardData.Hand[num].CardType == CardType.Attack)
        {
            Player.GetComponentInChildren<CharacterStat>().Attack();
        }
        //카드 효과
        StartCoroutine(m_CardData.Hand[num].OnExcute());

        //에너지랑 카드 이동
        CurrEnergy -= m_CardData.Hand[num].Cost;

        foreach (var power in Player.GetComponentInChildren<Stat>().Powers)
        {
            power.CardUse(m_CardData.Hand[num]);
        }
        foreach (var monster in Monsters)
        {
            int tmp = monster.GetComponentInChildren<Stat>().Powers.Count;
            for (int i = 0; i < tmp; ++i)
            {
                monster.GetComponentInChildren<Stat>().Powers[i].CardUse(m_CardData.Hand[num]);
            }
        }

        if (m_CardData.Hand[num].IsExtinct)
        {
            foreach (var power in Player.GetComponentInChildren<Stat>().Powers)
            {
                power.CardExtinct(m_CardData.Hand[num]);
            }
            m_CardData.Hand[0].IsEnable = false;
            m_CardData.Extinction.Add(m_CardData.Hand[num]);
            m_CardData.Hand.RemoveAt(num);
        }
        else if(m_CardData.Hand[num].CardType == CardType.Power)
        {
            m_CardData.Hand[0].IsEnable = false;
            m_CardData.Hand.RemoveAt(num);
        }
        else
        {
            m_CardData.Hand[0].IsEnable = false;
            m_CardData.Discard.Add(m_CardData.Hand[num]);
            m_CardData.Hand.RemoveAt(num);
        }
        BattleUI.UpdateData();

        
        //사용할 수 있는 카드 확인
        BattleCardNotify();
        //덱 갯수랑 상황 변경점 알림
        m_CardData.Notify();
    }

    //몬스터 살아있는지 체크
    public void CheckMonstersLive(GameObject Monster) {
        if (Monsters.Contains(Monster))
        {
            Monsters.Remove(Monster);
        }
        if (Monsters.Count <= 0)
        {
            Invoke("Victory",0.5f);
        }
    }
    /*
     IBattleCard 옵저버 패턴 관련
     */
    public void Attach(IBattleCard card)
    {
        BattleCard.Add(card);
    }

    public void Detach(IBattleCard card)
    {
        BattleCard.Remove(card);
    }
    public void BattleCardNotify()
    {
        if (BattleCard.Count > 0)
        {
            foreach (var card in BattleCard)
            {
                card.OnEffectOn(CurrEnergy);
            }
        }
    }

    /*
    IDrawEvent 옵저버 패턴 관련
    */

    public void Attach(IDrawEvent observer)
    {
        DrawEvents.Add(observer);
    }

    public void Detach(IDrawEvent observer)
    {
        DrawEvents.Remove(observer);
    }
    public void DrawNotify(CardData data)
    {
        if (DrawEvents.Count > 0)
        {
            foreach (var observer in DrawEvents)
            {
                observer.DrawCard(data);
            }
        }
    }

    void ShowRewardWindow()
    {
        MainSceneController.Instance.UIControl.RemoveCurUI();
        MainSceneController.Instance.UIControl.MakeReward();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    void Victory()
    {
        ChangeBattleState(BattleDataState.Win);
    }
}
