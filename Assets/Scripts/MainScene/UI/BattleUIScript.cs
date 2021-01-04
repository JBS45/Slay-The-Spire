using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIScript : MonoBehaviour, ICardObserver
{

    public GameObject ButtonRes;

    public EnergyOrb EnergyOrb;

    int MaxEnergy = 3;
    int CurrEnergy;
    public float Length;
    public float TermPerCard;
    BattleData m_BattleData;

    public GameObject CardRes;
    List<GameObject> HandCard;

    public GameObject BattleDeckButton;
    public GameObject DiscardDeckButton;
    public GameObject ExtinctiontButton;
    public Transform HandPoint;


    private void Awake()
    {
        HandCard = new List<GameObject>();
        m_BattleData = MainSceneController.Instance.m_BattleData;
        m_BattleData.Attach(this);
        Length = 800.0f;
        UIInit();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CardAlign()
    {
        int TotalCardNum = HandCard.Count;
        TermPerCard = Length / TotalCardNum;

        //홀수
        if (TotalCardNum % 2 == 1)
        {
            for(int i= 0; i<TotalCardNum; ++i)
            {
                float delta = TermPerCard * (i - (TotalCardNum / 2));
                HandCard[i].transform.localPosition = new Vector3(delta, 0, 0);
                HandCard[i].GetComponent<BattleCardUIScript>().SetOriginPos(HandCard[i].transform.localPosition);
                
            }

            HandCard[TotalCardNum / 2].transform.localPosition = Vector3.zero;
        }
        else
        {
            for (int i = 0; i < TotalCardNum/2; ++i)
            {
                float delta = TermPerCard * (i - (TotalCardNum / 2));
                HandCard[i].transform.localPosition = new Vector3(-TermPerCard/2 + delta, 0, 0);
                HandCard[i].GetComponent<BattleCardUIScript>().SetOriginPos(HandCard[i].transform.localPosition);
            }
            for(int i = TotalCardNum / 2; i < TotalCardNum; ++i)
            {
                float delta = TermPerCard * (i - (TotalCardNum / 2));
                HandCard[i].transform.localPosition = new Vector3(TermPerCard / 2 + delta, 0, 0);
                HandCard[i].GetComponent<BattleCardUIScript>().SetOriginPos(HandCard[i].transform.localPosition);
            }
        }
       
    }
    public void UIInit()
    {
        //함수에 버튼 눌렀을시 덱을 보여주는 함수를 넣어야함
        BattleDeckButton.GetComponent<BattleDeckButton>().ButtonSetting(() => { Debug.Log("Deck"); });

        //함수에 버튼 눌렀을시 덱을 보여주는 함수를 넣어야함
        DiscardDeckButton.GetComponent<BattleDeckButton>().ButtonSetting(() => { Debug.Log("Discard"); });

        ExtinctiontButton.GetComponent<BattleDeckButton>().ButtonSetting(() => { Debug.Log("Extinct"); });
        ExtinctiontButton.GetComponent<BattleDeckButton>().DisEnableButton(false);
        EnergyOrb.OrbImageInit(MainSceneController.Instance.m_PlayerInfo.GetCharacterType());
    }


    public void UpdateData(int Deck, int Discard, int Extinction, int Hand,int currEnergy)
    {
        BattleDeckButton.GetComponent<BattleDeckButton>().CountUpdate(Deck);
        DiscardDeckButton.GetComponent<BattleDeckButton>().CountUpdate(Discard);
        ExtinctiontButton.GetComponent<BattleDeckButton>().CountUpdate(Extinction);
        if (Extinction >= 1)
        {
            ExtinctiontButton.GetComponent<BattleDeckButton>().DisEnableButton(true);
        }
        CurrEnergy = currEnergy;
        EnergyOrb.EnergyCharge(CurrEnergy, MaxEnergy);
    }
    public void DrawCard(CardAsset cardAsset)
    {
        GameObject obj = Instantiate(CardRes);
        obj.transform.SetParent(HandPoint);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<BattleCardUIScript>().SetCardUI(cardAsset);
        HandCard.Add(obj);
        CardAlign();
    }
}
