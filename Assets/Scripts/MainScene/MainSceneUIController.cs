using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUIController : MonoBehaviour
{

    public Canvas UICanvas;
    public Image FadePanel;
    public BarScript InfoBar;
    public GameObject ZeroPanel;
    public GameObject BattleUI;

    [SerializeField]
    Transform RelicBar;
    public Transform RelicBarPos { get => RelicBar; }

    GameObject CurUI;
    GameObject PastUI;

    [SerializeField]
    GameObject MapRes;
    GameObject Map;
    bool IsMapOpen = false;

    [SerializeField]
    GameObject FireCampWindowRes;

    [SerializeField]
    GameObject CardWindowRes;
    GameObject CurrentDeckWindow;
    bool IsDeckWindowOpen = true;

    [SerializeField]
    GameObject RewardRes;
    GameObject Reward;

    [SerializeField]
    GameObject AddRewardRes;
    GameObject AddReward;
    public GameObject AddRewardWinodow { get => AddReward; set => AddReward = value; }


    [Header("ToolTip")]
    [SerializeField]
    GameObject ToolTipRes;
    GameObject _ToolTip;
    public GameObject ToolTip { get => _ToolTip; set => _ToolTip = value; }


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void FadeInEffect(Delvoid Del)
    {
        FadePanel.enabled = true;
        StartCoroutine(FadeIn(Del));
        
    }

    public void FadeOutEffect(Delvoid Del)
    {
        FadePanel.enabled = true;
        StartCoroutine(FadeOut(Del));

    }

    IEnumerator FadeIn(Delvoid Del)
    {

        FadePanel.transform.SetAsLastSibling();
        float alpha = FadePanel.color.a;
        while (alpha >= 0.01f)
        {
            alpha -= Time.deltaTime * 2.0f;
            FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, alpha);
            yield return null;
        }
        FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, 0);
        FadePanel.enabled = false;
        if (Del != null)
        {
            Del();
        }
    }
    IEnumerator FadeOut(Delvoid Del)
    {
        FadePanel.transform.SetAsLastSibling();
        float alpha = FadePanel.color.a;
        while (alpha <= 0.99f)
        {
            alpha += Time.deltaTime * 2.0f;
            FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, alpha);
            yield return null;
        }
        FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, 1);
        FadePanel.enabled = false;
        if (Del != null)
        {
            Del();
        }

    }
    public void ZeroFloorUI()
    {
        CurUI = Instantiate(ZeroPanel);
        CurUI.transform.SetParent(UICanvas.transform);
        CurUI.transform.localScale = Vector3.one;
        CurUI.transform.localPosition = new Vector3(-480.0f, -390.0f, 0.0f);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeBattleUI()
    {
        CurUI = Instantiate(BattleUI);
        CurUI.transform.SetParent(UICanvas.transform);
        CurUI.transform.localScale = Vector3.one;
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeReward()
    {
        CurUI = Instantiate(RewardRes);
        CurUI.GetComponent<RewardWindow>().WindowSetting(MainSceneController.Instance.Reward);
        CurUI.transform.SetParent(UICanvas.transform);
        CurUI.transform.localScale = Vector3.one;
        CurUI.transform.localPosition = Vector3.zero;
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeAddReward()
    {
        AddReward = Instantiate(AddRewardRes);
        AddReward.transform.SetParent(UICanvas.transform);
        AddReward.transform.localScale = Vector3.one;
        AddReward.transform.localPosition = Vector3.zero;
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeToolTip()
    {
        _ToolTip = Instantiate(ToolTipRes);
        _ToolTip.transform.SetParent(UICanvas.transform);
        _ToolTip.transform.localScale = Vector3.one;
        _ToolTip.transform.localPosition = Vector3.zero;
        _ToolTip.SetActive(false);
    }
    public void MakeFireCampWindow()
    {
        CurUI = Instantiate(FireCampWindowRes);
        CurUI.transform.SetParent(UICanvas.transform);
        CurUI.transform.localScale = Vector3.one;
        CurUI.transform.localPosition = Vector3.zero;
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void GetDeckWindow()
    {
        if (CurrentDeckWindow == null)
        {
            CurrentDeckWindow = Instantiate(CardWindowRes);
            CurrentDeckWindow.transform.SetParent(UICanvas.transform);
            CurrentDeckWindow.transform.localScale = Vector3.one;
            CurrentDeckWindow.transform.localPosition = Vector3.zero;
            CurrentDeckWindow.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Show, true);
            CurrentDeckWindow.GetComponent<CardWindow>().Cancel.onClick.AddListener(CurrentDeckWindow.GetComponent<CardWindow>().CancelButtonEvent);
            InfoBar.gameObject.transform.SetAsLastSibling();
        }
        else
        {
            if (IsDeckWindowOpen)
            {
                CurrentDeckWindow.SetActive(false);
                IsDeckWindowOpen = false;
            }
            else
            {
                CurrentDeckWindow.SetActive(true);
                IsDeckWindowOpen = true;
            }
        }
    }
    public GameObject MakeCardWindow()
    {
        GameObject tmp = Instantiate(CardWindowRes);
        tmp.transform.SetParent(UICanvas.transform);
        tmp.transform.localScale = Vector3.one;
        tmp.transform.localPosition = Vector3.zero;
        tmp.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Show, false);
        InfoBar.gameObject.transform.SetAsLastSibling();

        return tmp;
    }
    public void MakeMap()
    {
        Map = Instantiate(MapRes);
        Map.transform.SetParent(UICanvas.transform);
        Map.transform.localScale = Vector3.one;
        Map.transform.localPosition = Vector3.zero;
        InfoBar.gameObject.transform.SetAsLastSibling();
        Map.GetComponent<MapUIScript>().CancelButtonEvent(OffIsMapOpen);
    }
    public void ToggleMapInfoBar()
    {
        if (IsMapOpen == false)
        {
            Map.GetComponent<MapUIScript>().OpenMapInfoBar();
            Map.transform.SetAsLastSibling();
            InfoBar.gameObject.transform.SetAsLastSibling();
            IsMapOpen = true;
        }
        else
        {
            Map.GetComponent<MapUIScript>().HideMap();
            IsMapOpen = false;
        }

    }
    public void OpenMapProgress()
    {
        Map.GetComponent<MapUIScript>().OpenMapProgress();
        Map.transform.SetAsLastSibling();
        InfoBar.gameObject.transform.SetAsLastSibling();
        IsMapOpen = true;
    }
    public void OffMap()
    {
        Map.GetComponent<MapUIScript>().HideMap();
        IsMapOpen = false;
    }
    public GameObject GetMapUI()
    {
        return Map;
    }
    public void OffIsMapOpen()
    {
        IsMapOpen = false;
    }
    public void RemoveCurUI()
    {
        PastUI = CurUI;
        CurUI = null;
        Destroy(PastUI);
    }
    public GameObject GetCurUI()
    {
        return CurUI;
    }
}
