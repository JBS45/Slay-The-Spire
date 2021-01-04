using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroFloorEvent : MonoBehaviour
{
    public GameObject ButtonRes;

    List<GameObject> m_Buttons;
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

        m_Buttons[0].GetComponent<MainSceneEventButton>().ButtonSetting("[1번]", 
            ()=> 
            {
                Debug.Log("1번");
                DestroyButton();
                MakeButton();
                m_Buttons[0].GetComponent<MainSceneEventButton>().ButtonSetting("[떠난다]", Finish);
            });
        m_Buttons[1].GetComponent<MainSceneEventButton>().ButtonSetting("[2번]",
            () =>
            {
                Debug.Log("1번");
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

        //임시
        MainSceneController.Instance.EventStateChange(MainSceneController.EventState.Battle);
        Destroy(this.gameObject);
    }
}
