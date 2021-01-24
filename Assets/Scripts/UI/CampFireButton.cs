using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CampFireButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    Image Outline;

    [SerializeField]
    Button Button;

    [SerializeField]
    TMP_Text Text;
    [SerializeField]
    string Description;

    private void Awake()
    {
        Outline.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetButtonEvent(Delvoid del)
    {
        Button.onClick.AddListener(()=> { del(); });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        Outline.enabled = true;
        Text.text = string.Format(Description, MainSceneController.Instance.PlayerData.MaxHp * 0.3f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        Outline.enabled = false;
        Text.text = "";
    }
}
