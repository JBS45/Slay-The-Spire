using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum WindowType
{
    None, Reward, Remove, Add, Enchant, Show,
}
public class CardWindow : MonoBehaviour
{
    [SerializeField]
    GameObject CardRes;

    WindowType m_Type;

    [SerializeField]
    Transform Content;
    GridLayoutGroup Layout;

    List<GameObject> Cards;
    List<Vector3> PosList;

    [SerializeField]
    GameObject CancelButton;
    public Button Cancel
    {
        get => CancelButton.GetComponentInChildren<Button>();
    }
    Vector3 CancelButtonOriginVector;

    [SerializeField]
    GameObject CardOrderBar;

    RectTransform rect;

    ScrollRect ScrollRect;
    Scrollbar Scroll;

    bool IsOrder = true;
    bool IsName = false;
    bool IsType = false;
    bool IsCost = false;
    private void OnEnable()
    {
        Scroll.size = 0.2f;
        Scroll.value = 1;
        Content.localPosition = new Vector3(Content.localPosition.x, -500, Content.localPosition.z);
        StartCoroutine(OnEnableCancelButton());
    }

    private void Awake()
    {
        Layout = Content.GetComponent<GridLayoutGroup>();
        rect = GetComponent<RectTransform>();
        Cards = new List<GameObject>();
        PosList = new List<Vector3>();
        Scroll = GetComponentInChildren<Scrollbar>();
        ScrollRect = GetComponentInChildren<ScrollRect>();
        CancelButtonOriginVector = CancelButton.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCardWindow(List<CardData> CardDataList ,WindowType type, bool IsCanAlign = false)
    {
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        m_Type = type;

        if (IsCanAlign)
        {
            CardOrderBar.SetActive(true);
        }
        else
        {
            CardOrderBar.SetActive(false);
        }

        switch (m_Type)
        {
            case WindowType.Enchant:
                SettingEnchantWindow(CardDataList);
                break;
            case WindowType.Show:
                SettingWindow(CardDataList);
                break;
            case WindowType.Remove:
                SettingWindow(CardDataList);
                break;
        }

        
    }
    void SettingWindow(List<CardData> CardDataList)
    {
        if (Cards.Count > 0)
        {
            foreach (var item in Cards)
            {
                Destroy(item);
            }
            Cards.Clear();
        }

        foreach (var item in CardDataList) {
            GameObject tmp = Instantiate(CardRes);
            tmp.transform.SetParent(Content);
            tmp.transform.localScale = new Vector3((Layout.cellSize.x / 240), (Layout.cellSize.x / 240), (Layout.cellSize.x / 240));
            tmp.GetComponent<CardUIScript>().SetCardUI(item);
            tmp.GetComponent<CardUIScript>().SetSize(Layout.cellSize.x / 240);

            Cards.Add(tmp);
        }
        
    }
    void SettingEnchantWindow(List<CardData> CardDataList)
    {
        if (Cards.Count > 0)
        {
            foreach (var item in Cards)
            {
                Destroy(item);
            }
            Cards.Clear();
        }

        List<CardData> tmpList = new List<CardData>();

        foreach (var item in CardDataList)
        {
            if(item.MultipleEnchant==true || item.EnchantCount == 0)
            {
                tmpList.Add(item);
            }
        }

        foreach (var item in tmpList)
        {
            GameObject tmp = Instantiate(CardRes);
            tmp.transform.SetParent(Content);
            tmp.transform.localScale = new Vector3((Layout.cellSize.x / 240), (Layout.cellSize.x / 240), (Layout.cellSize.x / 240));
            tmp.GetComponent<CardUIScript>().SetCardUI(item);
            tmp.GetComponent<CardUIScript>().SetSize(Layout.cellSize.x / 240);

            Cards.Add(tmp);
        }

    }

    public void CancelButtonEvent()
    {
        MainSceneController.Instance.UIControl.GetDeckWindow();
    }

    IEnumerator OnEnableCancelButton()
    {
        CancelButton.transform.localPosition = CancelButtonOriginVector;
        Vector3 target = CancelButtonOriginVector + new Vector3(250, 0, 0);
        while (Vector3.Distance(CancelButton.transform.localPosition, target) > 1.0f)
        {
            CancelButton.transform.localPosition = Vector3.MoveTowards(CancelButton.transform.localPosition, target, 10.0f);
            yield return null;
        }
        CancelButton.transform.localPosition = target;
    }
    public void OrderButton()
    {
        PosList.Clear();
        foreach (var card in Cards)
        {
            PosList.Add(card.transform.localPosition);
        }
        if (IsOrder)
        {
            IsOrder = false;

            Cards.Reverse();
            for (int i = 0; i < Cards.Count; ++i)
            {
                StartCoroutine(MoveCard(Cards[i], PosList[i]));
            }
        }
        else
        {
            IsOrder = true;

            Cards.Reverse();
            for (int i = 0; i < Cards.Count; ++i)
            {
                StartCoroutine(MoveCard(Cards[i], PosList[i]));
            }
        }
    }
    public void NameButton()
    {
        PosList.Clear();
        foreach (var card in Cards)
        {
            PosList.Add(card.transform.localPosition);
        }
        if (IsName)
        {
            IsName = false;
            var List = Cards.OrderByDescending(card => card.GetComponent<CardUIScript>().Data.CardName).ToList();

            for(int i = 0; i < List.Count; ++i)
            {
                StartCoroutine(MoveCard(List[i], PosList[i]));
            }
            Cards = List;
        }
        else
        {
            IsName = true;

            var List = Cards.OrderBy(card => card.GetComponent<CardUIScript>().Data.CardName).ToList();

            for (int i = 0; i < List.Count; ++i)
            {
                StartCoroutine(MoveCard(List[i], PosList[i]));
            }
            Cards = List;
        }
    }
    public void TypeButton()
    {
        PosList.Clear();
        foreach (var card in Cards)
        {
            PosList.Add(card.transform.localPosition);
        }
        if (IsType)
        {
            IsType = false;
            var List = Cards.OrderBy(card => card.GetComponent<CardUIScript>().Data.CardName).OrderByDescending(card => card.GetComponent<CardUIScript>().Data.CardType).ToList();

            for (int i = 0; i < List.Count; ++i)
            {
                StartCoroutine(MoveCard(List[i], PosList[i]));
            }
            Cards = List;
        }
        else
        {
            IsType = true;

            var List = Cards.OrderBy(card => card.GetComponent<CardUIScript>().Data.CardName).OrderBy(card => card.GetComponent<CardUIScript>().Data.CardType).ToList();

            for (int i = 0; i < List.Count; ++i)
            {
                StartCoroutine(MoveCard(List[i], PosList[i]));
            }
            Cards = List;
        }
    }
    public void CostButton()
    {
        PosList.Clear();
        foreach (var card in Cards)
        {
            PosList.Add(card.transform.localPosition);
        }
        if (IsCost)
        {
            IsCost = false;
            var List = Cards.OrderBy(card => card.GetComponent<CardUIScript>().Data.CardName).OrderByDescending(card => card.GetComponent<CardUIScript>().Data.Cost).ToList();

            for (int i = 0; i < List.Count; ++i)
            {
                StartCoroutine(MoveCard(List[i], PosList[i]));
            }
            Cards = List;
        }
        else
        {
            IsCost = true;

            var List = Cards.OrderBy(card => card.GetComponent<CardUIScript>().Data.CardName).OrderBy(card => card.GetComponent<CardUIScript>().Data.Cost).ToList();

            for (int i = 0; i < List.Count; ++i)
            {
                StartCoroutine(MoveCard(List[i], PosList[i]));
            }
            Cards = List;
        }
    }
    IEnumerator MoveCard(GameObject Card,Vector3 target)
    {
        while (Vector3.Distance(Card.transform.localPosition, target) > 1.0f)
        {
            Card.transform.localPosition = Vector3.MoveTowards(Card.transform.localPosition, target, 50.0f);
            yield return null;
        }
        Card.transform.localPosition = target;
    }
    public void OnValueChange()
    {
        CardOrderBar.transform.localPosition = Content.localPosition + new Vector3(0, 40, 0);
        Scroll.size = 0.2f;
    }
    public void SetEnchantCardButtonEvent()
    {
        foreach(var item in Cards)
        {
            item.GetComponentInChildren<Button>().onClick.AddListener(()=> { item.GetComponent<CardUIScript>().SelectCardUI(WindowType.Enchant); });
        }
    }
    public void SetRemoveCardButtonEvent()
    {
        foreach (var item in Cards)
        {
            item.GetComponentInChildren<Button>().onClick.AddListener(() => { item.GetComponent<CardUIScript>().SelectCardUI(WindowType.Remove); });
        }
    }
}
