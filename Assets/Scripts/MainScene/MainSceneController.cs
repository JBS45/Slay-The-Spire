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
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainSceneController>();
            }
            return _instance;
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

    [SerializeField]
    PlayerDataAsset m_playerData;
    public PlayerDataAsset PlayerData
    {
        get { return m_playerData; }
    }

    private void Awake()
    {
        GameDataInit();
        m_Char = Instantiate(CharDB.Instance.GetCharacterAsset().Prefab);
        m_Char.transform.SetParent(m_CharSpawnPoint);
        m_Char.transform.localPosition = Vector3.zero;
        m_Char.GetComponentInChildren<CharacterStat>().SetUp(PlayerData);
        m_UIControl.InfoBar.BarInit(PlayerData);
    }
    // Start is called before the first frame update
    void Start()
    {
        //저장된 정보가 있으면 그 층의 정보를 기반으로 로드

        //그게 아닌 경우
        m_MapControl.MapGenerate();
        m_UIControl.MakeMap();
        m_MapControl.DrawMap(m_UIControl.GetMapUI());
        m_UIControl.GetMapUI().GetComponent<MapUIScript>().HideMap();

        ChangeState(MapNodeType.None);
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    void ChangeState(MapNodeType state)
    {
        m_State = state;
        m_BattleData.ClearData();
        m_Reward.Clear();
        m_UIControl.RemoveCurUI();
        m_UIControl.OffMap();
        Random.InitState(m_MapControl.GetMapNode(PlayerData.CurrentFloor, PlayerData.CurrentFloorIndex).Seed);

        switch (m_State)
        {
            case MapNodeType.None:
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.NPCSpawn(NPCType.Neow);
                m_UIControl.ZeroFloorUI();
                m_UIControl.MakeToolTip();
                m_Char.GetComponentInChildren<CharacterStat>().HIdeUI();
                break;
            case MapNodeType.Merchant:
                m_Reward.ShopReward();
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.NPCSpawn(NPCType.Merchant);
                m_UIControl.MakeShopWindow();
                m_UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetClassCard(Reward.CardList);
                m_UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetNeutralCard(Reward.Neutral);
                m_BattleData.Monsters[0].GetComponent<Merchant>().SetShowWindow(m_UIControl.GetCurUI().GetComponent<ShopWindowScript>());
                break;
            case MapNodeType.Mistery:
                MysterySelect();
                break;
            case MapNodeType.Monster:
                m_Reward.NormalReward(PlayerData.CharType, 3);
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.MonsterSpawn();
                m_UIControl.MakeBattleUI();
                m_BattleData.ChangeBattleState(BattleDataState.Init);
                break;
            case MapNodeType.Elite:
                m_Reward.EliteReward(PlayerData.CharType, 3);
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.EliteSpawn();
                m_UIControl.MakeBattleUI();
                m_BattleData.ChangeBattleState(BattleDataState.Init);
                break;
            case MapNodeType.Boss:
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
                m_Spawner.BossSpawn(Boss);
                m_UIControl.MakeBattleUI();
                m_BattleData.ChangeBattleState(BattleDataState.Init);
                break;
            case MapNodeType.Rest:
                m_BackgroundControl.BackgroundChange(BackgroundType.FireCamp);
                m_UIControl.MakeFireCampWindow();
                break;
            case MapNodeType.Treasure:
                m_Reward.TreasureReward();
                m_Spawner.NPCSpawn(NPCType.Chest);
                m_BackgroundControl.BackgroundChange(BackgroundType.Battle);
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
        //저장 정보가 없으면
        m_playerData.CharType = CharDB.Instance.GetPlayChar();
        m_playerData.MaxHp = CharDB.Instance.GetCharacterAsset().Hp;
        m_playerData.CurrentHp = CharDB.Instance.GetCharacterAsset().Hp;
        m_playerData.CurrentMoney = CharDB.Instance.GetCharacterAsset().Gold;
        m_playerData.CurrentFloor = 0;
        m_playerData.CurrentFloorIndex = 0;
        m_playerData.MaxPotions = 3;
        m_playerData.DrawPerTurn = 5;
        m_playerData.EnergeyPerTurn = 3;
        m_playerData.CardRemoveCount = 0;

        //미스터리 이벤트
        MysteryDB.Instance.NoSaveInit();

        //유물 DB 초기화
        RelicDB.Instance.NoSaveInit();

        //유물 기본
        if (m_playerData.Relics == null)
        {
            m_playerData.Relics = new List<RelicData>();
        }
        m_playerData.Relics.Clear();

        m_playerData.AddRelic(CharDB.Instance.GetCharacterAsset().StartRelic);

        if (m_playerData.OriginDecks == null)
        {
            m_playerData.OriginDecks = new List<CardData>();
        }

        m_playerData.OriginDecks.Clear();

        for (int i = 0; i < CharDB.Instance.GetCharacterAsset().StartDeck.Length; ++i)
        {
            CardData Tmp = new CardData(CharDB.Instance.GetCharacterAsset().StartDeck[i]);
            m_playerData.OriginDecks.Add(Tmp);
        }
        m_playerData.Notify();
        //포션 없음

        //저장 정보가 있으면
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
                m_UIControl.GetCurUI().GetComponent<MysteryEventWindow>().SetWindow(MysterySelector());
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
}
