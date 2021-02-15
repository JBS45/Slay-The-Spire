using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    static MainSceneController _instance = null;
    public static MainSceneController Instance
    {
        get
        {
             return _instance = FindObjectOfType<MainSceneController>() ?? _instance;
        }
    }
    [SerializeField]
    MainSceneUIController m_UIControl;
    public MainSceneUIController UIControl
    {
        get { return m_UIControl; }
    }
    [SerializeField]
    BackgroundScript m_BackgroundControl;
    public BackgroundScript Background
    {
        get => m_BackgroundControl;
    }
    [SerializeField]
    MapGenerator m_MapControl;
    BossType _Boss;
    public BossType Boss { get => _Boss; set => _Boss = value; }

    [SerializeField]
    MonsterSpawner m_Spawner;
    public MonsterSpawner Spawner { get => m_Spawner; set => m_Spawner = value; }
    [SerializeField]
    BattleData m_BattleData;
    public BattleData BattleData
    {
        get { return m_BattleData; }
    }

    [SerializeField]
    RewardManager m_Reward;
    public RewardManager Reward
    {
        get { return m_Reward; }
    }

    GameObject m_Char;

    public GameObject Character
    {
        get { return m_Char; }
    }
    public Transform m_CharSpawnPoint;

    
    MapNodeType m_State;
    public MapNodeType CurrentNode { get => m_State; }
    int MapSeed;

    [SerializeField]
    PlayerDataAsset m_playerData;
    public PlayerDataAsset PlayerData
    {
        get { return m_playerData; }
    }

    [SerializeField]
    SEManager _SEManager;
    public SEManager SEManager { get => _SEManager; }
    [SerializeField]
    SEManager _AtkSEManager;
    public SEManager AtkSEManager { get => _AtkSEManager; }
    [SerializeField]
    BGMManager _BGMManager;
    public BGMManager BGMManager { get => _BGMManager; }

    SaveDataStruct SaveData;

    MapNodeType IsPreNodeMytery;
    string Mysterystring;

    private void Awake()
    {
        GameDataInit();
        m_Char = Instantiate(CharDB.Instance.GetCharacterAsset(m_playerData.CharType).Prefab);
        m_Char.transform.SetParent(m_CharSpawnPoint);
        m_Char.transform.localPosition = Vector3.zero;
        m_UIControl.InfoBar.BarInit(PlayerData);
        IsPreNodeMytery = MapNodeType.None;
    }
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(m_playerData.Seed);
        m_MapControl.MapGenerate();
        m_UIControl.MakeMap();
        m_UIControl.MakeToolTip();
        m_MapControl.DrawMap(m_UIControl.GetMapUI());
        m_UIControl.GetMapUI().GetComponent<MapUIScript>().HideMap();

        //저장된 정보가 있으면 그 층의 정보를 기반으로 로드
        if (SaveData.IsSave)
        {
            int last = SaveData.Path.Count-1;
            for (int i = 1; i < last; ++i)
            {
                m_UIControl.GetMapUI().GetComponent<MapUIScript>().ClearStage(i, SaveData.Path[i]);
            }
            m_UIControl.GetMapUI().GetComponent<MapUIScript>().SaveStage(last, SaveData.Path[last]);
        }
        //그게 아닌 경우
        else
        {
            ChangeState(MapNodeType.None);
        }
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    void ChangeState(MapNodeType state)
    {
        IsPreNodeMytery = m_State;
        m_State = state;
        m_BattleData.ClearData();
        m_Reward.Clear();
        m_UIControl.RemoveCurUI();
        m_UIControl.OffMap();
        Random.InitState(m_MapControl.GetMapNode(PlayerData.CurrentFloor, PlayerData.CurrentFloorIndex).Seed);
        if (IsPreNodeMytery == MapNodeType.Mistery&& Mysterystring!="")
        {
            PlayerData.PastEvents.Add(Mysterystring);
        }
        else if(IsPreNodeMytery == MapNodeType.Monster)
        {
            PlayerData.Monster++;
        }
        else if(IsPreNodeMytery == MapNodeType.Elite)
        {
            PlayerData.Elite++;
        }

        PlayerData.Notify();
        BGMManager.PlayBGM(1);
        
        switch (m_State)
        {
            case MapNodeType.None:
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.NPCSpawn(NPCType.Neow);
                m_UIControl.ZeroFloorUI();
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
            case MapNodeType.Merchant:
                m_Reward.ShopReward();
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.NPCSpawn(NPCType.Merchant);
                m_UIControl.MakeShopWindow();
                m_UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetClassCard(Reward.CardList);
                m_UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetNeutralCard(Reward.Neutral);
                m_BattleData.Monsters[0].GetComponent<Merchant>().SetShowWindow(m_UIControl.GetCurUI().GetComponent<ShopWindowScript>());
                BGMManager.PlayBGM(2);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
            case MapNodeType.Mistery:
                MysterySelect();
                break;
            case MapNodeType.Monster:
                m_Reward.NormalReward(PlayerData.CharType, 3);
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.MonsterSpawn();
                m_UIControl.MakeBattleUI();
                m_BattleData.IsClear = SaveData.IsClear;
                m_BattleData.ChangeBattleState(BattleDataState.Init);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
            case MapNodeType.Elite:
                m_Reward.EliteReward(PlayerData.CharType, 3);
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.EliteSpawn();
                m_UIControl.MakeBattleUI();
                m_BattleData.IsClear = SaveData.IsClear;
                m_BattleData.ChangeBattleState(BattleDataState.Init);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
            case MapNodeType.Boss:
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.BossSpawn(Boss);
                m_UIControl.MakeBattleUI();
                m_BattleData.IsClear = SaveData.IsClear;
                m_BattleData.ChangeBattleState(BattleDataState.Init);
                BGMManager.PlayBGM(3);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
            case MapNodeType.Rest:
                m_BackgroundControl.BackgroundChange(BackgroundType.FireCamp);
                m_UIControl.MakeFireCampWindow();
                BGMManager.PlayBGM(5);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
            case MapNodeType.Treasure:
                m_Reward.TreasureReward();
                m_Spawner.NPCSpawn(NPCType.Chest);
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);
                break;
        }
    }

    
    public void EventStateChange(MapNodeType state)
    {
        m_UIControl.FadeOutEffect(
            () =>
            {
                ChangeState(state);
                m_UIControl.FadeInEffect(() => {; });
            });
    }
    public MainSceneUIController GetUIControl()
    {
        return m_UIControl;
    }
    public MapGenerator GetMapControl()
    {
        return m_MapControl;
    }

    void GameDataInit()
    {
        if (SaveLoadManager.Instance.Load(ref SaveData))
        {
            //저장 정보가 있으면

            m_playerData.CharType = SaveData.Character;
            m_playerData.MaxHp = SaveData.MaxHP;
            m_playerData.CurrentHp = SaveData.CurHP;
            m_playerData.CurrentMoney = SaveData.CurMoney;
            m_playerData.CurrentFloor = SaveData.CurFloor;
            m_playerData.CurrentFloorIndex = SaveData.CurFloorIndex;
            m_playerData.MaxPotions = 0;
            m_playerData.DrawPerTurn = 5;
            m_playerData.EnergeyPerTurn = 3;
            m_playerData.CardRemoveCount = SaveData.CardRemove;

            m_playerData.Monster = SaveData.Monster;
            m_playerData.Elite = SaveData.Elite;
            m_playerData.Boss = SaveData.Boss;

            //미스터리 이벤트
            MysteryDB.Instance.NoSaveInit();
            MysteryDB.Instance.SaveInit(SaveData.PastEvents);

            //유물 DB 초기화
            RelicDB.Instance.Init();

            m_playerData.Clear();
            //유물 기본
            if (m_playerData.Relics == null)
            {
                m_playerData.Relics = new List<RelicData>();
            }

            foreach (var item in SaveData.Relic)
            {
                m_playerData.AddRelic(item.GetRelic());
                RelicDB.Instance.RemoveData(item.GetRelic());
            }

            if (m_playerData.OriginDecks == null)
            {
                m_playerData.OriginDecks = new List<CardData>();
            }

            foreach(var item in SaveData.Card)
            {
                m_playerData.OriginDecks.Add(item.GetCardData());
            }

            m_playerData.Seed = SaveData.MapGenrateSeed;

            m_playerData.Notify();
        }
        else
        {
            SaveData = new SaveDataStruct();
            //저장 정보가 없으면
            m_playerData.CharType = CharDB.Instance.GetPlayChar();
            m_playerData.MaxHp = CharDB.Instance.GetCharacterAsset().Hp;
            m_playerData.CurrentHp = CharDB.Instance.GetCharacterAsset().Hp;
            m_playerData.CurrentMoney = CharDB.Instance.GetCharacterAsset().Gold;
            m_playerData.CurrentFloor = 0;
            m_playerData.CurrentFloorIndex = 0;
            m_playerData.MaxPotions = 0;
            m_playerData.DrawPerTurn = 5;
            m_playerData.EnergeyPerTurn = 3;
            m_playerData.CardRemoveCount = 0;

            m_playerData.Monster = 0;
            m_playerData.Elite = 0;
            m_playerData.Boss = 0;


            //미스터리 이벤트
            MysteryDB.Instance.NoSaveInit();

            //유물 DB 초기화
            RelicDB.Instance.Init();

            m_playerData.Clear();
            //유물 기본
            if (m_playerData.Relics == null)
            {
                m_playerData.Relics = new List<RelicData>();
            }

            m_playerData.AddRelic(CharDB.Instance.GetCharacterAsset().StartRelic);

            if (m_playerData.OriginDecks == null)
            {
                m_playerData.OriginDecks = new List<CardData>();
            }


            for (int i = 0; i < CharDB.Instance.GetCharacterAsset().StartDeck.Length; ++i)
            {
                CardData Tmp = new CardData(CharDB.Instance.GetCharacterAsset().StartDeck[i]);
                m_playerData.OriginDecks.Add(Tmp);
            }

            MapSeed = Random.Range(0, int.MaxValue);
            m_playerData.Seed = MapSeed;


            m_playerData.Notify();

            SaveData.Save(m_playerData);
            SaveLoadManager.Instance.Save(SaveData);
            //포션 없음

        }
        
    }
    void MysterySelect()
    {
        if (MysteryDB.Instance.CurrentKey.Count > 0)
        {
            int rand = Random.Range(0, 100);
            if (rand <= 15)
            {
                ChangeState(MapNodeType.Monster);
            }
            else if(rand>=16&&rand<30){
                ChangeState(MapNodeType.Merchant);
            }
            else
            {
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_UIControl.MakeEventWindow();
                Mysterystring = MysterySelector();
                m_UIControl.GetCurUI().GetComponent<MysteryEventWindow>().SetWindow(Mysterystring);
                SaveData.Save(m_playerData);
                SaveLoadManager.Instance.Save(SaveData);

            }
        }
        else
        {
            int rand = Random.Range(0, 100);
            if (rand <= 50)
            {
                ChangeState(MapNodeType.Monster);
            }
            else
            {
                ChangeState(MapNodeType.Merchant);
            }
        }
    }
    string MysterySelector()
    {
        return MysteryDB.Instance.RandomSelector();
    }
    public void Save(bool IsClear)
    {
        SaveData.Save(m_playerData, IsClear);
        SaveLoadManager.Instance.Save(SaveData);
    }
    public void DispathMonster()
    {
        for (int i = 0; i < SaveData.Path.Count - 1; ++i)
        {
            MapNodeType tmp = m_MapControl.GetMapNodeType(i, SaveData.Path[i]);
            if (tmp == MapNodeType.Monster)
            {
                m_playerData.Monster++;
            }
            else if (tmp == MapNodeType.Elite)
            {
                m_playerData.Elite++;
            }
            else if (tmp == MapNodeType.Boss)
            {
                m_playerData.Boss++;
            }
        }
    }
}
