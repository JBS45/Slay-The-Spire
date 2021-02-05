using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public delegate void Delvoidint(int num);


public class BattleCardUIScript : MonoBehaviour
{
    [SerializeField]
    Sprite[] CardBackgroundRes;
    [SerializeField]
    Sprite[] RareFrameRes;
    [SerializeField]
    Sprite[] CardNameBanner;
    [SerializeField]
    Sprite[] CostOrbRes;

    [SerializeField]
    BattleCardData BattleCardData;

    CharacterStat CharStat;
    
    GameObject UICanvas;
    public GameObject Canvas { get => UICanvas; }

    int siblingIndex;

    [Header("TextColorTable")]
    [SerializeField]
    Color[] TextColor;

    [Header("Card Setting")]
    public GameObject TotalCard;
    public Image CardBackground;
    public TMP_Text CardDescription;
    public Image CardImage;
    public Image RareFrame;
    public TMP_Text CardTypeText;
    public Image NameBanner;
    public TMP_Text CardName;
    public Image CostOrb;
    public TMP_Text CostText;
    public TMP_Text HandNumText;

    [SerializeField]
    CardEffectScript _CardEffect;
    public CardEffectScript CardEffect { get=> _CardEffect; }

    Vector3 DiscardDeckPos;
    Vector3 BattleDeckPos;
    Vector3 CardUsedPos;

    GameObject BattleUI;

    int HandNum;

    Vector3 originPos;



    private void Awake()
    {
        CharStat = MainSceneController.Instance.Character.GetComponentInChildren<CharacterStat>();
        UICanvas = GameObject.Find("Canvas");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetDescription(BattleCardData.Data);
    }


    //여러 사용하는 위치들 저장
    public void SetPos(Vector3 DeckPos,Vector3 Discard,Vector3 cardUsedPos,GameObject battleUI)
    {
        BattleDeckPos = DeckPos;
        DiscardDeckPos = Discard;
        CardUsedPos = cardUsedPos;
        BattleUI = battleUI;
    }

    //카드 기본적인 이미지나 기본값 저장
    public void SetCardUI(CardData card)
    {

        switch (card.CardType)
        {
            case CardType.Attack:
                CardTypeText.text = "Attack";
                CardBackground.sprite = CardBackgroundRes[((int)card.CharType) * 3 + 1];
                RareFrame.sprite = RareFrameRes[((int)(card.CardType-1) * 3) + (int)card.Rarity];
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
        CardName.text = card.CardName;

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

    public void ShowCard()
    {
        TotalCard.transform.localRotation = Quaternion.identity;
    }


    public void SetOriginPos(Vector3 pos)
    {
        originPos = pos;
    }
    //정렬에 맞춰서 회전
    public void CardAlignRotate(float z)
    {
        TotalCard.transform.eulerAngles = new Vector3(0, 0, z);
    }
    //UI랑 battleData의 list번호를 가짐
    public void SetHandNum(int num)
    {
        HandNumText.text = (num + 1).ToString();
    }


    //타겟 종류에 따라 다음 함수를 정해줌

    public void DestroyCard()
    {
        Destroy(this.gameObject);
    }

    public void UseCard(HandCardStateDel Del, HandCardState state)
    {
        StartCoroutine(MoveToUseCardPos(Del, state));
    }
    //사용된 카드 이동
    IEnumerator MoveToUseCardPos(HandCardStateDel Del, HandCardState state)
    {
        transform.SetParent(BattleUI.transform);
        while (Vector3.Distance(transform.localPosition, CardUsedPos) > 1.0f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, CardUsedPos, 70.0f);
            yield return null;
        }
        transform.localPosition = CardUsedPos;

        yield return new WaitForSeconds(0.1f);
        Del(state);
        
    }

    public void DiscardCard()
    {
        StartCoroutine(MoveToDiscard());
    }
    //버려지는 카드 이동
    IEnumerator MoveToDiscard()
    {
        transform.SetParent(BattleUI.transform);
        Quaternion tmp = Quaternion.FromToRotation(transform.localPosition, DiscardDeckPos);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        while (Quaternion.Angle(transform.localRotation,tmp)>1.0f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, tmp, 20.0f);
            yield return null;
        }
        while (Vector3.Distance(transform.localPosition, DiscardDeckPos) > 1.0f)
        {
            transform.localPosition=Vector3.MoveTowards(transform.localPosition, DiscardDeckPos, 100.0f);
            yield return null;
        }
        DestroyCard();
    }
    void SetDescription(CardData data)
    {
        
        string tmp="";
        int tmpInt = 0;
        Color tmpColor;

        if (data.EnchantCount > 0)
        {
            CardName.color = TextColor[1];
        }
        CardName.text = data.CardName + data.Enchant.EnchantCardName;


        for (int i = 0; i < data.Action.Count; ++i)
        {
            switch (data.Action[i].Type)
            {
                case AbilityType.Attack:
                    tmpInt = AttackManager.Instance.UseAttack(MainSceneController.Instance.Character, data.GetTarget(), data.Action[i].SkillEffect, data.Action[i].AbilityKey, data.Action[i].Value, false);
                    break;
                case AbilityType.Skill:
                    tmpInt = SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, data.GetTarget(), data.Action[i].AbilityKey, data.Action[i].Value, false);
                    break;
                case AbilityType.Power:
                    tmpInt = PowerManager.Instance.AssginBuff(data.GetTarget(), data.Action[i].variety, data.Action[i].Value, false);
                    break;
            }
            if (Mathf.Abs(data.BaseValue[i]) > tmpInt)
            {
                tmpColor = TextColor[1];
            }
            else if(Mathf.Abs(data.BaseValue[i]) == tmpInt)
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

                for (int j = 0; j < tmpInt; ++j)
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

        int[] RGB = { (int)(color.r * 255)/16 , (int)(color.r * 255)%16, (int)(color.g * 255)/16, (int)(color.g * 255)%16, (int)(color.b * 255)/16, (int)(color.r * 255)%16 };

        for(int i = 0; i < RGB.Length; ++i)
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
}
