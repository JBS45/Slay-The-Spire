using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TitleButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Image HighligtImage;
    public TMP_Text ButtonText;
    Vector3 target;
    bool bTextMoving = false;
    SEManager m_SEManager;

    private void Awake()
    {
        HighligtImage.enabled = false;
        target = ButtonText.transform.localPosition;
        m_SEManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        GetComponentInChildren<Button>().onClick.AddListener(PlayClickSound);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        HighligtImage.enabled = true;
        target += new Vector3(50.0f, 0.0f, 0.0f);
        if (!bTextMoving)
        {
            StartCoroutine(PointerEventTextMove());
        }
        m_SEManager.PlaySE(0);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        HighligtImage.enabled = false;
        target -= new Vector3(50.0f, 0.0f, 0.0f);
        if (!bTextMoving)
        {
            StartCoroutine(PointerEventTextMove());
        }
    }

    IEnumerator PointerEventTextMove()
    {
        bTextMoving = true;
        while (Vector3.Distance(ButtonText.transform.localPosition, target) >= 1.0f)
        {
            ButtonText.transform.localPosition = Vector3.MoveTowards(ButtonText.transform.localPosition, target, 10.0f);
            yield return null;
        }
        ButtonText.transform.localPosition = target;
        bTextMoving = false;
    }
    void PlayClickSound()
    {
        m_SEManager.PlaySE(1);
    }
}
