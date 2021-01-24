using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarScript : MonoBehaviour
{
    [Header("Bar")]
    [SerializeField]
    GameObject HpBarBody;
    [SerializeField]
    GameObject DamageBody;
    [SerializeField]
    GameObject PoisonBody;
    [SerializeField]
    TMP_Text HPText;

    [Header("Total")]
    [SerializeField]
    GameObject HpBar;
    [SerializeField]
    GameObject DamageBar;
    [SerializeField]
    GameObject PoisonBar;


    [Header("Block")]
    [SerializeField]
    GameObject BlockImage1;
    [SerializeField]
    GameObject BlockImage2;
    [SerializeField]
    TMP_Text BlockText;
    [SerializeField]
    Sprite[] BlockSprtie;

    Animator anim;

    float OriginLength;

    Color OriginHPColor;
    private void Awake()
    {
        BlockImage1.SetActive(false);
        BlockImage2.SetActive(false);
        BlockText.enabled = false;
        PoisonBar.SetActive(false);
        OriginLength = HpBarBody.GetComponent<RectTransform>().rect.width;
        OriginHPColor = HpBar.GetComponentInChildren<Image>().color;
        anim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDefence(int defence)
    {
        
        BlockImage1.SetActive(true);
        BlockImage2.SetActive(true);
        BlockImage1.transform.localPosition = Vector3.zero;
        BlockImage2.transform.localPosition = Vector3.zero;
        BlockImage1.GetComponentInChildren<Image>().sprite = BlockSprtie[0];
        BlockImage2.GetComponentInChildren<Image>().sprite = BlockSprtie[0];

        Image[] HpBars = HpBar.GetComponentsInChildren<Image>();

        for (int i=0; i< HpBars.Length; ++i)
        {
            HpBars[i].color = new Color(0.4f, 0.65f, 0.9f);
        }

        BlockText.enabled = true;
        BlockText.text = defence.ToString();

    }
    public void DefenceBreak()
    {
        anim.SetTrigger("Break");
        BlockText.enabled = false;
        BlockText.text = "";
        Image[] HpBars = HpBar.GetComponentsInChildren<Image>();

        for (int i = 0; i < HpBars.Length; ++i)
        {
            HpBars[i].color = OriginHPColor;
        }

    }
    public void GetDamage(int CurHp,int MaxHP,int Defence)
    {
        HPText.text = CurHp + "/" + MaxHP;
        BlockText.text = Defence.ToString();
        HpBarBody.GetComponent<RectTransform>().sizeDelta = new Vector2(OriginLength * CurHp / MaxHP, HpBarBody.GetComponent<RectTransform>().rect.height);
        if (CurHp <= 0)
        {
            HpBar.SetActive(false);
            DamageBar.SetActive(false);
            HPText.text = "죽음";

            return;
        }
        
        StartCoroutine(GetDamageCoroutine(CurHp, MaxHP));
    }
    IEnumerator GetDamageCoroutine(int CurHp, int MaxHP)
    {
        yield return new WaitForSeconds(0.5f);
        float target = OriginLength * CurHp / MaxHP;

        while(DamageBody.GetComponent<RectTransform>().rect.width - target >= 1.0f)
        {
            float tmpX = Mathf.Lerp(DamageBody.GetComponent<RectTransform>().rect.width, target, Time.deltaTime * 5);
            DamageBody.GetComponent<RectTransform>().sizeDelta = new Vector2(tmpX, DamageBody.GetComponent<RectTransform>().rect.height);
            yield return null;
        }
        DamageBody.GetComponent<RectTransform>().sizeDelta = new Vector2(OriginLength * CurHp / MaxHP, DamageBody.GetComponent<RectTransform>().rect.height);
    }
    public void OffImage()
    {
        BlockImage1.SetActive(false);
        BlockImage2.SetActive(false);
        BlockText.enabled = false;
    }
}
