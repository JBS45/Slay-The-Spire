using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using TMPro;

public enum IntentType
{
    Attack,Defend,Buff,Debuff,Debuff2,AttackAndDefend,AttackAndBuff, AttackAndDeBuff,DefendAndBuff, Sleep ,Stun, UnKnown
}
public class IntentControl : MonoBehaviour
{
    [SerializeField]
    GameObject IntentImageRes;
    List<GameObject> Intent;

    [SerializeField]
    GameObject BuffEffect;

    [SerializeField]
    GameObject DeBuffEffect;

    [SerializeField]
    TMP_Text DamageText;

    [Header("Sprite")]
    [SerializeField]
    Sprite[] Attacks;
    [SerializeField]
    Sprite[] IntentSprite;

    [Header("Sound")]
    [SerializeField]
    AudioClip[] SE;
    int currentSENum;
    IntentType Type;

    bool IsInit = false;
    private void Awake()
    {
        BuffEffect.GetComponent<ParticleSystem>().Stop();
        DeBuffEffect.GetComponent<ParticleSystem>().Stop();
        DamageText.enabled = false;
        IsInit = false;
        Intent = new List<GameObject>();
        currentSENum = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIntent(int Damage, int Repeat, IntentType type)
    {
        Type = type;
        Sprite tmp = SpriteSelector(Damage);
        string text;
        if (Repeat >= 2) {
            text = Damage.ToString() + "x" + Repeat.ToString();
        }
        else
        {
            text = Damage.ToString();
        }
        if (IsInit == false)
        {
            Intent.Add(Instantiate(IntentImageRes));
            Intent[0].transform.SetParent(transform);
            Intent[0].transform.localScale = Vector3.one;
            Intent[0].transform.localPosition = Vector3.zero;
            switch (Type)
            {
                case IntentType.Attack:
                    DamageText.enabled = true;
                    Intent[0].GetComponent<IntentItemControl>().SetData(tmp);
                    DamageText.text = text;
                    break;
                case IntentType.AttackAndBuff:
                    DamageText.enabled = true;
                    Intent[0].GetComponent<IntentItemControl>().SetData(tmp);
                    BuffEffect.GetComponent<ParticleSystem>().Play();
                    DamageText.text = text;
                    break;
                case IntentType.AttackAndDeBuff:
                    DamageText.enabled = true;
                    Intent[0].GetComponent<IntentItemControl>().SetData(tmp);
                    DeBuffEffect.GetComponent<ParticleSystem>().Play();
                    DamageText.text = text;
                    break;
                case IntentType.AttackAndDefend:
                    DamageText.enabled = true;
                    Intent[0].GetComponent<IntentItemControl>().SetData(tmp);
                    Intent.Add(Instantiate(IntentImageRes));
                    Intent[1].transform.SetParent(transform);
                    Intent[1].transform.localScale = Vector3.one;
                    Intent[1].transform.localPosition = Vector3.zero;
                    Intent[1].GetComponent<IntentItemControl>().SetData(IntentSprite[4]);
                    Intent[1].GetComponent<IntentItemControl>().SetAfterImage(true);
                    Intent[0].transform.SetAsLastSibling();
                    DamageText.text = text;
                    break;
                case IntentType.Buff:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[0]);
                    BuffEffect.GetComponent<ParticleSystem>().Play();
                    break;
                case IntentType.Debuff:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[1]);
                    Intent[0].GetComponent<IntentItemControl>().SetRotate(true);
                    DeBuffEffect.GetComponent<ParticleSystem>().Play();
                    break;
                case IntentType.Debuff2:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[2]);
                    Intent[0].GetComponent<IntentItemControl>().SetRotate(true);
                    break;
                case IntentType.Defend:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[4]);
                    break;
                case IntentType.DefendAndBuff:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[3]);
                    BuffEffect.GetComponent<ParticleSystem>().Play();
                    break;
                case IntentType.Sleep:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[5]);
                    Intent[0].GetComponent<IntentItemControl>().Setflicker(true);
                    break;
                case IntentType.Stun:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[6]);
                    Intent[0].GetComponent<IntentItemControl>().Setflicker(true);
                    break;
                case IntentType.UnKnown:
                    DamageText.enabled = false;
                    Intent[0].GetComponent<IntentItemControl>().SetData(IntentSprite[7]);
                    Intent[0].GetComponent<IntentItemControl>().Setflicker(true);
                    break;
            }
        }
        else
        {
            Intent[0].GetComponent<IntentItemControl>().SetData(tmp);
            DamageText.text = text;
        }
        IsInit = true;

    }
    Sprite SpriteSelector(int Damage)
    {
        Sprite tmp;

        if (Damage <= 5)
        {
            tmp = Attacks[0];
            currentSENum = 0;
        }
        else if (Damage > 5 && Damage <= 10)
        {
            tmp = Attacks[1];
            currentSENum = 1;
        }
        else if (Damage > 10 && Damage <= 15)
        {
            tmp = Attacks[2];
            currentSENum = 2;
        }
        else if (Damage > 15 && Damage <= 20)
        {
            tmp = Attacks[3];
            currentSENum = 3;
        }
        else if (Damage > 20 && Damage <= 25)
        {
            tmp = Attacks[4];
            currentSENum = 4;
        }
        else if (Damage > 25 && Damage <= 30)
        {
            tmp = Attacks[5];
            currentSENum = 5;
        }
        else
        {
            tmp = Attacks[6];
            currentSENum = 6;
        }

        return tmp;
    }

    public void OnAction()
    {
        MainSceneController.Instance.AtkSEManager.PlaySE(SE[currentSENum]);
        for(int i = 0; i < Intent.Count; ++i)
        {
            Intent[i].GetComponent<IntentItemControl>().OnAfterImage(()=> { Destroy(this.gameObject); });
        }
        DeBuffEffect.GetComponent<ParticleSystem>().Stop();
        BuffEffect.GetComponent<ParticleSystem>().Stop();
       
    }

    
}
