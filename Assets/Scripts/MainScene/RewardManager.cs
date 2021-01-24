using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    int RewardGold;
    public int Gold { get => RewardGold; }
    PotionType RewardPotion;
    public PotionType Potion { get => RewardPotion; }
    List<CardData> RewardCardList;
    public List<CardData> CardList { get => RewardCardList; }


    int Basic=60;
    int UnCommon=30;
    int Rare=10;

    int RewardCardCount;

    private void Awake()
    {
        RewardCardList = new List<CardData>();
        RewardCardCount = 3;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        RewardGold = 15;
        RewardPotion = PotionType.None;
        RewardCardList.Clear();
    }
    public void BasicReward()
    {
        int goldRandom = Random.Range(-5, 6);
        int IsGetPotionRandom = Random.Range(0, 10);
        int cardMaxNum = CardDB.Instance.IronClad.Card.Count;
        List<int> randomCard = new List<int>();

        RewardGold = 15 + goldRandom;
        if (IsGetPotionRandom < 4)
        {
            RewardPotion=PotionSelector();
        }
        CardSelector(ref RewardCardList);
    }

    PotionType PotionSelector()
    {
        int potionNum = Random.Range(1,System.Enum.GetValues(typeof(PotionType)).Length);

        return (PotionType)potionNum;
    }
    void CardSelector(ref List<CardData> list)
    {
        List<CardAsset> BasicList = CardDB.Instance.IronClad.Card.FindAll(item => item.Rarity == RarityOptions.Basic);
        List<CardAsset> UnCommonList = CardDB.Instance.IronClad.Card.FindAll(item => item.Rarity == RarityOptions.Common);
        List<CardAsset> RarityList = CardDB.Instance.IronClad.Card.FindAll(item => item.Rarity == RarityOptions.Rare);

        int RaritySelector;

        for(int i=0;i< RewardCardCount;)
        {
            RaritySelector = Random.Range(0, 100);
            if (BasicList.Count > 0 && RaritySelector < Basic)
            {
                int random = Random.Range(0, BasicList.Count);
                CardData tmp = new CardData(BasicList[random]);
                BasicList.RemoveAt(random);
                list.Add(tmp);
                i++;
            }
            else if (UnCommonList.Count>0 && RaritySelector < Basic + UnCommon)
            {
                int random = Random.Range(0, UnCommonList.Count);
                CardData tmp = new CardData(UnCommonList[random]);
                UnCommonList.RemoveAt(random);
                list.Add(tmp);
                i++;
            }
            else if(RarityList.Count>0 && RaritySelector < Basic + UnCommon + Rare)
            {
                int random = Random.Range(0, RarityList.Count);
                CardData tmp = new CardData(RarityList[random]);
                RarityList.RemoveAt(random);
                list.Add(tmp);
                i++;
            }


        }
    }
}
