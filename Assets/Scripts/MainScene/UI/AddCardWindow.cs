using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCardWindow : MonoBehaviour
{
    [SerializeField]
    GameObject RewardBar;
    [SerializeField]
    TMP_Text Text;

    [SerializeField]
    GameObject CardRes;

    [SerializeField]
    Transform CardPos;

    GameObject CardButton;

    List<GameObject> CardList;
    public List<GameObject> Card { get => CardList; }

    [SerializeField]
    Button PassButton;

    int CardCount = 3;
    bool IsEnable;

    Delvoid DestroyDel;

    private void Awake()
    {
        Text.text = "카드 보상";
        CardList = new List<GameObject>();
        CardCount = 3;
        IsEnable = false;
        DestroyDel = DestroyWindow;
    }
    private void OnEnable()
    {
        if (IsEnable)
        {
            WindowInit();
        }
    }
    void WindowInit()
    {
        RewardBar.transform.localPosition = new Vector3(0, 200.0f, 0);
        CardList[0].transform.localPosition = Vector3.zero;
        CardList[2].transform.localPosition = Vector3.zero;

        StartCoroutine(CardInit());
        StartCoroutine(InitRewardBarMove());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setting(List<CardData> List,WindowType type)
    {
        
        for (int i = 0; i < CardCount; ++i)
        {
            GameObject tmp = Instantiate(CardRes);
            CardData TmpData = List[i];
            tmp.transform.SetParent(CardPos);
            tmp.transform.localScale = Vector3.one;
            tmp.transform.localPosition = Vector3.zero;
            tmp.GetComponent<CardUIScript>().SetCardUI(List[i]);
            tmp.GetComponent<CardUIScript>().SetButtonEvent(
                ()=>
                {
                    MainSceneController.Instance.PlayerData.AddCard(TmpData);
                    MainSceneController.Instance.Reward.transform.SetParent(MainSceneController.Instance.UIControl.UICanvas.transform);
                    MainSceneController.Instance.UIControl.GetCurUI().SetActive(true);
                    MainSceneController.Instance.UIControl.MakeCard(WindowType.Reward, 1, TmpData);
                    StartCoroutine(tmp.GetComponent<CardUIScript>().MoveToOriginDeck());
                    IsEnable = false;
                    DestroyWindow();


                });
            CardList.Add(tmp);
        }
        IsEnable = true;


        WindowInit();

    }
    IEnumerator CardInit()
    {
        Vector3 target = new Vector3(500, 0, 0);
        Vector3 Origin1 = CardList[0].transform.localPosition + target;
        Vector3 Origin2 = CardList[0].transform.localPosition - target;

        while (Vector3.Distance(CardList[0].transform.localPosition, Origin1) > 1.0f)
        {
            CardList[0].transform.localPosition = Vector3.MoveTowards(CardList[0].transform.localPosition, Origin1, 30.0f);
            CardList[2].transform.localPosition = Vector3.MoveTowards(CardList[2].transform.localPosition, Origin2, 30.0f);
            yield return null;
        }
        CardList[0].transform.localPosition = Origin1;
        CardList[2].transform.localPosition = Origin2;
    }
    IEnumerator InitRewardBarMove()
    {
        Vector3 target = new Vector3(0, 280.0f, 0);

        while (Vector3.Distance(RewardBar.transform.localPosition, target) > 1.0f)
        {
            RewardBar.transform.localPosition = Vector3.Lerp(RewardBar.transform.localPosition, target, 5f * Time.deltaTime);
            yield return null;
        }
        RewardBar.transform.localPosition = target;
    }

    public void SetButtonEvent(Delvoid del)
    {
        PassButton.onClick.AddListener(() => { del(); });
    }
    public void DestroyWindow()
    {
        MainSceneController.Instance.UIControl.AddRewardWinodow = null;
        Destroy(CardButton);
        Destroy(this.gameObject);
    }
    public void SetCardButton(GameObject obj)
    {
        CardButton = obj;
    }
}
