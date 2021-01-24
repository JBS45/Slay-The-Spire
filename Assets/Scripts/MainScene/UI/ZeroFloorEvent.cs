using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroFloorEvent : MonoBehaviour
{
    public GameObject ButtonRes;

    List<GameObject> m_Buttons;

    [SerializeField]
    Relic Lament;
    private void Awake()
    {
        m_Buttons = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(ButtonRes);
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<MainSceneEventButton>().ButtonSetting("[대화한다]", SelectBless);
        m_Buttons.Add(obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectBless()
    {
        foreach(var button in m_Buttons)
        {
            Destroy(button);
        }
        m_Buttons.Clear();
        for (int i = 0; i < 2; ++i)
        {
            MakeButton();
        }
        int tmpHp = MainSceneController.Instance.PlayerData.MaxHp / 10;
        int MaxHp = MainSceneController.Instance.PlayerData.MaxHp;
        m_Buttons[0].GetComponent<MainSceneEventButton>().ButtonSetting("[최대 체력을 "+ tmpHp+" 을 추가로 얻습니다.]", 
            ()=> 
            {
                MainSceneController.Instance.Character.GetComponentInChildren<Stat>().MaxHealthPoint = MaxHp + tmpHp;
                MainSceneController.Instance.Character.GetComponentInChildren<Stat>().CurrentHealthPoint = MaxHp + tmpHp;
                MainSceneController.Instance.PlayerData.Notify();
                DestroyButton();
                MakeButton();
                m_Buttons[0].GetComponent<MainSceneEventButton>().ButtonSetting("[떠난다]", Finish);
            });
        m_Buttons[1].GetComponent<MainSceneEventButton>().ButtonSetting("[다음 세번의 전투를 적의 체력이 1인채로 시작합니다.]",
            () =>
            {
                MainSceneController.Instance.PlayerData.AddRelic(Lament);
                DestroyButton();
                MakeButton();
                m_Buttons[0].GetComponent<MainSceneEventButton>().ButtonSetting("[떠난다]", Finish);
            });
    }

    void DestroyButton()
    {
        foreach (var button in m_Buttons)
        {
            Destroy(button);
        }
        m_Buttons.Clear();
    }

    void MakeButton()
    {
        GameObject obj = Instantiate(ButtonRes);
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        m_Buttons.Add(obj);
    }

    void Finish()
    {
        //원래는 MainSceneUI에게 맵UI를 보여주게 해야한다.
        MainSceneController.Instance.GetUIControl().OpenMapProgress();
        StartCoroutine(MainSceneController.Instance.GetUIControl().GetMapUI().GetComponent<MapUIScript>().FirstMapOpen());

    }
}
