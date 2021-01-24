using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharDescriptionPanel : MonoBehaviour
{
    public TMP_Text m_CharName;
    public TMP_Text m_HpText;
    public TMP_Text m_GoldText;
    public TMP_Text m_CharDescription;

    public Image m_RelicImage;
    public TMP_Text m_RelicName;
    public TMP_Text m_RelicDescription;

    bool IsHIde;

    // Start is called before the first frame update
    void Start()
    {
        IsHIde = true;
        transform.localPosition = transform.localPosition + new Vector3(-600.0f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PanelRefresh(CharacterType type)
    {
        if (IsHIde)
        {
            SetPanel(type);
            StartCoroutine(MoveToTarget(this.gameObject, new Vector3(600.0f, 0, 0), null));
            IsHIde = false;
        }
        else
        {
            StartCoroutine(MoveToTarget(this.gameObject,new Vector3(-600.0f, 0, 0),
                                        () =>{
                                            SetPanel(type);
                                            StartCoroutine(MoveToTarget(this.gameObject, new Vector3(600.0f, 0, 0), null));
                                        }));
        }
    }
    public void SetPanel(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Ironclad:
                m_CharName.text = CharDB.Instance.GetCharacterAsset().CharacterName;
                m_HpText.text = "HP : " + CharDB.Instance.GetCharacterAsset().Hp + "/" + CharDB.Instance.GetCharacterAsset().Hp;
                m_GoldText.text = "Gold : " + CharDB.Instance.GetCharacterAsset().Gold;
                m_CharDescription.text = CharDB.Instance.GetCharacterAsset().Description;
                m_RelicImage.sprite = CharDB.Instance.GetCharacterAsset().StartRelic.RelicImage;
                m_RelicName.text = CharDB.Instance.GetCharacterAsset().StartRelic.Name;
                m_RelicDescription.text = CharDB.Instance.GetCharacterAsset().StartRelic.Description;
                break;
            case CharacterType.Silent:
                break;
            case CharacterType.Defect:
                break;
            case CharacterType.Watcher:
                break;
        }
    }

    IEnumerator MoveToTarget(GameObject obj, Vector3 distance, Delvoid del)
    {
        Vector3 Target = obj.transform.localPosition + distance;
        while (Vector3.Distance(obj.transform.localPosition, Target) >= 1.0f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, Target, 80.0f);
            yield return null;
        }
        obj.transform.localPosition = Target;
        if (del != null)
        {
            del();
        }
    }
}
