using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class CardUIScript : MonoBehaviour
{
    public Sprite[] CardBackgroundRes;
    public Sprite[] RareFrameRes;
    public Sprite[] CardNameBanner;
    public Sprite[] CostOrbRes;

    CardAsset m_Data;

    public Image CardBackground;
    public TMP_Text CardDescription;
    public Image CardImage;
    public Image RareFrame;
    public TMP_Text CardTypeText;
    public Image NameBanner;
    public TMP_Text CardName;
    public Image CostOrb;
    public TMP_Text CostText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        switch (m_Data.charType)
        {
            case CharacterType.None:
                CostOrb.sprite = CostOrbRes[0];
                break;
            case CharacterType.Ironclad:
                CostOrb.sprite = CostOrbRes[1];
                break;
        }

        CostText.text = m_Data.Cost.ToString();
    }
}
