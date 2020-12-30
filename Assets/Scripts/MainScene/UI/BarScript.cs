using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarScript : MonoBehaviour
{
    public TMP_Text m_CharName;
    public TMP_Text m_HPText;
    public TMP_Text m_GoldText;
    public GameObject[] Potions;
    public Image m_FloorImage;
    public TMP_Text m_FloorText;
    public TMP_Text m_DeckCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BarInit(string CharName,int MaxPotions, int CurrentHp, int MaxHP, int CurrentGold, int Floor ,int DeckCount)
    {
        m_CharName.text = CharName;
        foreach(var potion in Potions)
        {
            potion.SetActive(false);
        }
        for(int i = 0; i < MaxPotions; ++i)
        {
            Potions[i].SetActive(true);
        }
        m_HPText.text = CurrentHp + "/" + MaxHP;
        m_GoldText.text = CurrentGold.ToString();
        if (Floor > 0)
        {
            m_FloorImage.enabled = true;
            m_FloorText.text = Floor.ToString();
        }
        else
        {
            m_FloorImage.enabled = false;
            m_FloorText.enabled = false;
        }
        m_DeckCount.text = DeckCount.ToString();
    }

}
