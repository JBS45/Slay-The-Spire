using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MysteryEventWindow : MonoBehaviour,IDeckChange
{
    [SerializeField]
    Image EventImage;

    [Header("Button")]
    [SerializeField]
    GameObject ButtonRes;
    [SerializeField]
    Transform ButtonPos;
    List<GameObject> Buttons;

    [Header("String")]
    [SerializeField]
    TMP_Text Description;
    [SerializeField]
    TMP_Text Name;

    delegate void MysteryButtonEvent(MysteryButton button);

    Mystery Value;

    GameObject ExtraWindow;

    Dictionary<string, MysteryButtonEvent> Methods;

    private void Awake()
    {
        Buttons = new List<GameObject>();
        Methods = new Dictionary<string, MysteryButtonEvent>();
        SetMethod();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMethod()
    {
        Methods.Add("Enchant", CardEnchant);
        Methods.Add("Progress", Progress);
        Methods.Add("Remove", CardRemove);
        Methods.Add("AddRandom", AddRandomCard);
    }

    public void SetWindow(string Key)
    {
        if (Buttons.Count > 0)
        {
            foreach(var button in Buttons)
            {
                Destroy(button);
            }
        }

        Buttons.Clear();

        Value = MysteryDB.Instance.DB[Key];

        EventImage.sprite = Value.sprite;

        for(int i = 0; i < Value.Button.Count; ++i)
        {
            GameObject tmp = Instantiate(ButtonRes,ButtonPos);
            MysteryButtonEvent EventDel = new MysteryButtonEvent((MysteryButton button) => {});
            foreach (var ButtonEvent in Value.Button[i].ButtonEventKey)
            {
                EventDel += Methods[ButtonEvent];
            }
            tmp.GetComponentInChildren<TMP_Text>().text = Value.Button[i].ButtonString;
            tmp.transform.SetAsFirstSibling();
            int num = i;
            tmp.GetComponentInChildren<Button>().onClick.AddListener(
                () => 
                {
                    EventDel(Value.Button[num]);
                });
            Buttons.Add(tmp);
        }

        Description.text = Value.Description;
        Name.text = Value.Name;

    }

    public void NextWindow(MysteryButton button)
    {
        SetWindow(button.NextKey);
    }
    public void CardEnchant(MysteryButton button)
    {
        ExtraWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
        ExtraWindow.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Enchant, false);
        ExtraWindow.GetComponent<CardWindow>().CancelButton.SetActive(false);
        ExtraWindow.GetComponent<CardWindow>().SetEnchantCardButtonEvent(()=> { NextWindow(button); });
    }
    public void CardRemove(MysteryButton button)
    {
        ExtraWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
        ExtraWindow.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Remove, false);
        ExtraWindow.GetComponent<CardWindow>().CancelButton.SetActive(false);
        ExtraWindow.GetComponent<CardWindow>().SetRemoveCardButtonEvent(() => { NextWindow(button); });
    }
    public void AddRandomCard(MysteryButton button)
    {
        int random = Random.Range(0, CardDB.Instance.IronClad.Card.Count);
        CardData tmp = new CardData(CardDB.Instance.IronClad.Card[random]);
        MainSceneController.Instance.PlayerData.AddCard(tmp);
    }
    public void Progress(MysteryButton button)
    {
        MainSceneController.Instance.UIControl.OpenMapProgress();
    }

    public CardData OriginDeckChange(CardData data)
    {
        return data;
    }
}
