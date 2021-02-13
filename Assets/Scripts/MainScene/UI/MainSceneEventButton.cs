using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainSceneEventButton : MonoBehaviour,IPointerEnterHandler
{
    public TMP_Text Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonSetting(string text,Delvoid Del)
    {
        Text.text = text;
        if (Del != null)
        {
            MainSceneController.Instance.SEManager.PlaySE(1);
            GetComponent<Button>().onClick.AddListener(() => { Del(); });
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponentInChildren<Button>().enabled)
        {
            MainSceneController.Instance.SEManager.PlaySE(0);
        }
    }
    public void ChangeText(Turn state)
    {
        switch (state)
        {
            case Turn.Player:
                Text.text = "턴 종료";
                break;
            case Turn.Enemy:
                Text.text = "적 턴";
                break;
        }
    }
}
