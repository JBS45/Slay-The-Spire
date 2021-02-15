using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardRemoveScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IShopWindow
{

    bool IsEnable;

    [SerializeField]
    Button m_Button;

    [SerializeField]
    GameObject GoldText;

    [SerializeField]
    Sprite UsedSprite;

    GameObject RemoveCardWindow;

    float CardSize;

    int Gold;

    private void Awake()
    {
        CardSize = 0.8f;
        transform.localScale = new Vector3(CardSize, CardSize, CardSize);
        IsEnable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEnable)
        {
            transform.localScale = new Vector3(1.3f * CardSize, 1.3f * CardSize, 1.3f * CardSize);
            HandEnter();
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsEnable)
        {
            transform.localScale = new Vector3(CardSize, CardSize, CardSize);
            HandExit();
        }
    }

    public void ShopUISetting(int gold, ShopWindowDelegate del)
    {
        GoldText.SetActive(true);
        Gold = gold;
        GoldText.GetComponentInChildren<TMP_Text>().text = Gold.ToString();
        m_Button.onClick.AddListener(() => { ShopButtonEvent(del); });
    }
    public void ShopRefresh()
    {
        if (Gold > MainSceneController.Instance.PlayerData.CurrentMoney)
        {
            GoldText.GetComponentInChildren<TMP_Text>().color = new Color(224 / 255, 49 / 255, 55 / 255);
        }
        else
        {
            GoldText.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
    }
    public void ShopButtonEvent(ShopWindowDelegate del)
    {
        if (Gold < MainSceneController.Instance.PlayerData.CurrentMoney)
        {
            if (RemoveCardWindow == null)
            {
                RemoveCardWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
            }
            RemoveCardWindow.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Remove, false);
            RemoveCardWindow.GetComponent<CardWindow>().Cancel.onClick.AddListener(() => { RemoveCardWindow.SetActive(false); });
            RemoveCardWindow.GetComponent<CardWindow>().SetRemoveCardButtonEvent(MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>().UseRemove);
            del(this);
        }
    }
    public void HandEnter()
    {
        MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetTarget(this.gameObject);
    }
    public void HandExit()
    {
        MainSceneController.Instance.UIControl.GetCurUI().GetComponent<ShopWindowScript>().SetTarget(null);
    }
    public void Use()
    {
        IsEnable = false;
        m_Button.image.sprite = UsedSprite;
        MainSceneController.Instance.PlayerData.CurrentMoney -= MainSceneController.Instance.PlayerData.RemoveCardGold();
        MainSceneController.Instance.PlayerData.CardRemoveCount++;
        GoldText.SetActive(false);
        Destroy(RemoveCardWindow);
    }
}
