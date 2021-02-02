using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Stat : MonoBehaviour
{
    [Header("Stat")]
    protected int MaxHP;
    public int MaxHealthPoint { get => MaxHP; set => MaxHP = value; }
    protected int CurrentHP;
    public int CurrentHealthPoint { get => CurrentHP; set => CurrentHP = value; }

    protected bool IsLive = true;
    public bool Live { get => IsLive; }

    //버프, 디버프
    int _Defence = 0;
    public int Defence { get => _Defence; set => _Defence = value; }

    [SerializeField]
    List<Power> PowerList;
    public List<Power> Powers { get => PowerList;}

    Color origin;

    protected SkeletonAnimation m_Skeleton;
    [SerializeField]
    GameObject DamageTextRes;


    //타겟 이펙트
    [SerializeField]
    protected Transform TargetEffectPos;
    [SerializeField]
    GameObject TargetEffectRes;
    GameObject TargetEffect;

    //스킬 이펙트
    [SerializeField]
    protected Transform SkillEffectPos;

    //HP바
    [SerializeField]
    Transform HPBarPos;
    [SerializeField]
    protected GameObject HPBarRes;
    protected GameObject HPBar;

    //Power
    GameObject PowerUI;
    [SerializeField]
    GameObject PowerContentRes;
    List<GameObject> PowerContents;


    [SerializeField]
    protected Transform IntentPos;
    [SerializeField]
    protected Transform DamagePos;

    MeshRenderer mesh;
    protected GameObject Canvas;

    Vector3 RT;
    Vector3 LB;

    BoxCollider2D collider;
    bool IsEnableTarget;
    bool IsDefenceOn;

    protected void Awake()
    {
        IsEnableTarget = false;
        IsDefenceOn = false;
        m_Skeleton = GetComponent<SkeletonAnimation>();
        origin = m_Skeleton.Skeleton.GetColor();
        PowerList = new List<Power>();
        PowerContents = new List<GameObject>();
    }
    public void SetClear()
    {
        PowerList.Clear();
    }

    public void SetUp(int curHP, int maxHP)
    {

        Canvas = GameObject.Find("Canvas");
        HPBar = Instantiate(HPBarRes);
        TargetEffect = Instantiate(TargetEffectRes);
        mesh = GetComponentInChildren<MeshRenderer>();
        collider = GetComponentInChildren<BoxCollider2D>();

        CurrentHP = curHP;
        MaxHP = maxHP;

        UIPositionSelector(ref HPBar, HPBarPos.position);

        RT = mesh.bounds.max;
        LB = mesh.bounds.min;
        TargetEffect.transform.SetParent(Canvas.transform);
        TargetEffect.transform.localScale = Vector3.one;
        TargetEffect.transform.localPosition = Vector3.zero;
        TargetEffect.GetComponent<TargetUIScript>().SetTargetSize(UIcoordinatePos(new Vector3(LB.x, RT.y, 0)), UIcoordinatePos(new Vector3(RT.x, RT.y, 0)), UIcoordinatePos(new Vector3(LB.x, LB.y, 0)), UIcoordinatePos(new Vector3(RT.x, LB.y, 0)));


        HPBar.GetComponent<HPBarScript>().GetDamage(CurrentHP, MaxHP, Defence);
        TargetEffect.SetActive(false);

    }
    public void HIdeUI()
    {
        TargetEffect.SetActive(false);
        HPBar.SetActive(false);
    }
    protected void UIPositionSelector(ref GameObject obj, Vector3 pos)
    {
        obj.transform.SetParent(Canvas.transform);
        obj.transform.SetAsFirstSibling();
        obj.transform.localScale = Vector3.one;

        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        screenPos =  new Vector3((Canvas.GetComponent<RectTransform>().rect.size.x*screenPos.x)- Canvas.GetComponent<RectTransform>().rect.size.x / 2 - 150,
            (Canvas.GetComponent<RectTransform>().rect.size.y * screenPos.y)- Canvas.GetComponent<RectTransform>().rect.size.y / 2, 0);
        obj.transform.localPosition = screenPos;
    }
    protected Vector3 UIcoordinatePos(Vector3 pos)
    {

        //1920:743 = ?:tmp.x
        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        screenPos = new Vector3((Canvas.GetComponent<RectTransform>().rect.size.x * screenPos.x) - Canvas.GetComponent<RectTransform>().rect.size.x / 2,
            (Canvas.GetComponent<RectTransform>().rect.size.y * screenPos.y) - Canvas.GetComponent<RectTransform>().rect.size.y / 2, 0);


        return screenPos;
    }
    public void TargetOn()
    {
        TargetEffect.SetActive(true);
        TargetEffect.GetComponent<TargetUIScript>().TargetOn();
    }
    public void TargetOff()
    {
        TargetEffect.GetComponent<TargetUIScript>().StopAllCoroutines();
        TargetEffect.SetActive(false);
    }
    public void ResetDefence()
    {
        if (IsDefenceOn)
        {
            Defence = 0;
            HPBar.GetComponentInChildren<HPBarScript>().DefenceBreak();
            IsDefenceOn = false;
        }
    }

    public virtual void SetDefence(int defence)
    {
        Defence += defence;
        HPBar.GetComponentInChildren<HPBarScript>().SetDefence(Defence);
        IsDefenceOn = true;
    }

    public virtual void GetDamage(int Damage)
    {
        int DamageAmount = 0;

        //방어도가 있을때
        if (Defence > 0)
        {
            //데미지가 방어도 이상이면
            if (Defence <= Damage)
            {
                DamageAmount = Damage - Defence;
                //방어도에 데미지
                MakeDefenceDamageText(Defence);
                if (DamageAmount > 0)
                {
                    MakeDamageText(DamageAmount);
                    CurrentHP -= DamageAmount;
                    HPBar.GetComponentInChildren<HPBarScript>().GetDamage(CurrentHP, MaxHP,Defence);
                }
                HPBar.GetComponentInChildren<HPBarScript>().DefenceBreak();
                IsDefenceOn = false;
                Defence = 0;
            }
            else if(Defence>Damage)
            {
                Defence -= Damage;
                MakeDefenceDamageText(Damage);
                HPBar.GetComponentInChildren<HPBarScript>().GetDamage(CurrentHP, MaxHP, Defence);
            }
        }
        //방어도가 없을때
        else if(Defence<=0)
        {
            IsDefenceOn = false;
            CurrentHP -= Damage;
            MakeDamageText(Damage);
            if (HPBar != null)
            {
                HPBar.GetComponentInChildren<HPBarScript>().GetDamage(CurrentHP, MaxHP, Defence);
            }
        }

        foreach(var power in Powers)
        {
            power.GetDamage();
        }

        StartCoroutine(ShakeEffect(0.4f));
        StartCoroutine(DamageEffect());
    }
    public virtual void Cure(int value)
    {
        if (CurrentHP + value < MaxHP)
        {
            MakeDamageText(value, Color.green);
            CurrentHP += value;
        }
        else
        {
            int tmp = MaxHP - CurrentHP;
            MakeDamageText(tmp, Color.green);
            CurrentHP = MaxHP;
        }
        
    }
    public void MakeSkillEffect(GameObject EffectRes,Sprite sprite)
    {
        GameObject obj = Instantiate(EffectRes);
        obj.GetComponent<SkillEffect>().Setting(SkillEffectPos, sprite);
    }
    public void MakeSkillEffect(GameObject EffectRes)
    {
        GameObject obj = Instantiate(EffectRes);
        obj.GetComponent<SkillEffect>().Setting(SkillEffectPos);
    }
    public void SetIsEnableTarget(bool Is)
    {
        IsEnableTarget = Is;
    }
    protected void MakeDamageText(int dmg)
    {
        GameObject obj = Instantiate(DamageTextRes);
        obj.transform.SetParent(Canvas.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = UIcoordinatePos(SkillEffectPos.position);
        int rand = Random.Range(-50, 50);
        obj.GetComponent<DamageTextUI>().SetDamage(dmg, rand, UIcoordinatePos(DamagePos.localPosition));
    }
    protected void MakeDamageText(int dmg,Color color)
    {
        GameObject obj = Instantiate(DamageTextRes);
        obj.transform.SetParent(Canvas.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = UIcoordinatePos(SkillEffectPos.position);
        int rand = Random.Range(-50, 50);
        obj.GetComponent<DamageTextUI>().SetDamage(dmg, rand, UIcoordinatePos(DamagePos.localPosition),color);
    }
    protected void MakeDefenceDamageText(int dmg)
    {
        GameObject obj = Instantiate(DamageTextRes);
        obj.transform.SetParent(Canvas.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = UIcoordinatePos(SkillEffectPos.position);
        obj.GetComponent<DamageTextUI>().AttackDefence(dmg, UIcoordinatePos(DamagePos.localPosition));
    }
    protected bool IsDead()
    {
        if (CurrentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void Death()
    {
        IsLive = false;
        Powers.Clear();
        Destroy(HPBar);
        Destroy(TargetEffect);
        foreach (var power in PowerContents)
        {
            Destroy(power);
        }
        PowerContents.Clear();
        Defence = 0;
        IsEnableTarget = false;
    }
    IEnumerator DamageEffect()
    {
        float Timer = 0;

        while (Timer < 0.2f)
        {
            Timer += Time.deltaTime;
            m_Skeleton.Skeleton.SetColor(Color.red);
            yield return null;
        }
        while(m_Skeleton.Skeleton.r<0.9f|| m_Skeleton.Skeleton.g<0.9f|| m_Skeleton.Skeleton.b < 0.9f)
        {
            m_Skeleton.Skeleton.SetColor(Color.Lerp(m_Skeleton.Skeleton.GetColor(),Color.white,0.5f));
            yield return null;
        }
        m_Skeleton.Skeleton.SetColor(origin);
    }
    IEnumerator ShakeEffect(float time)
    {
        int dir = 1;
        float Timer = 0;
        while (Timer< time)
        {
            Timer += Time.deltaTime;
            if (transform.localPosition.x > 0.15f|| transform.localPosition.x < -0.15f)
            {
                dir *= -1;
            }
            transform.localPosition += new Vector3(dir * Time.deltaTime * 2f, 0, 0);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
    public void BattleEnd()
    {
        Powers.Clear();
        Destroy(HPBar);
        Destroy(TargetEffect);
        foreach(var power in PowerContents)
        {
            Destroy(power);
        }
        PowerContents.Clear();
        Defence = 0;
        IsEnableTarget = false;
    }
    public void PowerRefresh()
    {
        Powers.RemoveAll(item => item.Value == 0);
        Powers.RemoveAll(item => item == null);
        PowerContents.RemoveAll(item => item == null);
        foreach (var power in PowerContents)
        {
            power.GetComponent<PowerImage>().Refresh();
        }
    }
    public void AddPowerUI(Power power,Sprite sprite)
    {
        if (PowerUI == null)
        {
            PowerUI = new GameObject("PowerUI");
            UIPositionSelector(ref PowerUI, HPBarPos.position);
            PowerUI.transform.localPosition += new Vector3(60, -80, 0);
            PowerUI.transform.SetAsFirstSibling();
            PowerUI.AddComponent<RectTransform>();
            PowerUI.AddComponent<HorizontalLayoutGroup>();
            PowerUI.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
            PowerUI.GetComponent<HorizontalLayoutGroup>().childControlHeight = false;
            PowerUI.GetComponent<HorizontalLayoutGroup>().childForceExpandHeight = false;
            PowerUI.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;

            PowerContents.Clear();
        }

        GameObject tmp = Instantiate(PowerContentRes);
        tmp.transform.SetParent(PowerUI.transform);
        tmp.transform.localScale = Vector3.one;
        tmp.GetComponent<PowerImage>().SetImage(power, sprite);
        tmp.GetComponent<PowerImage>().Refresh();


        PowerContents.Add(tmp);
    }
    protected void OnDestroy()
    {
        Powers.Clear();
        Destroy(HPBar);
        Destroy(TargetEffect);
        foreach (var power in PowerContents)
        {
            Destroy(power);
        }
        PowerContents.Clear();
        Defence = 0;
        IsEnableTarget = false;
    }
}
