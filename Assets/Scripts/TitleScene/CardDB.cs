﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDB : MonoBehaviour
{
    static CardDB _instance = null;
    public static CardDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CardDB>();
            }
            return _instance;
        }
    }

    public CardAsset[] Neutral;
    public CardAsset[] IronClad;

    public List<CardAsset[]> m_CardDB;
    // Start is called before the first frame update
    private void Awake()
    {
        m_CardDB = new List<CardAsset[]>();
        m_CardDB.Add(Neutral);
        m_CardDB.Add(IronClad);
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CardAsset FindCard(CharacterType CharType,string CardName)
    {
        CardAsset result;

        foreach(var CardData in m_CardDB[(int)CharType])
        {
            if (CardData.CardName.Equals(CardName))
            {
                result = CardData;
                return result;
            }
        }


        return null;
    }
}