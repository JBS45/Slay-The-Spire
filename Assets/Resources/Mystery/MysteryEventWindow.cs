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

    [SerializeField]
    Sprite[] ButtonImage;

    [Header("String")]
    [SerializeField]
    TMP_Text Description;
    [SerializeField]
    TMP_Text Name;


    [Header("Color")]
    [SerializeField]
    Color[] TextColor;

    Mystery Value;

    GameObject ExtraWindow;

    delegate void ButtonEventDel(MysteryButton button);

    private void Awake()
    {
        Buttons = new List<GameObject>();
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
    {/*

        Methods.Add("Remove", CardRemove);
        Methods.Add("Change", CardChange);
        Methods.Add("AddRandom", AddRandomCard);
        */
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
            GameObject tmp = Instantiate(ButtonRes, ButtonPos);
            tmp.transform.SetAsFirstSibling();

            if (CheckCondtion(Value.Button[i]))
            {
                tmp.GetComponentInChildren<Image>().sprite = ButtonImage[0];
                MakeButton(tmp, Value.Button[i]);
            }
            else
            {
                tmp.GetComponentInChildren<Image>().sprite = ButtonImage[1];
                tmp.GetComponentInChildren<TMP_Text>().text = "[잠김]";
            }
            Buttons.Add(tmp);
        }

        Description.text = Value.Description;
        Name.text = Value.Name;

    }
    bool CheckCondtion(MysteryButton button)
    {
        switch (button.condtion.type)
        {
            case ConditionType.Gold:
                if (MainSceneController.Instance.PlayerData.CurrentMoney>=(int)button.condtion.value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return true;
        }
    }

    /*GameObject MakeButton(MysteryButton button, int number,bool IsEnable)
    {
        GameObject tmp = Instantiate(ButtonRes, ButtonPos);
        tmp.transform.SetAsFirstSibling();

        if (IsEnable)
        {
            MysteryButtonEvent EventDel = new MysteryButtonEvent(Methods[button.ButtonEventKey]);

            tmp.GetComponentInChildren<TMP_Text>().text = button.ButtonString;

            int num = number;
            tmp.GetComponentInChildren<Image>().sprite = ButtonImage[0];
            tmp.GetComponentInChildren<Button>().onClick.AddListener(
                () =>
                {
                    EventDel(Value.Button[num]);
            });
        }
        else
        {
            tmp.GetComponentInChildren<Image>().sprite = ButtonImage[1];
            tmp.GetComponentInChildren<TMP_Text>().text = "[잠김]";
        }
        return tmp;
    }*/
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
    public void CardChange(MysteryButton button)
    {
        ExtraWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
        ExtraWindow.GetComponent<CardWindow>().SetCardWindow(MainSceneController.Instance.PlayerData.OriginDecks, WindowType.Remove, false);
        ExtraWindow.GetComponent<CardWindow>().CancelButton.SetActive(false);
        AddRandomCard(button);
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

    void MakeButton(GameObject button, MysteryButton buttonEvent)
    {
        ButtonEventDel Del = new ButtonEventDel((MysteryButton)=> { });
        button.GetComponentInChildren<TMP_Text>().text = "";
        foreach (var func in buttonEvent.ButtonEvents) {
            switch (func.ButtonEventKey) {
                case "Enchant":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[1]), func.Value);
                    Del += CardEnchant;
                    break;
                case "RandomEnchant":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[1]), func.Value);
                    Del += (MysteryButton)
                        => {
                                List<CardData> tmp = MainSceneController.Instance.PlayerData.OriginDecks.FindAll(item => item.EnchantCount == 0 || item.MultipleEnchant == true);

                                int count = Mathf.Min(tmp.Count, func.Value);

                                for (int i = 0; i < count; ++i)
                                {
                                    int random = Random.Range(0, tmp.Count);
                                    tmp[random].CardEnchant();
                                    tmp.RemoveAt(random);
                                }
                        };
                    break;
                case "Remove":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[1]), func.Value);
                    Del += CardRemove;
                    break;
                case "Change":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[1]), func.Value);
                    Del += CardChange;
                    break;
                case "Money":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[3]), func.Value);
                    Del += (MysteryButton) => { if(MainSceneController.Instance.PlayerData.CurrentMoney>=func.Value)
                                                    MainSceneController.Instance.PlayerData.CurrentMoney -= func.Value; };
                    
                    break;
                case "Damage":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[2]), (int)(((float)func.Value / 100) * MainSceneController.Instance.PlayerData.MaxHp));
                    Del += (MysteryButton) => { MainSceneController.Instance.Character.GetComponentInChildren<Stat>().GetDamage((int)(((float)func.Value / 100) * MainSceneController.Instance.PlayerData.MaxHp)); };
                    break;
                case "Heal":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[1]), (int)(((float)func.Value/100)*(float)MainSceneController.Instance.PlayerData.MaxHp));
                    Del += (MysteryButton) => { MainSceneController.Instance.Character.GetComponentInChildren<Stat>().Cure((int)(((float)func.Value / 100) * MainSceneController.Instance.PlayerData.MaxHp)); };
                    break;
                case "Progress":
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[0]), func.Value);
                    Del += Progress;
                    break;
                default:
                    button.GetComponentInChildren<TMP_Text>().text += string.Format(func.ButtonString, ColorTohexadecimal(TextColor[0]), func.Value);
                    Del = NextWindow;
                    break;
            }
        }
        button.GetComponentInChildren<Button>().onClick.AddListener(() => { Del(buttonEvent); });
    }
    string ColorTohexadecimal(Color color)
    {
        string result = "";

        int[] RGB = { (int)(color.r * 255) / 16, (int)(color.r * 255) % 16, (int)(color.g * 255) / 16, (int)(color.g * 255) % 16, (int)(color.b * 255) / 16, (int)(color.r * 255) % 16 };

        for (int i = 0; i < RGB.Length; ++i)
        {
            if (RGB[i] < 10)
            {
                result += RGB[i].ToString();
            }
            else if (RGB[i] == 10)
            {
                result += "A";
            }
            else if (RGB[i] == 11)
            {
                result += "B";
            }
            else if (RGB[i] == 12)
            {
                result += "C";
            }
            else if (RGB[i] == 13)
            {
                result += "D";
            }
            else if (RGB[i] == 14)
            {
                result += "E";
            }
            else if (RGB[i] == 15)
            {
                result += "F";
            }
        }

        return result;
    }
}
