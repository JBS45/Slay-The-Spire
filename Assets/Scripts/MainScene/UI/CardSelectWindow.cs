using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectWindow : MonoBehaviour
{
    [SerializeField]
    GameObject CardRes;
    GameObject[] Cards = new GameObject[2];

    [SerializeField]
    Transform Content;

    CardData Data;
    CardData UpgradeData;

    [SerializeField]
    GameObject CancelButton;
    Vector3 CancelButtonOriginVector;
    [SerializeField]
    GameObject ConfirmButton;
    Vector3 ConfirmButtonOriginVector;


    private void Awake()
    {
        CancelButtonOriginVector = CancelButton.transform.localPosition;
        ConfirmButtonOriginVector = ConfirmButton.transform.localPosition;

        StartCoroutine(OnEnableCancelButton());
        StartCoroutine(OnEnableConfirmButton());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWindowType(CardData data,WindowType type)
    {
        switch (type)
        {
            case WindowType.Enchant:
                Enchant(data);
                break;
            case WindowType.Remove:
                Remove(data);
                break;
        }
    }

    void Enchant(CardData data)
    {
        Data = data;
        UpgradeData = new CardData(data);
        UpgradeData.CardEnchant();

        Cards[0] = Instantiate(CardRes);
        Cards[0].transform.SetParent(Content);
        Cards[0].transform.localScale = Vector3.one;
        Cards[0].transform.localPosition = new Vector3(-300, 0, 0);
        Cards[0].GetComponent<CardUIScript>().SetCardUI(Data);

        Cards[1] = Instantiate(CardRes);
        Cards[1].transform.SetParent(Content);
        Cards[1].transform.localScale = Vector3.one;
        Cards[1].transform.localPosition = new Vector3(300, 0, 0);
        Cards[1].GetComponent<CardUIScript>().SetCardUI(UpgradeData);

        GetComponent<RectTransform>().sizeDelta = new Vector2(MainSceneController.Instance.UIControl.UICanvas.pixelRect.width, MainSceneController.Instance.UIControl.UICanvas.pixelRect.height);
        ConfirmButton.GetComponentInChildren<Button>().onClick.AddListener(EnchantConfirm);

    }
    void Remove(CardData data)
    {
        Data = data;

        Cards[0] = Instantiate(CardRes);
        Cards[0].transform.SetParent(Content);
        Cards[0].transform.localScale = Vector3.one;
        Cards[0].transform.localPosition = new Vector3(0, 0, 0);
        Cards[0].GetComponent<CardUIScript>().SetCardUI(Data);

        GetComponent<RectTransform>().sizeDelta = new Vector2(MainSceneController.Instance.UIControl.UICanvas.pixelRect.width, MainSceneController.Instance.UIControl.UICanvas.pixelRect.height);
        ConfirmButton.GetComponentInChildren<Button>().onClick.AddListener(RemoveConfirm);
    }

    IEnumerator OnEnableConfirmButton()
    {
        ConfirmButton.transform.localPosition = ConfirmButtonOriginVector;
        Vector3 target = ConfirmButtonOriginVector - new Vector3(250, 0, 0);
        while (Vector3.Distance(ConfirmButton.transform.localPosition, target) > 1.0f)
        {
            ConfirmButton.transform.localPosition = Vector3.MoveTowards(ConfirmButton.transform.localPosition, target, 10.0f);
            yield return null;
        }
        ConfirmButton.transform.localPosition = target;
    }
    IEnumerator OnEnableCancelButton()
    {
        CancelButton.transform.localPosition = CancelButtonOriginVector;
        Vector3 target = CancelButtonOriginVector + new Vector3(250, 0, 0);
        while (Vector3.Distance(CancelButton.transform.localPosition, target) > 1.0f)
        {
            CancelButton.transform.localPosition = Vector3.MoveTowards(CancelButton.transform.localPosition, target, 10.0f);
            yield return null;
        }
        CancelButton.transform.localPosition = target;
    }

    public void Cancel()
    {
        Destroy(this.gameObject);
    }
    public void EnchantConfirm()
    {
        Data.CardEnchant();
        if (MainSceneController.Instance.UIControl.GetCurUI().GetComponent<FireCampWindowScript>() != null)
        {
            MainSceneController.Instance.UIControl.GetCurUI().GetComponent<FireCampWindowScript>().FireOff();
        }
        Destroy(this.gameObject);
    }
    public void RemoveConfirm()
    {
        MainSceneController.Instance.PlayerData.RemoveCard(Data);
        MainSceneController.Instance.PlayerData.CurrentMoney -= MainSceneController.Instance.PlayerData.RemoveCardGold();
        MainSceneController.Instance.PlayerData.CardRemoveCount++;
        MainSceneController.Instance.PlayerData.Notify();
        if (MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>() != null)
        {
            MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>().UseRemove();
        }
        Destroy(this.gameObject);
    }
}
