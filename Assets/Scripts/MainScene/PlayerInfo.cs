using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    public List<CardAsset> OriginDeck;

    int MaxHp;
    int CurrentHp;
    int CurrentFloor;
    int CurrentMoney;
    int MaxPotions;

    //유물
    //덱
    //포션
    private void Awake()
    {
        OriginDeck = new List<CardAsset>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetMaxHp()
    {
        return MaxHp;
    }
    public int GetCurHp()
    {
        return CurrentHp;
    }
    public int GetCurFloor()
    {
        return CurrentFloor ;
    }
    public int GetCurrentMoney()
    {
        return CurrentMoney;
    }
    public int GetMaxPotions()
    {
        return MaxPotions;
    }

    public void GameDataInit()
    {
        //저장 정보가 없으면
        MaxHp = CharDB.Instance.GetCharacterAsset().Hp;
        CurrentHp = CharDB.Instance.GetCharacterAsset().Hp;
        CurrentMoney = CharDB.Instance.GetCharacterAsset().Gold;
        CurrentFloor = 0;
        MaxPotions = 3;

        //유물 기본

        OriginDeck = new List<CardAsset>();
        for(int i = 0; i < 5; ++i) {
            OriginDeck.Add(CardDB.Instance.FindCard(CharacterType.Ironclad, "Strike"));
        }
        for (int i = 0; i < 5; ++i)
        {
            OriginDeck.Add(CardDB.Instance.FindCard(CharacterType.Ironclad, "Defend"));
        }
        //포션 없음

        //저장 정보가 있으면
    }

}
