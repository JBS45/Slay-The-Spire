using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardWindow : MonoBehaviour
{
    bool IsGold;
    bool IsPotion;
    bool IsCard;

    [SerializeField]
    GameObject RewardBar;
    [SerializeField]
    TMP_Text Text;

    List<GameObject> RewardButtonList;

    [SerializeField]
    GameObject RewardButtonRes;

    [SerializeField]
    Transform Background;

    [SerializeField]
    GameObject ProceedButton;

    Vector3 Target;
    [SerializeField]
    TMP_Text ProceedText;

    RewardManager Reward;

    GameObject AddCard;

    private void Awake()
    {
        
        IsGold = false;
        IsPotion = false;
        IsCard = false;
        Target = new Vector3(660.0f, -240.0f, 0);

        RewardButtonList = new List<GameObject>();

    }
    private void OnEnable()
    {
        WindowInit();
        CheckButtonCount();
    }
    // Start is called before the first frame update
    void Start()
    {
        ProceedButton.GetComponentInChildren<Button>().onClick.AddListener(() => { MainSceneController.Instance.GetUIControl().OpenMapProgress(); });
        AddCard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckButtonCount();
    }

    public void WindowInit()
    {
        int rand = Random.Range(0, 10);
        if (rand < 5)
        {
            Text.text = "전리품!";
        }
        else
        {
            Text.text = "보상!";
        }
        RewardBar.transform.localPosition = new Vector3(0, 200.0f, 0);



        StartCoroutine(ProceedButtonMove());
        StartCoroutine(InitRewardBarMove());
    }
    public void BattleReward(RewardManager reward)
    {
        Reward = reward;
        
        //골드
        GameObject tmp = Instantiate(RewardButtonRes);

        tmp.transform.SetParent(Background);
        tmp.transform.localScale = Vector3.one;
        tmp.GetComponent<RewardButtonScript>().SetGoldReward(RewardType.Gold, reward.RandomGoldGenerator(20));
        RewardButtonList.Add(tmp);

        /*if (Reward.Potion != PotionType.None)
        {
            tmp = Instantiate(RewardButtonRes);
            tmp.transform.SetParent(Background);
            tmp.transform.localScale = Vector3.one;
            tmp.GetComponent<RewardButtonScript>().SetReward(RewardType.Potion, 0, PotionType.None, this.gameObject, AddCard,CheckButtonCount);
            RewardButtonList.Add(tmp);
        }
        */
        tmp = Instantiate(RewardButtonRes);
        tmp.transform.SetParent(Background);
        tmp.transform.localScale = Vector3.one;
        tmp.GetComponent<RewardButtonScript>().SetCardReward(RewardType.Card, reward.CardSelector(CharacterType.Ironclad, 3), this.gameObject);
        RewardButtonList.Add(tmp);

        //Addwindow
        MainSceneController.Instance.UIControl.MakeAddReward();
        AddCard = MainSceneController.Instance.UIControl.AddRewardWinodow;
        MakeAddCardWindow();
    }
    public void RemoveAddCard()
    {
        Destroy(AddCard);
    }
    IEnumerator ProceedButtonMove()
    {
        
        while (Vector3.Distance(ProceedButton.transform.localPosition, Target) > 1.0f)
        {
            ProceedButton.transform.localPosition = Vector3.MoveTowards(ProceedButton.transform.localPosition, Target, 10.0f);
            yield return null;
        }
        ProceedButton.transform.localPosition = Target;
    }

    IEnumerator InitRewardBarMove()
    {
        Vector3 target = new Vector3(0, 280.0f, 0);

        while (Vector3.Distance(RewardBar.transform.localPosition, target) > 1.0f)
        {
            RewardBar.transform.localPosition = Vector3.Lerp(RewardBar.transform.localPosition, target, 5f*Time.deltaTime);
            yield return null;
        }
        RewardBar.transform.localPosition = target;
    }
    void MakeAddCardWindow()
    {
        AddCard.GetComponent<AddCardWindow>().Setting(MainSceneController.Instance.Reward.CardList, WindowType.Reward);
        AddCard.GetComponent<AddCardWindow>().SetCardButton(RewardButtonList[RewardButtonList.Count - 1]);
        AddCard.GetComponent<AddCardWindow>().SetButtonEvent(
                    () => {
                        this.gameObject.SetActive(true);
                        AddCard.SetActive(false);
                    });
    }
    public void CheckButtonCount()
    {
        RewardButtonList.RemoveAll(item => item == null);
        if (RewardButtonList.Count == 1)
        {
            switch (RewardButtonList[0].GetComponent<RewardButtonScript>().Type)
            {
                case RewardType.Gold:
                    ProceedText.text = "골드 넘기기";
                    break;
                case RewardType.Potion:
                    ProceedText.text = "포션 넘기기";
                    break;
                case RewardType.Card:
                    ProceedText.text = "카드 넘기기";
                    break;
            }
        }
        else if(RewardButtonList.Count == 0)
        {
            ProceedText.text = "진행";
        }
    }
}
