using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToolTipScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text Name;
    [SerializeField]
    TMP_Text Description;

    [SerializeField]
    RectTransform Mid;

    float width;
    float Height;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(RelicData data)
    {
        Name.text = data.Name;
        Description.text = data.Description;

        width = Name.rectTransform.sizeDelta.x;
        float Tmp = Description.fontSize * data.Description.Length;
        Height = (Tmp / width * Description.fontSize);

        Mid.sizeDelta = new Vector2(width, Height);
    }
}
