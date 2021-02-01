using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class IntentItemControl : MonoBehaviour
{
    [SerializeField]
    GameObject AfterImage;
    Image[] AfterImages;

    [SerializeField]
    Image MainImage;

    

    private void Awake()
    {

        AfterImages = AfterImage.GetComponentsInChildren<Image>();

        foreach(var image in AfterImages)
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
    public void SetData(Sprite sprite)
    {
        MainImage.sprite = sprite;
        foreach (var image in AfterImages)
        {
            image.sprite = sprite;
        }
    }
    public void Setflicker(bool IsUse)
    {
        if (IsUse)
        {
            StartCoroutine(Flicker());
        }
        else
        {
            StopCoroutine(Flicker());
            MainImage.color = Color.white;
        }
    }
    public void SetRotate(bool IsUse)
    {
        if (IsUse)
        {
            StartCoroutine(Rotate());
        }
        else
        {
            StopCoroutine(Rotate());
            MainImage.transform.eulerAngles = Vector3.zero;
        }
    }
    public void SetAfterImage(bool IsUse)
    {
        if (IsUse)
        {
            StartCoroutine(AfterImageEffect());
        }
        else
        {
            StopCoroutine(AfterImageEffect());
            MainImage.transform.eulerAngles = Vector3.zero;
        }
    }
    public void OnAfterImage()
    {
        StopAllCoroutines();
        StartCoroutine(EndAfterImageEffect());
    }

    IEnumerator Flicker()
    {
        int dir = -1;
        MainImage.color = new Color(1f, 1f, 1f, 0.9f);
        while (true)
        {
            if(MainImage.color.a>0.9f|| MainImage.color.a < 0.5f)
            {
                dir *= -1;
            }
            MainImage.color += new Color(0, 0, 0, Time.deltaTime/2)*dir;
            yield return null;
        }
    }

    IEnumerator EndAfterImageEffect()
    {
        int count = 0;
        foreach (var image in AfterImages)
        {
            image.enabled = true;
        }

        MainImage.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

        while (AfterImages[AfterImages.Length - 1].color.a>0.05f)
        {
            for(int i = 0; i <= count; ++i)
            {
                AfterImages[i].color -= new Color(0, 0, 0, Time.deltaTime);
                AfterImages[i].transform.localScale += new Vector3(Time.deltaTime*2, Time.deltaTime*2, 0);
            }
            if (AfterImages[count].color.a < 0.7f)
            {
                if(count< AfterImages.Length - 1)
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

    IEnumerator Rotate()
    {
        while (true)
        {
            MainImage.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime*36);
            yield return null;
        }
    }

    IEnumerator AfterImageEffect()
    {
        int count = 0;
        foreach (var image in AfterImages)
        {
            image.enabled = true;
        }


        while (true)
        {
            for (int i = 0; i <= count; ++i)
            {
                AfterImages[i].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                AfterImages[i].transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
                if(AfterImages[i].transform.localScale.x > 2.0f)
                {
                    AfterImages[i].transform.localScale = Vector3.one;
                }
            }
            if (AfterImages[count].transform.localScale.x > 1.3f)
            {
                if (count < AfterImages.Length - 1)
                {
                    count++;
                }
            }
            yield return null;
        }
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
