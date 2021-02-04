using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    static TitleSceneController _instance = null;
    public static TitleSceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TitleSceneController>();
            }
            return _instance;
        }
    }
    public Canvas MainCanvas;

    public enum TitleSceneState
    {
        None=0,Create, BeginAnimation, Ready, CharSelectPanel
    }

    public enum ScreenUIState
    {
        Main,SelectChar
    }

    public GameObject m_MainTitle;
    public GameObject m_TowerSprite;
    public GameObject m_FrontCloudSpawner;
    public GameObject m_BackCloudSpawner;

    public GameObject TitleString;
    public GameObject ButtonPanel;

    public Image FadePanel;
    public GameObject SelectPanelRes;

    bool IsSave = false;

    [SerializeField]
    BGMManager m_BGMManager;
    [SerializeField]
    SEManager m_SEManager;

    TitleSceneState m_State=TitleSceneState.None;

    Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(TitleSceneState.Create);
    }

    // Update is called once per frame
    void Update()
    {
        StateOperate();
    }

    void ChangeState(TitleSceneState state)
    {
        if (m_State == state) return;
        m_State = state;

        switch (m_State)
        {
            case TitleSceneState.Create:
                SaveDataStruct tmp = new SaveDataStruct();
                IsSave = SaveLoadManager.Instance.Load(ref tmp) ? tmp.IsSave : false;
                MakePanelButton(IsSave);
                ChangeState(TitleSceneState.BeginAnimation);
                break;
            case TitleSceneState.BeginAnimation:
                StartCoroutine("TitleBeginMoveTowerImage");
                StartCoroutine(m_FrontCloudSpawner.GetComponent<CloudSpawner>().StartSpawnCloud());
                StartCoroutine(m_BackCloudSpawner.GetComponent<CloudSpawner>().StartSpawnCloud());
                break;
            case TitleSceneState.Ready:
                if (!m_BGMManager.IsPlay())
                {
                    m_BGMManager.PlayBGM(0);
                }
                TitleString.SetActive(true);
                StartCoroutine(MoveToTarget(ButtonPanel, new Vector3(550, 0, 0), null));
                break;
            case TitleSceneState.CharSelectPanel:
                TitleString.SetActive(false);
                StartCoroutine(MoveToTarget(ButtonPanel, new Vector3(-550, 0, 0), null));
                break;
        }
    }
    void StateOperate()
    {
        switch (m_State)
        {
            case TitleSceneState.Create:
                break;
            case TitleSceneState.BeginAnimation:
                if (Input.anyKeyDown)
                {
                    StopCoroutine("TitleBeginMoveTowerImage");
                    m_TowerSprite.GetComponent<RectTransform>().localPosition = TargetPos;
                    ChangeState(TitleSceneState.Ready);
                }
                break;
            case TitleSceneState.Ready:
                break;
            case TitleSceneState.CharSelectPanel:
                break;
        }
    }

    void MakePanelButton(bool IsSave)
    {
        ButtonPanel.GetComponent<ButtonPanel>().Clear();
        if (IsSave)
        {
            ButtonPanel.GetComponent<ButtonPanel>().ButtonSetting("Continue", SceneChange);
            ButtonPanel.GetComponent<ButtonPanel>().ButtonSetting("New Game", NewGame);
        }
        else
        {
            ButtonPanel.GetComponent<ButtonPanel>().ButtonSetting("Start", PushStartButton);
        }
        ButtonPanel.GetComponent<ButtonPanel>().ButtonSetting("Option", PushOptionButton);
        ButtonPanel.GetComponent<ButtonPanel>().ButtonSetting("Exit", PushQuitButton);
    }

    //버튼 함수 구역
    public void PushStartButton()
    {
        m_SEManager.PlaySE(1);
        //셀렉트 메뉴 패널 초기화
        GameObject obj = Instantiate(SelectPanelRes);
        obj.transform.SetParent(MainCanvas.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        obj.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        obj.GetComponent<SelectPanelScript>().InitMenu();

        //캔슬버튼에 상태를 원래상태로 돌리는 이벤트 추가
        obj.GetComponent<SelectPanelScript>().CancelButton.GetComponentInChildren<Button>().onClick.AddListener(() => { ChangeState(TitleSceneState.Ready); m_SEManager.PlaySE(1); });

        ChangeState(TitleSceneState.CharSelectPanel);
    }
    public void NewGame()
    {
        SaveLoadManager.Instance.Delete();
        MakePanelButton(false);
    }
    public void PushOptionButton()
    {
        m_SEManager.PlaySE(1);
    }
    public void PushQuitButton()
    {
        m_SEManager.PlaySE(1);
        Application.Quit();
    }

    public void SceneChange()
    {
        StartCoroutine(FadeOut(()=> { SceneManager.LoadScene("MainScene"); }));

    }

    IEnumerator TitleBeginMoveTowerImage()
    {
        //타겟 좌표를 설정
        TargetPos = new Vector3(m_TowerSprite.GetComponent<RectTransform>().localPosition.x,
            m_TowerSprite.GetComponent<RectTransform>().localPosition.y - 200.0f,
            m_TowerSprite.GetComponent<RectTransform>().localPosition.z);

        //타워 이미지의 위치가 목표 위치와의 거리가 1보다 클 경우 반복한다.
        while (Vector3.Distance(m_TowerSprite.GetComponent<RectTransform>().localPosition, TargetPos) >= 1.0f)
        {
            m_TowerSprite.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(m_TowerSprite.GetComponent<RectTransform>().localPosition, TargetPos, 0.5f + (25 * Time.deltaTime));
            yield return null;
        }

        //마지막으로 타워 이미지의 위치를 타겟포스로 맞춰준다.
        m_TowerSprite.GetComponent<RectTransform>().localPosition = TargetPos;
        ChangeState(TitleSceneState.Ready);
    }

    IEnumerator MoveToTarget(GameObject obj,Vector3 distance,Delvoid del)
    {
        Vector3 Target= obj.transform.localPosition + distance;
        while (Vector3.Distance(obj.transform.localPosition, Target) >= 1.0f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, Target, 30.0f);
            yield return null;
        }
        obj.transform.localPosition = Target;
        if (del != null)
        {
            del();
        }
    }

    IEnumerator FadeOut(Delvoid Del)
    {
        FadePanel.transform.SetAsLastSibling();
        float alpha = FadePanel.color.a;
        while (alpha<=0.99f)
        {
            alpha += Time.deltaTime*3.0f;
            FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, alpha);
            yield return null;
        }
        if (Del != null)
        {
            Del();
        }
    }
}
