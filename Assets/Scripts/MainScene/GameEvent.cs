using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    static GameEvent _instance = null;
    public static GameEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameEvent>();
            }
            return _instance;
        }
    }

    public enum EventType
    {
        None,Neow,Trade,Random,Battle,EliteBattle,BossBattle
    }

    EventType SelectEvent = EventType.None;

    public GameObject NeowRes;
    public GameObject MerchantRes;
    public MainSceneUIController m_UIControl;

    List<GameObject> m_EventObj;

    private void Awake()
    {
        m_EventObj = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeState(EventType eventType){
        SelectEvent = eventType;
        if (m_EventObj.Count > 0)
        {
            foreach (var obj in m_EventObj)
            {
                Destroy(obj);
            }
            m_EventObj.Clear();
        }
        switch (SelectEvent)
        {
            case EventType.Neow:
                m_EventObj.Add(Instantiate(NeowRes));
                m_EventObj[0].transform.localPosition = new Vector3(11.5f, -5.0f, 0.0f);
                m_UIControl.ZeroFloorUI(ref m_EventObj);
                break;
            case EventType.Trade:
                break;
            case EventType.Random:
                break;
            case EventType.Battle:
                break;
            case EventType.EliteBattle:
                break;
            case EventType.BossBattle:
                break;
        }
    }
    public void ChangeEventState(EventType eventType)
    {
        m_UIControl.FadeOutEffect(
            () =>
            {
                ChangeState(eventType);
                m_UIControl.FadeInEffect(() => {; });
            });
    }
}
