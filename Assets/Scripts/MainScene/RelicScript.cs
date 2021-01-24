using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RelicScript : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler,IObservers
{
    [Header("AfterImage effect")]
    [SerializeField]
    GameObject AfterImage;
    Image[] AfterImages;

    [Header("Relic image")]
    [SerializeField]
    Image RelicImage;

    RelicData Data;

    [SerializeField]
    TMP_Text Text;



    private void Awake()
    {
        AfterImages = AfterImage.GetComponentsInChildren<Image>();
        foreach (var image in AfterImages)
        {
            image.enabled = false;
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AfterImageEffect()
    {
        int count = 0;
        foreach (var image in AfterImages)
        {
            image.enabled = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
            image.transform.localScale = Vector3.one; 
        }

        while (AfterImages[AfterImages.Length - 1].color.a > 0.05f)
        {
            for (int i = 0; i <= count; ++i)
            {
                AfterImages[i].color -= new Color(0, 0, 0, Time.deltaTime);
                AfterImages[i].transform.localScale += new Vector3(Time.deltaTime * 2, Time.deltaTime * 2, 0);
            }
            if (AfterImages[count].color.a < 0.7f)
            {
                if (count < AfterImages.Length - 1)
                {
                    count++;
                }
            }
            yield return null;
        }
        foreach (var image in AfterImages)
        {
            image.enabled = false;
        }
    }
    public void OnAfterImage()
    {
        StartCoroutine(AfterImageEffect());
    }
    public void SetData(RelicData data)
    {
        Data = data;
        RelicImage.sprite = data.RelicImage;
        foreach (var image in AfterImages)
        {
            image.sprite = data.RelicImage;
        }
        if (Data.IsStack)
        {
            Text.text = Data.StackCount.ToString();
        }
        else
        {
            Text.enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        MainSceneController.Instance.UIControl.ToolTip.SetActive(true);
        MainSceneController.Instance.UIControl.ToolTip.transform.localPosition = GameUtill.CoordCanvasPosition(transform.localPosition - new Vector3(0,100,0));
        MainSceneController.Instance.UIControl.ToolTip.GetComponent<ToolTipScript>().SetData(Data);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MainSceneController.Instance.UIControl.ToolTip.SetActive(false);
    }
    public void UpdateData()
    {
        OnAfterImage();
        if (Data.IsStack)
        {
            Text.text = Data.StackCount.ToString();
        }
        else
        {
            Text.enabled = false;
        }
    }
}
