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

    private void Awake()
    {
        RewardCardList = new List<CardData>();
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
    public int RandomGoldGenerator(int Amount)
    {
        int tmp = Amount + Random.Range(-(Amount / 4), (Amount / 4) + 1);
        RewardGold = tmp;
        return tmp;
    }
    public List<PotionType> PotionSelector(int Count)
    {
        List<PotionType> tmp = new List<PotionType>();
        int potionNum;
        for(int i=0; i < Count; )
        {
            potionNum = Random.Range(1, System.Enum.GetValues(typeof(PotionType)).Length);
            if (tmp.Exists(item => item != (PotionType)potionNum))
            {
                tmp.Add((PotionType)potionNum);
                ++i;
            }
            
        }


        return tmp;
    }

    PotionType PotionSelector()
    {
        int potionNum = Random.Range(1,System.Enum.GetValues(typeof(PotionType)).Length);

        return (PotionType)potionNum;
    }
    public List<CardData> CardSelector(CharacterType type,int count)
    {
        List<CardData> tmpList = new List<CardData>();
        List<CardAsset> BasicList = new List<CardAsset>();
        List<CardAsset> UnCommonList = new List<CardAsset>();
        List<CardAsset> RarityList = new List<CardAsset>();

        switch (type) {
            case CharacterType.None:
                BasicList = CardDB.Instance.Neutral.Card.FindAll(item => item.Rarity == RarityOptions.Basic);
                UnCommonList = CardDB.Instance.Neutral.Card.FindAll(item => item.Rarity == RarityOptions.Common);
                RarityList = CardDB.Instance.Neutral.Card.FindAll(item => item.Rarity == RarityOptions.Rare);
                break;
            case CharacterType.Ironclad:
                BasicList = CardDB.Instance.IronClad.Card.FindAll(item => item.Rarity == RarityOptions.Basic);
                UnCommonList = CardDB.Instance.IronClad.Card.FindAll(item => item.Rarity == RarityOptions.Common);
                RarityList = CardDB.Instance.IronClad.Card.FindAll(item => item.Rarity == RarityOptions.Rare);
                break;
        }


        int RaritySelector;

        for(int i=0;i< count;)
        {
            RaritySelector = Random.Range(0, 100);
            if (BasicList.Count > 0 && RaritySelector < Basic)
            {
                int random = Random.Range(0, BasicList.Count);
                CardData tmp = new CardData(BasicList[random]);
                BasicList.RemoveAt(random);
                tmpList.Add(tmp);
                i++;
            }
            else if (UnCommonList.Count>0 && RaritySelector < Basic + UnCommon)
            {
                int random = Random.Range(0, UnCommonList.Count);
                CardData tmp = new CardData(UnCommonList[random]);
                UnCommonList.RemoveAt(random);
                tmpList.Add(tmp);
                i++;
            }
            else if(RarityList.Count>0 && RaritySelector < Basic + UnCommon + Rare)
            {
                int random = Random.Range(0, RarityList.Count);
                CardData tmp = new CardData(RarityList[random]);
                RarityList.RemoveAt(random);
                tmpList.Add(tmp);
                i++;
            }


        }

        RewardCardList = tmpList;

        return tmpList;
    }
}
