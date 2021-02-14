using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopWindowScript : MonoBehaviour,IObservers
{
    [SerializeField]
    GameObject CardRes;

    [SerializeField]
    GameObject CancelBtn;
    Vector3 CancelButtonOriginVector;
    bool IsCancel;

    [SerializeField]
    GameObject ProceedBtn;
    Vector3 ProceedBtnOriginVector;
    bool IsProceed;


    List<IShopWindow> Goods;

    [SerializeField]
    Transform ClassCard;

    [SerializeField]
    Transform NeutralCard;

    [SerializeField]
    GameObject RemoveCard;

    [SerializeField]
    GameObject Carpet;
    Vector3 CarpetOriginPos;

    [SerializeField]
    GameObject Hand;
    Vector3 HandOrigin;

    Merchant MerchantScript;

    GameObject Target;

    List<GameObject> Cards;

    private void Awake()
    {
        CancelButtonOriginVector = CancelBtn.transform.localPosition;
        ProceedBtnOriginVector = ProceedBtn.transform.localPosition;
        CarpetOriginPos = Carpet.transform.localPosition;
        IsProceed = true;
        IsCancel = false;
        Goods = new List<IShopWindow>();
        HandOrigin = Hand.transform.localPosition;
        RemoveCard.GetComponent<CardRemoveScript>().ShopUISetting(MainSceneController.Instance.PlayerData.RemoveCardGold(), Detach);
        Goods.Add(RemoveCard.GetComponent<IShopWindow>());
        Cards = new List<GameObject>();
        MainSceneController.Instance.PlayerData.Attach(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {

            Vector3 tmp = new Vector3(Target.transform.parent.localPosition.x+Target.transform.localPosition.x, Target.transform.parent.localPosition.y+ Target.transform.localPosition.y, Target.transform.localPosition.z);
                
            Hand.transform.localPosition = Vector3.MoveTowards(Hand.transform.localPosition, tmp, 30.0f);
        }
        else
        {
            Hand.transform.localPosition = Vector3.MoveTowards(Hand.transform.localPosition, HandOrigin, 30.0f);
        }
    }

    public void SetClassCard(List<CardData> ClassData)
    {
        foreach (var item in ClassData)
        {
            int tmpGold = 0;
            GameObject tmp = Instantiate(CardRes);
            tmp.transform.SetParent(ClassCard);
            tmp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            tmp.GetComponent<CardUIScript>().SetCardUI(item);
            tmp.GetComponent<CardUIScript>().SetSize(0.8f);

            switch (item.Rarity)
            {
                case RarityOptions.Basic:
                    tmpGold = MainSceneController.Instance.Reward.RandomGoldGenerator(50);
                    break;
                case RarityOptions.Common:
                    tmpGold = MainSceneController.Instance.Reward.RandomGoldGenerator(100);
                    break;
                case RarityOptions.Rare:
                    tmpGold = MainSceneController.Instance.Reward.RandomGoldGenerator(150);
                    break;
            }

            tmp.GetComponent<IShopWindow>().ShopUISetting(tmpGold, Detach);
            Attach(tmp.GetComponent<IShopWindow>());
            Cards.Add(tmp);

        }
    }
    public void SetNeutralCard(List<CardData> NeutralData)
    {
        foreach (var item in NeutralData)
        {
            int tmpGold = 0;
            GameObject tmp = Instantiate(CardRes);
            tmp.transform.SetParent(NeutralCard);
            tmp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            tmp.GetComponent<CardUIScript>().SetCardUI(item);
            tmp.GetComponent<CardUIScript>().SetSize(0.8f);

            switch (item.Rarity)
            {
                case RarityOptions.Basic:
                    tmpGold = MainSceneController.Instance.Reward.RandomGoldGenerator(50);
                    break;
                case RarityOptions.Common:
                    tmpGold = MainSceneController.Instance.Reward.RandomGoldGenerator(100);
                    break;
                case RarityOptions.Rare:
                    tmpGold = MainSceneController.Instance.Reward.RandomGoldGenerator(150);
                    break;
            }

            tmp.GetComponent<IShopWindow>().ShopUISetting(tmpGold, Detach);
            Attach(tmp.GetComponent<IShopWindow>());
            Cards.Add(tmp);
        }
    }

    public void ShowShop()
    {
        Carpet.SetActive(true);
        StartCoroutine(MoveShop());
        CancelButtonMove();
        ProceedButtonMove();
        Refresh();
    }

    public void HideShop()
    {
        Carpet.SetActive(false);
        MerchantScript.Enable = true;
        CancelButtonMove();
        ProceedButtonMove();
    }
    public void Proceed()
    {
        MainSceneController.Instance.UIControl.OpenMapProgress();
    }
    void CancelButtonMove()
    {
        StartCoroutine(OnEnableCancelButton());
    }
    IEnumerator OnEnableCancelButton()
    {
        Vector3 target;
        if (IsCancel)
        {
            target = CancelButtonOriginVector;
            IsCancel = false;
        }
        else
        {
            target = CancelButtonOriginVector + new Vector3(260, 0, 0);
            IsCancel = true;
        }
        while (Vector3.Distance(CancelBtn.transform.localPosition, target) > 1.0f)
        {
            CancelBtn.transform.localPosition = Vector3.MoveTowards(CancelBtn.transform.localPosition, target, 30.0f);
            yield return null;
        }
        CancelBtn.transform.localPosition = target;
    }

    void ProceedButtonMove()
    {
        StartCoroutine(OnEnableProceedButton());
    }

    IEnumerator OnEnableProceedButton()
    {
        Vector3 target;
        if (IsProceed)
        {
            target = ProceedBtnOriginVector + new Vector3(300, 0, 0);
            IsProceed = false;
        }
        else
        {
            target = ProceedBtnOriginVector;
            IsProceed = true;
        }
        while (Vector3.Distance(ProceedBtn.transform.localPosition, target) > 1.0f)
        {
            ProceedBtn.transform.localPosition = Vector3.MoveTowards(ProceedBtn.transform.localPosition, target, 30.0f);
            yield return null;
        }
        ProceedBtn.transform.localPosition = target;
    }
    IEnumerator MoveShop()
    {
        Vector3 target = CarpetOriginPos - new Vector3(0, 1080, 0);
        Carpet.transform.localPosition = CarpetOriginPos;

        while (Vector3.Distance(Carpet.transform.localPosition, target) > 1.0f)
        {
            Carpet.transform.localPosition = Vector3.MoveTowards(Carpet.transform.localPosition, target, 60.0f);
            yield return null;
        }

        Carpet.transform.localPosition = target;
    }
    public void SetMerchant(Merchant merchant)
    {
        MerchantScript = merchant;
    }
    public void Refresh()
    {
        foreach(var item in Goods)
        {
            item.ShopRefresh();
        }
    }
    public void Attach(IShopWindow Good)
    {
        Goods.Add(Good);
    }
    public void Detach(IShopWindow Good)
    {
        Goods.Remove(Good);
        Refresh();
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }
    public void UseRemove()
    {
        RemoveCard.GetComponent<CardRemoveScript>().Use();
    }
    public void UpdateData()
    {
        Cards.RemoveAll(item => item == null);
        foreach(var card in Cards)
        {
            card.GetComponent<CardUIScript>().ShopRefresh();
        }
    }
}
