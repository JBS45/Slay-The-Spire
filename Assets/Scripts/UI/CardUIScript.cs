using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class CardUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    Button m_Button;

    [Header("TextColorTable")]
    [SerializeField]
    Color[] TextColor;

    WindowType m_WindowType = WindowType.None;
    bool IsEnable;
    [SerializeField]
    TrailRenderer Trail;

    float CardSize = 1.0f;

    void Awake()
    {
        Trail.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsEnable = true;
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
                break;
            case CardType.Condition:
                CardTypeText.text = "Condition";
                CardBackground.sprite = CardBackgroundRes[2];
                RareFrame.sprite = RareFrameRes[((int)card.CardType * 3) + (int)card.Rarity];
                break;
            case CardType.Curse:
                CardTypeText.text = "Curse";
                CardBackground.sprite = CardBackgroundRes[0];
                RareFrame.sprite = RareFrameRes[((int)card.CardType * 3) + (int)card.Rarity];
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

        CostText.text = card.Cost.ToString();
    }
    void SetDescription(CardData data)
    {

        string tmp = "";
        int tmpInt = 0;
        Color tmpColor;
        for (int i = 0; i < data.Action.Count; ++i)
        {
            tmpInt = data.Action[i].Value;
            if (data.Action[i].Value > tmpInt)
            {
                tmpColor = TextColor[1];
            }
            else if (data.Action[i].Value == tmpInt)
            {
                tmpColor = TextColor[0];
            }
            else
            {
                tmpColor = TextColor[2];
            }
            tmp += string.Format(data.Action[i].Decription, ColorTohexadecimal(tmpColor), tmpInt);
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
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsEnable)
        {
            transform.localScale = new Vector3(CardSize, CardSize, CardSize);
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
                del();
            });
    }
    public void SetSize(float size)
    {
        CardSize = size;
    }
}

