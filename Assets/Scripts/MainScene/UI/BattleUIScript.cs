using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIScript : MonoBehaviour, IDrawEvent,IObservers
{

    public GameObject ButtonRes;

    public EnergyOrb EnergyOrb;

    int MaxEnergy = 3;
    int CurrEnergy;
    public float Length;
    public float TermPerCard;
    BattleData m_BattleData;
    BattleDataAsset m_BattleCardData;

    [SerializeField]
    GameObject CardRes;
    
    List<GameObject> HandCard;
    public List<GameObject> Hand { get => HandCard; }

    [SerializeField]
    GameObject BattleDeckButton;
    GameObject DeckWindow;
    [SerializeField]
    GameObject DiscardDeckButton;
    GameObject DiscardWindow;
    [SerializeField]
    GameObject ExtinctiontButton;
    GameObject ExtinctionWindow;
    [SerializeField]
    Button TurnEndButton;
    [SerializeField]
    Transform HandPoint;
    [SerializeField]
    Transform CardUsedPoint;

    [Header("Start Battle UI")]
    [SerializeField]
    Image StartPanel;
    [SerializeField]
    Image Sword1;
    [SerializeField]
    Image Sword2;
    [SerializeField]
    TMP_Text BattleStartText;

    PlayerController controller;
    public PlayerController Control { get => controller; }

    bool IsInit;

    private void Awake()
    {
        IsInit = false;
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsInit)
        {
            controller.InputHandler();
        }
    }

    public void CardAlign()
    {
        int TotalCardNum = HandCard.Count;
        TermPerCard = Length / TotalCardNum;
        float twist;
        if (TotalCardNum == 0)
        {
            twist = 0;
        }
        else
        {
            twist = 20 / TotalCardNum;
        }
        //홀수
        if (TotalCardNum % 2 == 1)
        {
            for (int i = 0; i < TotalCardNum; ++i)
            {
                float delta = TermPerCard * (i - (TotalCardNum / 2));
                float twistDelta = twist * (i - (TotalCardNum / 2));
                HandCard[i].transform.localPosition = new Vector3(delta, -Mathf.Abs(twistDelta * 3), 0);
                HandCard[i].GetComponent<BattleCardUIScript>().SetOriginPos(HandCard[i].transform.localPosition);
                HandCard[i].GetComponent<BattleCardUIScript>().CardAlignRotate(-twistDelta);

            }

            HandCard[TotalCardNum / 2].transform.localPosition = Vector3.zero;
        }
        else
        {
            for (int i = 0; i < TotalCardNum / 2; ++i)
            {
                float delta = TermPerCard * (i - (TotalCardNum / 2));
                float twistDelta = twist * (i - (TotalCardNum / 2));
                HandCard[i].transform.localPosition = new Vector3(delta + TermPerCard / 2, -Mathf.Abs(twistDelta * 3), 0);
                HandCard[i].GetComponent<BattleCardUIScript>().SetOriginPos(HandCard[i].transform.localPosition);
                HandCard[i].GetComponent<BattleCardUIScript>().CardAlignRotate(-twistDelta);
            }
            for (int i = TotalCardNum / 2; i < TotalCardNum; ++i)
            {
                float delta = TermPerCard * ((i + 1) - (TotalCardNum / 2));
                float twistDelta = twist * ((i + 1) - (TotalCardNum / 2));
                HandCard[i].transform.localPosition = new Vector3(delta - TermPerCard / 2, -Mathf.Abs(twistDelta * 3), 0);
                HandCard[i].GetComponent<BattleCardUIScript>().SetOriginPos(HandCard[i].transform.localPosition);
                HandCard[i].GetComponent<BattleCardUIScript>().CardAlignRotate(-twistDelta);
            }
        }

        int tmpSilbling = HandCard[0].transform.GetSiblingIndex();
        for (int i = 0; i < TotalCardNum; ++i)
        {
            HandCard[i].GetComponent<BattleCardData>().SetHandNum(i);
            HandCard[i].GetComponent<BattleCardData>().SetSibling(tmpSilbling + i);
            HandCard[i].GetComponent<BattleCardData>().OriginSibling();
        }

    }
    public void UIInit(BattleDataAsset CardData)
    {
        HandCard = new List<GameObject>();
        m_BattleData = MainSceneController.Instance.BattleData;
        Length = 600.0f;

        //함수에 버튼 눌렀을시 덱을 보여주는 함수를 넣어야함
        BattleDeckButton.GetComponent<BattleDeckButton>().ButtonSetting(() => { Debug.Log("Deck"); });

        //함수에 버튼 눌렀을시 덱을 보여주는 함수를 넣어야함
        DiscardDeckButton.GetComponent<BattleDeckButton>().ButtonSetting(() => { Debug.Log("Discard"); });

        ExtinctiontButton.GetComponent<BattleDeckButton>().ButtonSetting(() => { Debug.Log("Extinct"); });
        ExtinctiontButton.GetComponent<BattleDeckButton>().DisEnableButton(false);
        m_BattleCardData = CardData;
        m_BattleCardData.Attach(this);

        EnergyOrb.OrbImageInit(MainSceneController.Instance.PlayerData.CharType);

        controller = new PlayerController(this, m_BattleData);

        IsInit = true;
    }


    public void UpdateData()
    {
        BattleDeckButton.GetComponent<BattleDeckButton>().CountUpdate(m_BattleCardData.Deck.Count);
        DiscardDeckButton.GetComponent<BattleDeckButton>().CountUpdate(m_BattleCardData.Discard.Count);
        ExtinctiontButton.GetComponent<BattleDeckButton>().CountUpdate(m_BattleCardData.Extinction.Count);
        if (m_BattleCardData.Extinction.Count >= 1)
        {
            ExtinctiontButton.GetComponent<BattleDeckButton>().DisEnableButton(true);
        }
        CurrEnergy = m_BattleData.CurrentEnergy;
        EnergyOrb.EnergyCharge(CurrEnergy, MaxEnergy);
    }
    
    IEnumerator MoveCard(Vector3 target, GameObject Card, float speed)
    {

        while (Vector3.Distance(Card.transform.localPosition, target) > 1.0f)
        {
            Card.transform.localPosition = Vector3.MoveTowards(Card.transform.localPosition, target, speed * 10.0f);
            Card.transform.localScale = Vector3.Lerp(Card.transform.localScale, Vector3.one, Time.deltaTime * speed);
            yield return null;
        }

        Card.transform.localPosition = target;
        Card.transform.localScale = Vector3.one;
        CardAlign();
    }
    public void ShowCard(int num)
    {
        if (num == 0)
        {
            for (int i = 1; i < HandCard.Count; ++i)
            {
                HandCard[i].transform.localPosition += new Vector3(150, 0, 0);
            }
        }
        else if (num == 9)
        {
            for (int i = 0; i < HandCard.Count - 1; ++i)
            {
                HandCard[i].transform.localPosition += new Vector3(-150, 0, 0);
            }
        }
        else
        {
            for (int i = 0; i < num; ++i)
            {
                HandCard[i].transform.localPosition += new Vector3(-150, 0, 0);
            }
            for (int i = num + 1; i < HandCard.Count; ++i)
            {
                HandCard[i].transform.localPosition += new Vector3(150, 0, 0);
            }

        }
    }
    public void SetCardState(HandCardState state)
    {
        for (int i = 0; i < HandCard.Count; ++i)
        {
            HandCard[i].GetComponent<BattleCardData>().ChangeState(state);
        }
    }
    public void SetCardState(int num, HandCardState state)
    {
        for (int i = 0; i < HandCard.Count; ++i)
        {
            if (i != num)
            {
                HandCard[i].GetComponent<BattleCardData>().ChangeState(state);
            }
        }
    }
    IEnumerator BattleStartUI(BattleStateDel del, BattleDataState state)
    {
        float alpha = 0.0f;
        while (alpha <= 0.95f)
        {
            alpha += Time.deltaTime;
            Sword1.color = new Color(Sword1.color.r, Sword1.color.g, Sword1.color.b, alpha);
            Sword2.color = new Color(Sword2.color.r, Sword2.color.g, Sword2.color.b, alpha);
            BattleStartText.color = new Color(BattleStartText.color.r, BattleStartText.color.g, BattleStartText.color.b, alpha);

            yield return null;
        }
        alpha = 1.0f;

        Sword1.color = new Color(Sword1.color.r, Sword1.color.g, Sword1.color.b, alpha);
        Sword2.color = new Color(Sword2.color.r, Sword2.color.g, Sword2.color.b, alpha);
        BattleStartText.color = new Color(BattleStartText.color.r, BattleStartText.color.g, BattleStartText.color.b, alpha);

        float tmp = Camera.main.pixelWidth;


        Vector3 target = Sword1.transform.localPosition - new Vector3(tmp, 0, 0);

        while (Vector3.Distance(Sword1.transform.localPosition, target) > 1.0f)
        {
            Sword1.transform.localPosition = Vector3.MoveTowards(Sword1.transform.localPosition, target, 50.0f);
            Sword2.transform.localPosition = Vector3.MoveTowards(Sword2.transform.localPosition, target, 50.0f);
            BattleStartText.transform.localPosition = Vector3.MoveTowards(BattleStartText.transform.localPosition, target, 50.0f);

            yield return null;
        }
        StartCoroutine(MoveOrb());
        while (alpha > 0.05f)
        {
            alpha -= Time.deltaTime;
            StartPanel.color = new Color(StartPanel.color.r, StartPanel.color.g, StartPanel.color.b, alpha);
        }
        StartPanel.color = new Color(StartPanel.color.r, StartPanel.color.g, StartPanel.color.b, 0);

        del(state);
    }

    public void StartBattle(BattleStateDel del, BattleDataState state)
    {
        StartCoroutine(BattleStartUI(del, state));
    }
    public void DiscardCard(int num)
    {
        m_BattleData.Detach(HandCard[num].GetComponent<BattleCardData>());
        HandCard.RemoveAt(num);
        CardAlign();
    }
    public void UseCard(int num)
    {
        m_BattleData.Detach(HandCard[num].GetComponent<BattleCardData>());
        HandCard.RemoveAt(num);
        m_BattleData.UseCard(num);
        CardAlign();
    }
    public void UpdateHandData(List<CardData> hand)
    {
        for (int i = 0; i < HandCard.Count; ++i)
        {
            HandCard[i].GetComponent<BattleCardData>().UpdateData(hand[i]);
        }
    }
    public void TurnEnd()
    {
        if (HandCard.Count > 0)
        {
            foreach (var item in HandCard)
            {
                item.GetComponent<BattleCardUIScript>().DiscardCard();
            }
        }
        SetEnable(false);
        StartCoroutine(TurnEndCheck());
    }

    IEnumerator MoveOrb()
    {
        Vector3 target = EnergyOrb.gameObject.transform.localPosition + new Vector3(450, 0, 0);

        while (Vector3.Distance(EnergyOrb.gameObject.transform.localPosition, target) > 1.0f)
        {
            EnergyOrb.gameObject.transform.localPosition = Vector3.MoveTowards(EnergyOrb.gameObject.transform.localPosition, target, 50.0f);
            yield return null;
        }
        EnergyOrb.gameObject.transform.localPosition = target;
    }
    IEnumerator TurnEndCheck()
    {
        while (HandCard.Count > 0)
        {
            if (HandCard[0] == null)
            {
                HandCard.RemoveAt(0);
            }
            yield return null;
        }
        MainSceneController.Instance.BattleData.PlayerTurnProgress(TurnState.TurnEnd);
    }

    public void PlayerTurnBegin()
    {
        EnergyOrb.EnergyCharge(CurrEnergy, MaxEnergy);
    }
    //드로우 관련
    public void DrawCard(CardData data)
    {
        Draw(data);
    }
    public void Draw(CardData cardAsset)
    {
        GameObject Tmp = Instantiate(CardRes);
        Tmp.transform.SetParent(HandPoint);
        Tmp.transform.localScale = Vector3.zero;
        Tmp.transform.localPosition = new Vector3(BattleDeckButton.transform.localPosition.x, 0.0f, 0.0f);
        Tmp.GetComponent<BattleCardData>().SetInitData(cardAsset, this);
        Tmp.GetComponent<BattleCardUIScript>().SetPos(BattleDeckButton.transform.localPosition, DiscardDeckButton.transform.localPosition, CardUsedPoint.localPosition, this.gameObject);
        m_BattleData.Attach(Tmp.GetComponent<BattleCardData>());
        HandCard.Add(Tmp);
        StartCoroutine(MoveCard(HandPoint.transform.position, Tmp, 10.0f));
    }


    public void SetEnable(bool IsEnable)
    {
        TurnEndButton.enabled = IsEnable;   
    }
    public void IsCardUsing(bool IsUsing)
    {
        if (IsUsing) {
            SetCardState(HandCardState.None);
        }
        else
        {
            SetCardState(HandCardState.Idle);
        }
        Control.SetIsCardUsing(IsUsing);
    }

    public void ShowDeck()
    {
        if (DeckWindow == null)
        {
            DeckWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
        }
        DeckWindow.SetActive(true);
        DeckWindow.GetComponent<CardWindow>().SetCardWindow(m_BattleCardData.Deck, WindowType.Show, false);
        DeckWindow.GetComponent<CardWindow>().Cancel.onClick.AddListener(() => { DeckWindow.SetActive(false); });
        DeckWindow.GetComponent<CardWindow>().NameButton();
    }
    public void ShowDiscard()
    {
        if (DiscardWindow == null)
        {
            DiscardWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
        }
        DiscardWindow.SetActive(true);
        DiscardWindow.GetComponent<CardWindow>().SetCardWindow(m_BattleCardData.Discard, WindowType.Show, false);
        DiscardWindow.GetComponent<CardWindow>().Cancel.onClick.AddListener(() => { DiscardWindow.SetActive(false); });
        DiscardWindow.GetComponent<CardWindow>().NameButton();
    }
    public void ShowExtinction()
    {
        if (ExtinctionWindow == null)
        {
            ExtinctionWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
        }
        ExtinctionWindow.SetActive(true);
        ExtinctionWindow.GetComponent<CardWindow>().SetCardWindow(m_BattleCardData.Extinction, WindowType.Show, false);
        ExtinctionWindow.GetComponent<CardWindow>().Cancel.onClick.AddListener(() => { ExtinctionWindow.SetActive(false); });
        ExtinctionWindow.GetComponent<CardWindow>().NameButton();
    }

    void OnDestroy()
    {
        m_BattleCardData.Detach(this);
    }
}
