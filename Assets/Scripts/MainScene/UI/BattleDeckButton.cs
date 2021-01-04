using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class BattleDeckButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image ButtonImage;
    public Image CountImage;
    public TMP_Text CountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisEnableButton(bool IsEnable)
    {
        ButtonImage.enabled = IsEnable;
        CountImage.enabled = IsEnable;
        CountText.enabled = IsEnable;
    }

    public void ButtonSetting(Delvoid Del)
    {
        GetComponentInChildren<Button>().onClick.AddListener(() => { Del(); });
    }
    public void CountUpdate(int DeckCount)
    {
        CountText.text = DeckCount.ToString();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        ButtonImage.rectTransform.localScale = new Vector2(1.3f, 1.3f);
    }

    public void OnPointerExit(PointerEventData e)
    {
        ButtonImage.rectTransform.localScale = Vector2.one;
    }
}
