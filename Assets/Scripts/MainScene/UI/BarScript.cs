using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarScript : MonoBehaviour,IObservers
{
    public TMP_Text m_CharName;
    public TMP_Text m_HPText;
    public TMP_Text m_GoldText;
    public GameObject[] Potions;
    public Image m_FloorImage;
    public TMP_Text m_FloorText;
    public TMP_Text m_DeckCount;



    PlayerDataAsset m_PlayerData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BarInit(PlayerDataAsset playerdata)
    {
        m_PlayerData = playerdata;
        m_CharName.text = CharDB.Instance.GetCharacterAsset(m_PlayerData.CharType).CharacterName;
        m_PlayerData.Attach(this);
        UpdateData();
    }
    public void UpdateData()
    {
        foreach (var potion in Potions)
        {
            potion.SetActive(false);
        }
        for (int i = 0; i < m_PlayerData.MaxPotions; ++i)
        {
            Potions[i].SetActive(true);
        }
        m_HPText.text = m_PlayerData.CurrentHp.ToString() + "/" + m_PlayerData.MaxHp.ToString();
        m_GoldText.text = m_PlayerData.CurrentMoney.ToString();
        if (m_PlayerData.CurrentFloor > 0)
        {
            m_FloorImage.enabled = true;
            m_FloorText.enabled = true;
            m_FloorText.text = m_PlayerData.CurrentFloor.ToString();
        }
        else
        {
            m_FloorImage.enabled = false;
            m_FloorText.enabled = false;
        }
        m_DeckCount.text = m_PlayerData.OriginDecks.Count.ToString();
    }

}
