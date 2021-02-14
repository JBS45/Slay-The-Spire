using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class CardUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IShopWindow
{
    [SerializeField]
    GameObject TotalCard;

    public Sprite[] CardBackgroundRes;
    public Sprite[] RareFrameRes;
    public Sprite[] CardNameBanner;
    public Sprite[] CostOrbRes;

    CardData m_Data;
    public CardData Data { get => m_Data; }

    [SerializeField]
    public Image CardBackground;
    [SerializeField]
    public TMP_Text CardDescription;
    [SerializeField]
    public Image CardImage;
    [SerializeField]
    public Image RareFrame;
    [SerializeField]
    public TMP_Text CardTypeText;
    public Image NameBanner;
    [SerializeField]
    public TMP_Text CardName;
    [SerializeField]
    public Image CostOrb;
    [SerializeField]
    public TMP_Text CostText;

    [SerializeField]
    ParticleSystem Remove;
    [SerializeField]
    ParticleSystem Enchant;

    delegate void CardEffectDel(WindowType type);

    [SerializeField]
    GameObject GoldText;
    int _Gold;
    public int Gold { get => _Gold; set => _Gold = value; }
    bool _IsCanBuy;
    public bool IsCanBuy { get => _IsCanBuy; set => _IsCanBuy = value; }
    bool IsShop;

    [SerializeField]
    Button m_Button;

    [Header("TextColorTable")]
    [SerializeField]
    Color[] TextColor;

    WindowType m_WindowType = WindowType.None;
    bool _IsEnable;
    public bool IsEnable { get => _IsEnable; set => _IsEnable = value; }
    [SerializeField]
    TrailRenderer Trail;

    float CardSize = 1.0f;

    void Awake()
    {
        Trail.enabled = false;
        IsShop = false;
        GoldText.SetActive(false);
        IsEnable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardUI(CardData card)
    {
        m_Data = card;

        switch (card.CardType)
        {
            case CardType.Attack:
                CardTypeText.text = "Attack";
                CardBackground.sprite = CardBackgroundRes[((int)card.CharType) * 3 + 1];
                RareFrame.sprite = RareFrameRes[((int)(card.CardType - 1) * 3) + (int)card.Rarity];
                break;
            case CardType.Skill:
                CardTypeText.text = "Skill";
                CardBackground.sprite = CardBackgroundRes[((int)card.CharType) * 3 + 2];
                RareFrame.sprite = RareFrameRes[((int)(card.CardType - 1) * 3) + (int)card.Rarity];
                break;
            case CardType.Power:
                CardTypeText.text = "Power";
                CardBackground.sprite = CardBackgroundRes[((int)card.CharType) * 3 + 3];
                RareFrame.sprite = RareFrameRes[((int)(card.CardType - 1) * 3) + (int)card.Rarity];
                RareFrame.SetNativeSize();
                RareFrame.transform.localPosition = new Vector3(0, 80, 0);
                RareFrame.GetComponentInChildren<TMP_Text>().transform.localPosition = new Vector3(0, -110, 0);
                break;
            case CardType.Condition:
                CardTypeText.text = "Condition";
                CardBackground.sprite = CardBackgroundRes[2];
                RareFrame.sprite = RareFrameRes[3];
                if (card.Cost == 0)
                {
                    CostOrb.enabled = false;
                    CostText.enabled = false;
                }
                break;
            case CardType.Curse:
                CardTypeText.text = "Curse";
                CardBackground.sprite = CardBackgroundRes[0];
                RareFrame.sprite = RareFrameRes[(int)(card.CardType - 3)];
                break;
        }
        switch (card.Rarity)
        {
            case RarityOptions.Basic:
                NameBanner.sprite = CardNameBanner[0];
                break;
            case RarityOptions.Common:
                NameBanner.sprite = CardNameBanner[1];
                break;
            case RarityOptions.Rare:
                NameBanner.sprite = CardNameBanner[2];
                break;
        }
        SetDescription(card);
        CardImage.sprite = card.CardImage;

        switch (card.CharType)
        {
            case CharacterType.None:
                CostOrb.sprite = CostOrbRes[0];
                break;
            case CharacterType.Ironclad:
                CostOrb.sprite = CostOrbRes[1];
                break;
        }

        if (card.IsAllCost)
        {
            CostText.text = "X";
        }
        else
        {
            CostText.text = card.Cost.ToString();
        }
    }
    void SetDescription(CardData data)
    {

        string tmp = "";
        int tmpInt = 0;
        Color tmpColor;

        string CardNameResult = data.CardName;
        if (data.EnchantCount > 0)
        {
            CardName.color = TextColor[2];
            CardNameResult += "+";
        }
        CardName.text = CardNameResult;


        for (int i = 0; i < data.Action.Count; ++i)
        {
            switch (data.Action[i].Type)
            {
                case AbilityType.Attack:
                    tmpInt=AttackManager.Instance.UseAttack(MainSceneController.Instance.Character, data.GetTarget(), null,null, data.Action[i].AbilityKey, data.Action[i].Value, false);
                    break;
                case AbilityType.Skill:
                    tmpInt=SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, data.GetTarget(), data.Action[i].AbilityKey, data.Action[i].Value, false);
                    break;
                case AbilityType.Power:
                    tmpInt=PowerManager.Instance.AssginBuff(data.GetTarget(), data.Action[i].variety, data.Action[i].Value,false);
                    break;
            }

            if (Mathf.Abs(data.BaseValue[i]) > tmpInt)
            {
                tmpColor = TextColor[1];
            }
            else if (Mathf.Abs(data.BaseValue[i]) == tmpInt)
            {
                tmpColor = TextColor[0];
            }
            else
            {
                tmpColor = TextColor[2];
            }
            if (data.Action[i].AbilityKey == "Energy")
            {
                string result = "";
                string EnergyStr = "<sprite=0>";

                for(int j = 0; j < tmpInt; ++j)
                {
                    result += EnergyStr;
                }

                tmp += string.Format(data.Action[i].Decription, ColorTohexadecimal(tmpColor), result, data.Repeat);
            }
            else
            {
                tmp += string.Format(data.Action[i].Decription, ColorTohexadecimal(tmpColor), tmpInt, data.Repeat);
            }
        }
        CardDescription.text = tmp;
    }
    string ColorTohexadecimal(Color color)
    {
        string result = "";

        int[] RGB = { (int)(color.r * 255) / 16, (int)(color.r * 255) % 16, (int)(color.g * 255) / 16, (int)(color.g * 255) % 16, (int)(color.b * 255) / 16, (int)(color.r * 255) % 16 };

        for (int i = 0; i < RGB.Length; ++i)
        {
            if (RGB[i] < 10)
            {
                result += RGB[i].ToString();
            }
            else if (RGB[i] == 10)
            {
                result += "A";
            }
            else if (RGB[i] == 11)
            {
                result += "B";
            }
            else if (RGB[i] == 12)
            {
                result += "C";
            }
            else if (RGB[i] == 13)
            {
                result += "D";
            }
            else if (RGB[i] == 14)
            {
                result += "E";
            }
            else if (RGB[i] == 15)
            {
                result += "F";
            }
        }

        return result;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEnable)
        {
            transform.localScale = new Vector3(1.3f* CardSize, 1.3f* CardSize, 1.3f* CardSize);
        }
        if (IsShop)
        {
            HandEnter();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsEnable)
        {
            transform.localScale = new Vector3(CardSize, CardSize, CardSize);
        }
        if (IsShop)
        {
            HandExit();
        }
    }

    public IEnumerator MoveToOriginDeck()
    {
        Trail.enabled = true;
        Vector3 target = new Vector3(800, 500, 0);
        Quaternion tmpQuat = Quaternion.FromToRotation(transform.localPosition, target);
        float angle = Vector3.Angle(transform.localPosition, target)+360;
        float size = 1;

        while (size > 0.2f)
        {
            size -= Time.deltaTime*3;
            TotalCard.transform.localScale = new Vector3(size, size, 0);
            yield return null;
        }
        /*while (Vector3.Angle(transform.up, target-transform.localPosition) > 10.0f)
        {
            TotalCard.transform.eulerAngles += new Vector3(0, 0, 3.0f);
            yield return null;
        }*/
        while (Vector3.Distance(transform.localPosition, target) > 1.0f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 60.0f);
            yield return null;
        }

        Destroy(this.gameObject);
    }
    public void SetButtonEvent(Delvoid del)
    {
        m_Button.onClick.AddListener(
            () => 
            {
                MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.CardSelect);
                del();
            });
    }
    public void SetSize(float size)
    {
        CardSize = size;
    }

    public void SelectCardUI(WindowType type, Delvoid del)
    {
        MainSceneController.Instance.UIControl.MakeEnchantCardWindow();
        MainSceneController.Instance.UIControl.EnchantCardWindow.GetComponent<CardSelectWindow>().SetWindowType(Data, type,del);
    }
    public void ShopUISetting(int gold,ShopWindowDelegate del)
    {
        GoldText.SetActive(true);
        Gold = gold;
        GoldText.GetComponentInChildren<TMP_Text>().text = Gold.ToString();
        m_Button.onClick.AddListener(()=> { ShopButtonEvent(del); });
        IsShop = true;
    }
    public void ShopRefresh()
    {
        if (Gold > MainSceneController.Instance.PlayerData.CurrentMoney)
        {
            GoldText.GetComponentInChildren<TMP_Text>().color = TextColor[1];
        }
        else
        {
            GoldText.GetComponentInChildren<TMP_Text>().color = TextColor[0];
        }
    }
    public void ShopButtonEvent(ShopWindowDelegate del)
    {
        if (Gold < MainSceneController.Instance.PlayerData.CurrentMoney) { 

            transform.SetAsLastSibling();
            transform.SetParent(MainSceneController.Instance.UIControl.UICanvas.transform);
            MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.CashRegister);
            MainSceneController.Instance.PlayerData.CurrentMoney -= Gold;
            MainSceneController.Instance.PlayerData.AddCard(Data);
            MainSceneController.Instance.PlayerData.Notify();
            StartCoroutine(MoveToOriginDeck());
            del(this);
        }
    }
    public void HandEnter()
    {
        MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetTarget(this.gameObject);
    }
    public void HandExit()
    {
        MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetTarget(null);
    }
    public void CardEffect(WindowType type)
    {
        transform.SetParent(MainSceneController.Instance.UIControl.GetCanvas().transform);
        transform.localPosition = Vector3.zero;

        StartCoroutine(GrowScaleCard(SelectEffectType,type));
    }
    IEnumerator CardMove(GameObject Target)
    {
        yield return new WaitForSeconds(0.5f);
        if (Enchant.isPlaying)
        {
            yield return new WaitForSeconds(1f);
            Destroy(Enchant);
        }

        while (TotalCard.transform.localScale.x > 0.1f)
        {
            TotalCard.transform.localScale -= Vector3.one * 2*Time.deltaTime;
            yield return null;
        }
        Vector3 TargetPos = GameUtill.ChildCoordCanvas(Target, MainSceneController.Instance.UIControl.UICanvas.gameObject);

        Vector3 Dir = TargetPos - transform.localPosition;
        Quaternion QuatAngle = Quaternion.FromToRotation(transform.up, Dir);
        while (Vector3.Angle(transform.up, Dir) > 1.0f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, QuatAngle, 10 * Time.deltaTime);
            yield return null;
        }
        Trail.enabled = true;
        while (Vector3.Distance(transform.localPosition, TargetPos) > 1.0f){
            transform.localPosition = Vector3.Slerp(transform.localPosition, TargetPos, 30*Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);

    }
    IEnumerator GrowScaleCard(CardEffectDel del,WindowType type)
    {
        while (transform.localScale.x < 1.4f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,new Vector3(1.5f, 1.5f, 1.5f),5*Time.deltaTime);
            yield return null;
        }
        del(type);
    }
    void SelectEffectType(WindowType type)
    {
        switch (type)
        {
            case WindowType.Enchant:
                Enchant.Play();
                MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.Enchant);
                StartCoroutine(CardMove(MainSceneController.Instance.UIControl.InfoBar.m_DeckCount.gameObject));
                break;
            case WindowType.Remove:
                MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.CashRegister);
                StartCoroutine(RemoveEffect());
                break;
            case WindowType.Reward:
                MainSceneController.Instance.PlayerData.Notify();
                StartCoroutine(CardMove(MainSceneController.Instance.UIControl.InfoBar.m_DeckCount.gameObject));
                break;
        }
    }
    IEnumerator RemoveEffect()
    {
        Material[] Dissolve = { CardBackground.material, CardImage.material, NameBanner.material, RareFrame.material, CostOrb.material };
        Material[] instancesMat = new Material[Dissolve.Length];
        Image[] Images = { CardBackground, CardImage, NameBanner, RareFrame, CostOrb };
        TMP_Text[] Texts = { CardDescription, CardTypeText, CardName, CostText };

        for (int i = 0; i < Dissolve.Length; ++i)
        {
            instancesMat[i] = Instantiate(Dissolve[i]);
            Images[i].material = instancesMat[i];
        }

        Remove.Play();
        float amount = 0;
        while (Images[0].color.a > 0.05f)
        {
            foreach (var image in Images)
            {
                image.color -= new Color(0, 0, 0, Time.deltaTime);
            }
            foreach (var text in Texts)
            {
                text.color -= new Color(0, 0, 0, Time.deltaTime);
            }
            amount += Time.deltaTime;
            foreach (var mat in instancesMat)
            {
                mat.SetFloat("_DissolveAmount", amount);
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }
    public void SelectMoveCard(bool IsDiscard) {
        if (IsDiscard)
        {
            StartCoroutine(CardMove(MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>().DiscardDeckButton));
        }
        else
        {
            StartCoroutine(CardMove(MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>().BattleDeckButton));
        }
    }

}

