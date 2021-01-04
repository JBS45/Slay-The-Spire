using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    CharacterType CharType;
    [SerializeField]
    List<CardAsset> OriginDeck;

    int MaxHp;
    int CurrentHp;
    int CurrentFloor;
    int CurrentMoney;
    int MaxPotions;
    int DrawPerTurn = 5;
    int Seed;
    //유물
    //덱
    //포션
    private void Awake()
    {
        OriginDeck = new List<CardAsset>();
        //원래는 이 시점이 아니라 맵이 생성 될때 
        Seed = Random.Range(0, int.MaxValue);
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
    public int GetDrawPerTurn()
    {
        return DrawPerTurn;
    }
    public int GetSeed()
    {
        return Seed;
    }
    public void SetSeed(int num)
    {
        Seed = num;
    }
    public List<CardAsset> GetOriginDeck()
    {
        return OriginDeck;
    }
    public CharacterType GetCharacterType()
    {
        return CharType;
    }

    public void GameDataInit()
    {
        //저장 정보가 없으면
        CharType = CharDB.Instance.GetPlayChar();
        MaxHp = CharDB.Instance.GetCharacterAsset().Hp;
        CurrentHp = CharDB.Instance.GetCharacterAsset().Hp;
        CurrentMoney = CharDB.Instance.GetCharacterAsset().Gold;
        CurrentFloor = 0;
        MaxPotions = 3;

        //유물 기본

        for(int i=0;i< CharDB.Instance.GetCharacterAsset().StartDeck.Length; ++i)
        {
            OriginDeck.Add(CharDB.Instance.GetCharacterAsset().StartDeck[i]);
        }
        //포션 없음

        //저장 정보가 있으면
    }

}
