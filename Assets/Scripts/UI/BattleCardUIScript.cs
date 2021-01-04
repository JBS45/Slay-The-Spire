using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class BattleCardUIScript : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,ICardObserver
{
    public Sprite[] CardBackgroundRes;
    public Sprite[] RareFrameRes;
    public Sprite[] CardNameBanner;
    public Sprite[] CostOrbRes;

    CardAsset m_Data;
    bool IsEnable;
    bool IsClicked;
    bool IsInRange;
    int CardCost;

    GameObject UICanvas;

    int sibliingIndex;

    public GameObject AfterImage;
    Image[] AfterImages;
    public Color HighlightColor;

    public GameObject TotalCard;
    public Image Highlight;
    public Image CardBackground;
    public TMP_Text CardDescription;
    public Image CardImage;
    public Image RareFrame;
    public TMP_Text CardTypeText;
    public Image NameBanner;
    public TMP_Text CardName;
    public Image CostOrb;
    public TMP_Text CostText;
    public TMP_Text HandNum;

    Vector3 originPos;

    IEnumerator AfterImageCoroutine;

    private void Awake()
    {
        IsClicked = false;
        IsEnable = true;
        IsInRange = false;
        AfterImages = AfterImage.GetComponentsInChildren<Image>();
        AfterImageCoroutine = AfterImageEffect();

        UICanvas = GameObject.Find("Canvas");
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
            if (AfterImageCoroutine != null)
            {
                AfterImageCoroutine.MoveNext();
            }
        }
        CardPositionCheck();

    }
    void CardPositionCheck()
    {
        if (IsClicked)
        {
            Vector3 tmp = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            transform.localPosition = new Vector3(Camera.main.scaledPixelWidth * (tmp.x - 0.5f), Camera.main.scaledPixelHeight * tmp.y);
            if (transform.localPosition.y > Camera.main.scaledPixelHeight / 3)
            {
                if (IsInRange == false)
                {
                    IsInRange = true;
                    AfterImageCoroutine = AfterImageEffectInRange();
                }

            }
            else
            {
                IsInRange = false;
            }
        }
    }

    public void SetCardUI(CardAsset cardAsset)
    {
        m_Data = cardAsset;

        switch (m_Data.cardType)
        {
            case CardType.Attack:
                CardTypeText.text = "Attack";
                CardBackground.sprite = CardBackgroundRes[((int)m_Data.charType) * 3 + 1];
                RareFrame.sprite = RareFrameRes[((int)m_Data.cardType * 3) + (int)m_Data.Rarity];
                break;
            case CardType.Skill:
                CardTypeText.text = "Skill";
                CardBackground.sprite = CardBackgroundRes[((int)m_Data.charType) * 3 + 2];
                RareFrame.sprite = RareFrameRes[((int)m_Data.cardType * 3) + (int)m_Data.Rarity];
                break;
            case CardType.Power:
                CardTypeText.text = "Power";
                CardBackground.sprite = CardBackgroundRes[((int)m_Data.charType) * 3 + 3];
                RareFrame.sprite = RareFrameRes[((int)m_Data.cardType * 3) + (int)m_Data.Rarity];
                break;
            case CardType.Condition:
                CardTypeText.text = "Condition";
                CardBackground.sprite = CardBackgroundRes[2];
                RareFrame.sprite = RareFrameRes[((int)m_Data.cardType * 3) + (int)m_Data.Rarity];
                break;
            case CardType.Curse:
                CardTypeText.text = "Curse";
                CardBackground.sprite = CardBackgroundRes[0];
                RareFrame.sprite = RareFrameRes[((int)m_Data.cardType * 3) + (int)m_Data.Rarity];
                break;
        }
        CardDescription.text = m_Data.Description;
        CardImage.sprite = m_Data.CardImage;
        CardName.text = m_Data.CardName;
        CardCost = m_Data.Cost;

        switch (m_Data.charType)
        {
            case CharacterType.None:
                CostOrb.sprite = CostOrbRes[0];
                break;
            case CharacterType.Ironclad:
                CostOrb.sprite = CostOrbRes[1];
                break;
        }

        CostText.text = CardCost.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsInRange == false)
        {
            if (IsEnable && (!IsClicked))
            {
                IsClicked = true;
                sibliingIndex = transform.GetSiblingIndex();
                transform.SetAsLastSibling();

            }
            else if (IsEnable && IsClicked)
            {
                IsClicked = false;
                transform.localPosition = originPos;
                transform.SetSiblingIndex(sibliingIndex);
            }
        }
        else
        {
            if (IsEnable)
            {
                IsClicked = false;
                transform.SetSiblingIndex(sibliingIndex);
                //사용됨
            }
        }
    }
    public void ChangeCardCost(int cost)
    {
        CardCost = cost;
    }
    public void UpdateData(int deck, int discard, int extinction, int hand, int currEnergy)
    {
        if (currEnergy >= CardCost)
        {
            IsEnable = true;
            Highlight.enabled = true;
        }
        else
        {
            IsEnable = false;
            Highlight.enabled = false;
        }
    }
    public void DrawCard(CardAsset cardAsset)
    {

    }
    public void SetOriginPos(Vector3 pos)
    {
        originPos = pos;
    }
    
    IEnumerator AfterImageEffect()
    {
        float min = 1.05f;
        float max = 1.15f;

        for(int i=0;i< AfterImages.Length; ++i)
        {
            AfterImages[i].rectTransform.localScale = new Vector2(min+(0.5f*i), min + (0.5f * i));
            AfterImages[i].color = new Color(HighlightColor.r, HighlightColor.g, HighlightColor.b, 1.0f);
        }
        while (true)
        {
            for (int i = 0; i < AfterImages.Length; ++i)
            {
                AfterImages[i].rectTransform.localScale += new Vector3(Time.deltaTime*0.2f, Time.deltaTime * 0.2f);
                AfterImages[i].color -= new Color(0, 0, 0, Time.deltaTime*2);
                if (AfterImages[i].rectTransform.localScale.x >= max)
                {
                    AfterImages[i].rectTransform.localScale = new Vector3(min, min);
                    AfterImages[i].color = new Color(HighlightColor.r, HighlightColor.g, HighlightColor.b,1.0f);
                }
            }
            yield return null;
        }
    }
    IEnumerator AfterImageEffectInRange()
    {
        float min = 1.05f;
        float max = 1.2f;

        int Last = AfterImages.Length - 1;
        for (int i = 0; i < AfterImages.Length; ++i)
        {
            AfterImages[i].rectTransform.localScale = new Vector2(min + (0.5f * i), min + (0.5f * i));
            AfterImages[i].color = Color.white;
        }
        while (AfterImages[Last].rectTransform.localScale.x>=max)
        {
            for (int i = 0; i < AfterImages.Length; ++i)
            {
                AfterImages[i].rectTransform.localScale += new Vector3(Time.deltaTime * 0.5f, Time.deltaTime * 0.5f);
                AfterImages[i].color -= new Color(0, 0, 0, Time.deltaTime * 2);
            }
            yield return null;
        }
        AfterImageCoroutine = AfterImageEffect();
    }

}
