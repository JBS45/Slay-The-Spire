using System.Collections;
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

    [SerializeField]
    CardPool _Condition;
    public CardPool Condition { get => _Condition; }
    [SerializeField]
    CardPool _Neutral;
    public CardPool Neutral { get => _Neutral; }
    [SerializeField]
    CardPool _IronClad;
    public CardPool IronClad { get => _IronClad; }

    public List<CardAsset[]> m_CardDB;
    // Start is called before the first frame update
    private void Awake()
    {
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
        switch (CharType)
        {
            case CharacterType.Ironclad:
                if (IronClad.BaseCard.Exists(item => item.CardName == CardName))
                {
                    return IronClad.BaseCard.Find(item => item.CardName == CardName);
                }
                else if(IronClad.Card.Exists(item => item.CardName == CardName))
                {
                    return IronClad.Card.Find(item => item.CardName == CardName);
                }
                else
                {
                    return null;
                }
                break;
            case CharacterType.None:
                if (Neutral.Card.Exists(item => item.CardName == CardName))
                {
                    return Neutral.BaseCard.Find(item => item.CardName == CardName);
                }
                else if (Condition.Card.Exists(item => item.CardName == CardName))
                {
                    return Condition.Card.Find(item => item.CardName == CardName);
                }
                break;
        }


        return null;
    }
}
