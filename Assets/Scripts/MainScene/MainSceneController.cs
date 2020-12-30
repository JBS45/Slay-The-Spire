using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public MainSceneUIController m_UIContol;
    public BackgroundScript m_BackgroundControl;

    public PlayerInfo m_PlayerInfo;

    GameObject m_Char;
    public Transform m_CharSpawnPoint;

    public GameEvent m_Event;

    public enum MainSceneState
    {
        None,Init,Ready
    }

    MainSceneState m_State;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(MainSceneState.Init);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(MainSceneState state)
    {
        if (m_State == state) return;
        m_State = state;
        switch (m_State)
        {
            case MainSceneState.Init:
                m_Event = GameEvent.Instance;
                m_PlayerInfo.GameDataInit();
                m_Char = Instantiate(CharDB.Instance.GetCharacterAsset().Prefab);
                m_Char.transform.SetParent(m_CharSpawnPoint);
                m_UIContol.InfoBar.BarInit(CharDB.Instance.GetCharacterAsset().CharacterName, m_PlayerInfo.GetMaxPotions(),m_PlayerInfo.GetCurHp(),m_PlayerInfo.GetMaxHp(),m_PlayerInfo.GetCurrentMoney(),m_PlayerInfo.GetCurFloor(), m_PlayerInfo.OriginDeck.Count);
                ChangeState(MainSceneState.Ready);
                break;
            case MainSceneState.Ready:
                m_Event.ChangeEventState(GameEvent.EventType.Neow);
                m_BackgroundControl.WallChange();
                break;
        }
    }
}
