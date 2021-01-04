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
    public MainSceneUIController m_UIControl;
    public BackgroundScript m_BackgroundControl;

    public PlayerInfo m_PlayerInfo;
    public MonsterSpawner m_Spawner;
    public BattleData m_BattleData;

    GameObject m_Char;
    public Transform m_CharSpawnPoint;

    public enum EventState
    {
        None, Neow, Trade, Random, Battle, EliteBattle, BossBattle
    }

    EventState m_State;

    private void Awake()
    {
        m_PlayerInfo.GameDataInit();
        m_Char = Instantiate(CharDB.Instance.GetCharacterAsset().Prefab);
        m_Char.transform.SetParent(m_CharSpawnPoint);
        m_UIControl.InfoBar.BarInit(CharDB.Instance.GetCharacterAsset().CharacterName, m_PlayerInfo.GetMaxPotions(), m_PlayerInfo.GetCurHp(), m_PlayerInfo.GetMaxHp(), m_PlayerInfo.GetCurrentMoney(), m_PlayerInfo.GetCurFloor(), m_PlayerInfo.GetOriginDeck().Count);
    }
    // Start is called before the first frame update
    void Start()
    {
        //저장된 정보가 있으면 그 층의 정보를 기반으로 로드

        //그게 아닌 경우
        m_PlayerInfo.GameDataInit();
        ChangeState(EventState.Neow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeState(EventState state)
    {
        if (m_State == state) return;
        m_State = state;
        m_BackgroundControl.WallChange();
        m_BattleData.ClearData();
        switch (m_State)
        {
            case EventState.Neow:
                m_Spawner.MonsterSpawn(MonsterSpawner.SpawnMonsterType.Neow);
                m_UIControl.ZeroFloorUI();
                break;
            case EventState.Trade:
                break;
            case EventState.Random:
                break;
            case EventState.Battle:
                m_UIControl.MakeBattleUI();
                m_BattleData.BattleInit();
                break;
            case EventState.EliteBattle:
                break;
            case EventState.BossBattle:
                break;
        }
    }

    
    public void EventStateChange(EventState state)
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
}
