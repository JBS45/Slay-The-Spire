using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void HandCardStateDel(HandCardState state);
public enum HandCardState
{
    None, Idle, CardSelect, FindTarget, Using ,Used, Discard
}

public class BattleCardData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBattleCard
{
    CardData m_Data;
    [SerializeField]
    BattleCardUIScript CardUI;
    public BattleCardUIScript UI { get => CardUI; }
    BattleUIScript BattleUI;


    public CardData Data { get => m_Data; set => m_Data = value; }

    GameObject _target;
    public GameObject Target { get => _target; set => _target = value; }
    List<GameObject> _targets;
    public List<GameObject> Targets { get => _targets; set => _targets = value; }

    bool _IsEnable;
    public bool IsEnable { get => _IsEnable; }
    bool IsClicked;
    bool _IsInRange;
    public bool IsInRange { get => _IsInRange; }

    int siblingIndex;

    int HandNum;

    int CardCost;
    int _EnchantCount;
    public int EnchantCount { get => _EnchantCount; }

    HandCardState HandState = HandCardState.None;

    List<FunctionModule> CardAction;

    void Awake()
    {
        IsClicked = false;
        _IsEnable = true;
        _IsInRange = false;
        Targets = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnable)
        {
            StateProcess();
        }
    }

    public void SetInitData(CardData data, BattleUIScript battleUI)
    {
        m_Data = data;

        if (m_Data.IsEnable) _IsEnable = true;
        else _IsEnable = false;

        UpdateData(m_Data);
        BattleUI = battleUI;
        CardUI.SetCardUI(data);
        ChangeState(HandCardState.Idle);
    }

    public void UpdateData(CardData data)
    {

        m_Data = data;

        if (m_Data.IsEnable) _IsEnable = true;
        else _IsEnable = false;

        CardCost = data.Cost;
        _EnchantCount = data.EnchantCount;
        CardAction = data.Action;
        CardUI.SetCardUI(data);
    }

    public void SetHandNum(int num)
    {
        HandNum = num;
        CardUI.SetHandNum(HandNum);
    }

    public void ChangeState(HandCardState state)
    {
        if (!IsEnable && HandState == state) return;
        HandState = state;
        switch (HandState)
        {
            case HandCardState.Idle:
                break;
            case HandCardState.CardSelect:
                CardUI.ShowCard();
                break;
            case HandCardState.FindTarget:
                BattleUI.SetCardState(HandNum, HandCardState.None);
                break;
            case HandCardState.Using:
                SetTargetOff();
                CardUI.UseCard(ChangeState, HandCardState.Used);
                break;
            case HandCardState.Used:
                CardUI.CardEffect.OnTrail();
                BattleUI.UseCard(HandNum);
                ChangeState(HandCardState.Discard);
                break;
            case HandCardState.Discard:
                CardUI.DiscardCard();
                break;
                //case HandCardState.Disable:
        }
    }
    void StateProcess()
    {
        switch (HandState)
        {
            case HandCardState.Idle:
                break;
            case HandCardState.CardSelect:
                CardTracePointer();
                CardPositionCheck();
                break;
            case HandCardState.FindTarget:
                FindCardTarget();
                break;
            case HandCardState.Using:
                break;
        }
    }

    void CardTracePointer()
    {
        Vector3 screenPos = Input.mousePosition;
        float widthRate = CardUI.Canvas.GetComponent<RectTransform>().rect.size.x / Camera.main.pixelRect.width;
        float HeightRate = CardUI.Canvas.GetComponent<RectTransform>().rect.size.y / Camera.main.pixelRect.height;
        screenPos = new Vector3((widthRate * screenPos.x)- (CardUI.Canvas.GetComponent<RectTransform>().rect.size.x/2),
            HeightRate * screenPos.y, 0);
        transform.localPosition = screenPos;
        /*
        Vector3 tmp = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.localPosition = new Vector3(Camera.main.scaledPixelWidth * (tmp.x - 0.5f), Camera.main.scaledPixelHeight * tmp.y);*/
    }

    void CardPositionCheck()
    {

        if (transform.localPosition.y > Camera.main.scaledPixelHeight / 3)
        {
            if (_IsInRange == false)
            {
                CardUI.CardEffect.InRangeAnimation();
                _IsInRange = true;
                SetTarget();
            }
        }
        else
        {
            _IsInRange = false;
            SetTargetOff();

        }

    }
    //타겟 종류에 따라 다음 함수를 정해줌
    void SetTarget()
    {
        switch (m_Data.Targets)
        {
            case TargetOptions.AllEnemy:
            case TargetOptions.Random:
                MainSceneController.Instance.BattleData.AllEnemyTargetOn();
                m_Data.ClearTarget();
                break;
            case TargetOptions.Enemy:
                ChangeState(HandCardState.FindTarget);
                transform.localPosition = Vector3.zero;
                MainSceneController.Instance.BattleData.EnemyTargetOn();
                break;
            case TargetOptions.NoTarget:
                MainSceneController.Instance.BattleData.NoTargetTargetOn();
                m_Data.SetTarget(MainSceneController.Instance.Character);
                break;

        }
    }
    void SetTargetOff()
    {
        MainSceneController.Instance.BattleData.TargetOff();
    }
    void FindCardTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool IsHit = Physics2D.GetRayIntersection(ray);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (IsHit && hit.transform.gameObject.tag == "Monster")
        {
            //대상을 먼저 지정
            Target = hit.transform.gameObject;
            Target.GetComponentInChildren<Stat>().TargetOn();
            m_Data.SetTarget(Target);
        }
        else
        {
            Target = null;
            foreach(var monster in MainSceneController.Instance.BattleData.Monsters)
            {
                monster.GetComponentInChildren<Stat>().TargetOff();
            }
            m_Data.ClearTarget();
        }
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HandState == HandCardState.Idle)
        {
            BattleUI.ShowCard(HandNum);
            transform.localPosition += new Vector3(0, 160.0f, 0);
            CardUI.ShowCard();
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (HandState == HandCardState.Idle)
        {
            BattleUI.GetComponent<BattleUIScript>().CardAlign();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //카드를 선택함
        if ((HandState == HandCardState.Idle) && IsEnable)
        {
            BattleUI.GetComponent<BattleUIScript>().SetCardState(HandNum, HandCardState.None);
            ChangeState(HandCardState.CardSelect);
            return;
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if ((HandState == HandCardState.CardSelect) && IsEnable)
        {
            BattleUI.Control.SetHand(this.gameObject);
            return;
        }
    }
    public void SetSibling(int sibling)
    {
        siblingIndex = sibling;
    }
    public void OriginSibling()
    {
        transform.SetSiblingIndex(siblingIndex);
    }
    public void SetLastSibling()
    {
        transform.SetAsLastSibling();
    }

    //이펙트 킴
    public void OnEffectOn(int CurEnergy)
    {
        if (IsEnable)
        {
            if (CardCost <= CurEnergy)
            {
                CardUI.CardEffect.afterEffectOn();
                _IsEnable = true;
            }
            else
            {
                CardUI.CardEffect.afterEffectOff();
                _IsEnable = false;
            }
        }
    }

}

