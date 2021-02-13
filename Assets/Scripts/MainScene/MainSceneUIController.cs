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
    GameObject CardRes;
    
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
    GameObject EnchantCardWindowRes;
    GameObject EnchantCard;
    public GameObject EnchantCardWindow { get => EnchantCard; set => EnchantCard = value; }

    [SerializeField]
    GameObject RewardRes;
    GameObject Reward;

    [SerializeField]
    GameObject AddRewardRes;
    GameObject AddReward;
    public GameObject AddRewardWinodow { get => AddReward; set => AddReward = value; }

    [SerializeField]
    GameObject ShopRes;

    [SerializeField]
    GameObject EventRes;

    [SerializeField]
    GameObject GameOverUI;

    [SerializeField]
    GameObject OptionUI;

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
        FadePanel.transform.SetAsLastSibling();
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
        if (Del != null)
        {
            Del();
        }
        FadePanel.enabled = false;
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
        if (Del != null)
        {
            Del();
        }
    }
    public void ZeroFloorUI()
    {
        CurUI = Instantiate(ZeroPanel, UICanvas.transform);
        CurUI.transform.localPosition = new Vector3(-480.0f, -390.0f, 0.0f);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeBattleUI()
    {
        CurUI = Instantiate(BattleUI, UICanvas.transform);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeReward()
    {
        CurUI = Instantiate(RewardRes, UICanvas.transform);
        CurUI.GetComponent<RewardWindow>().BattleReward(MainSceneController.Instance.Reward);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeAddReward()
    {
        AddReward = Instantiate(AddRewardRes, UICanvas.transform);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeShopWindow()
    {
        CurUI = Instantiate(ShopRes, UICanvas.transform);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeEventWindow()
    {
        CurUI = Instantiate(EventRes, UICanvas.transform);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }

    public void MakeToolTip()
    {
        _ToolTip = Instantiate(ToolTipRes, UICanvas.transform);
        _ToolTip.SetActive(false);
    }
    public void MakeFireCampWindow()
    {
        CurUI = Instantiate(FireCampWindowRes, UICanvas.transform);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeEnchantCardWindow()
    {
        EnchantCard = Instantiate(EnchantCardWindowRes, UICanvas.transform);
        EnchantCard.transform.SetAsLastSibling();
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeGameOver()
    {
        CurUI = Instantiate(GameOverUI, UICanvas.transform);
        InfoBar.gameObject.transform.SetAsLastSibling();
    }
    public void MakeOption()
    {
        GameObject obj = Instantiate(OptionUI, UICanvas.transform);
    }
    public void GetDeckWindow()
    {
        if (CurrentDeckWindow == null)
        {
            CurrentDeckWindow = Instantiate(CardWindowRes, UICanvas.transform);
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
                CurrentDeckWindow.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Show, true);
                CurrentDeckWindow.transform.SetAsLastSibling();
                InfoBar.gameObject.transform.SetAsLastSibling();
                IsDeckWindowOpen = true;
            }
        }
    }
    public GameObject MakeCardWindow()
    {
        GameObject tmp = Instantiate(CardWindowRes, UICanvas.transform);
        tmp.transform.SetAsLastSibling();
        tmp.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Show, false);
        InfoBar.gameObject.transform.SetAsLastSibling();

        return tmp;
    }
    public void MakeMap()
    {
        Map = Instantiate(MapRes, UICanvas.transform);
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
    public void RemoveAllTagUI()
    {
        for(int i = 0; i < UICanvas.transform.childCount; ++i)
        {
            if (UICanvas.transform.GetChild(i).tag=="UI")
            {
                Destroy(UICanvas.transform.GetChild(i));
            }
        }
    }
    public void MakeCard(WindowType type,int count,CardData data)
    {
        for (int i = 0; i < count; ++i) {
            GameObject obj = Instantiate(CardRes, UICanvas.transform);
            obj.transform.localScale = Vector3.one * 0.1f;
            obj.transform.localPosition = Vector3.zero - new Vector3(200 * (count-1)-400*i, 0, 0);
            obj.GetComponent<CardUIScript>().SetCardUI(data);
            obj.GetComponent<CardUIScript>().IsEnable = false;
            obj.GetComponent<CardUIScript>().CardEffect(type);
        }
    }
    public void MakeCard(bool IsDiscard,int count, CardData data)
    {
        Debug.Log(count);
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = Instantiate(CardRes, UICanvas.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero - new Vector3(200 * (count - 1) - 400 * i, 0, 0);
            obj.GetComponent<CardUIScript>().SetCardUI(data);
            obj.GetComponent<CardUIScript>().IsEnable = false;
            obj.GetComponent<CardUIScript>().SelectMoveCard(IsDiscard);
            
        }
    }
    public  GameObject GetCanvas()
    {
        return UICanvas.gameObject;
    }
}
