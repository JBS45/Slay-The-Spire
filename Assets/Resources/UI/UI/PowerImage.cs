using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerImage : MonoBehaviour
{
    [SerializeField]
    Image Image;
    [SerializeField]
    TMP_Text Text;

    [SerializeField]
    Sprite[] sprites;

    [Header("TextColorTable")]
    [SerializeField]
    Color[] TextColor;

    Power Data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Power power,Sprite sprite)
    {
        Data = power;
        Image.sprite = sprite;
    }
    public void Refresh()
    {
        if (Data.Value != 0)
        {
            if (Data.Value > 0)
            {
                Text.color = TextColor[0];
            }
            else
            {
                Text.color = TextColor[1];
            }
            Text.text = Data.Value.ToString();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
