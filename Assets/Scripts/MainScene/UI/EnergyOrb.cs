using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyOrb : MonoBehaviour
{
    public Sprite[] IronClad;
    public Sprite[] IronClad_D;
    public Sprite[] Silent;
    public Sprite[] Silent_D;
    public Sprite[] VFX;

    public Image[] OrbImage;
    public TMP_Text CostText;

    CharacterType m_Type = CharacterType.None;
    bool IsCostEmpty;
    bool IsInit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInit)
        {
            StateProcess();
        }
    }

    void StateProcess()
    {
        switch (m_Type)
        {
            case CharacterType.Ironclad:
                ImageRotate(OrbImage[1], 5.0f);
                ImageRotate(OrbImage[2], 10.0f);
                ImageRotate(OrbImage[3], 20.0f);
                ImageRotate(OrbImage[4], 30.0f);
                break;
            case CharacterType.Silent:
                ImageRotate(OrbImage[0], 5.0f);
                ImageRotate(OrbImage[1], 10.0f);
                ImageRotate(OrbImage[3], 20.0f);
                break;
        }
    }

    void ImageRotate(Image obj, float speed)
    {
        obj.transform.localEulerAngles += new Vector3(0, 0, speed * Time.deltaTime);
    }

    void ChangeImage()
    {
        if (!IsCostEmpty)
        {
            switch (m_Type)
            {
                case CharacterType.Ironclad:
                    for (int i = 0; i < IronClad.Length; ++i)
                    {
                        OrbImage[i].sprite = IronClad[i];
                    }
                    OrbImage[6].sprite = VFX[0];
                    OrbImage[7].sprite = VFX[0];
                    break;
                case CharacterType.Silent:
                    for (int i = 0; i < IronClad.Length; ++i)
                    {
                        OrbImage[i].sprite = Silent[i];
                    }
                    OrbImage[6].sprite = VFX[1];
                    OrbImage[7].sprite = VFX[1];
                    break;
            }
        }
        else
        {
            switch (m_Type)
            {
                case CharacterType.Ironclad:
                    for (int i = 0; i < IronClad.Length; ++i)
                    {
                        OrbImage[i].sprite = IronClad_D[i];
                    }
                    OrbImage[6].sprite = VFX[0];
                    OrbImage[7].sprite = VFX[0];
                    break;
                case CharacterType.Silent:
                    for (int i = 0; i < IronClad.Length; ++i)
                    {
                        OrbImage[i].sprite = Silent_D[i];
                    }
                    OrbImage[6].sprite = VFX[1];
                    OrbImage[7].sprite = VFX[1];
                    break;
            }
        }
    }
    public void OrbImageInit(CharacterType type)
    {
        IsCostEmpty = true;
        IsInit = true;
        m_Type = type;
        switch (m_Type)
        {
            case CharacterType.Ironclad:
                for (int i = 0; i < IronClad.Length; ++i)
                {
                    OrbImage[i].sprite = IronClad[i];
                }
                break;
            case CharacterType.Silent:
                for (int i = 0; i < IronClad.Length; ++i)
                {
                    OrbImage[i].sprite = Silent[i];
                }
                break;
        }
        ChangeImage();
    }
    public void EnergyCharge(int CurEnergy, int MaxEnergy)
    {
        if (IsCostEmpty&&CurEnergy==MaxEnergy)
        {
            StartCoroutine(EnergyVFX());
        }
        IsCostEmpty = false;
        OrbImage[6].color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
        EnergyChargeUpdate(CurEnergy, MaxEnergy);
    }

    public void EnergyChargeUpdate(int CurEnergy, int MaxEnergy)
    {
        if (CurEnergy == 0) {
            IsCostEmpty = true;
        }

        ChangeImage();

        CostText.text = CurEnergy.ToString() + "/" + MaxEnergy.ToString();
    }

    IEnumerator EnergyVFX()
    {
        OrbImage[6].rectTransform.sizeDelta = new Vector2(350, 350);
        OrbImage[7].rectTransform.sizeDelta = new Vector2(350, 350);
        OrbImage[6].rectTransform.eulerAngles = Vector3.zero;
        OrbImage[7].rectTransform.eulerAngles = new Vector3(0,180,0);
        float speed = 0.3f;
        while (OrbImage[6].rectTransform.sizeDelta.x > 1.0f&& OrbImage[7].rectTransform.sizeDelta.x > 1.0f)
        {
            speed += Time.deltaTime/2;
            OrbImage[6].rectTransform.sizeDelta -= Vector2.one;
            OrbImage[6].rectTransform.localEulerAngles += new Vector3(0, 0, speed);
            OrbImage[7].rectTransform.sizeDelta -= Vector2.one;
            OrbImage[7].rectTransform.localEulerAngles += new Vector3(0, 0, speed);
            yield return null;
        }
        OrbImage[6].rectTransform.sizeDelta = Vector2.zero;
        OrbImage[6].rectTransform.eulerAngles = Vector3.zero;
        OrbImage[7].rectTransform.sizeDelta = Vector2.zero;
        OrbImage[7].rectTransform.eulerAngles = new Vector3(0, 180, 0);
    }
}
