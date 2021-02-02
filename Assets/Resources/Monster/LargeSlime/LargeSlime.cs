using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;



public class LargeSlime : Stat,IMonsterPatten,ISplit
{
    [SerializeField]
    GameObject Monster;

    bool IsAttackEnd;

    [SerializeField]
    bool IsMonsterPatternRandom;

    [SerializeField]
    GameObject IntentRes;

    Animator anim;

    List<MonsterAction> Pattern;
    MonsterAction[] Deck;
    GameObject Intent;

    MonsterRenderer m_MonsterRenderer;

    int CurDeckCount;
    int SelectPattern;
    int Count;

    bool IsIntent;
    bool IsSplit;
    private new void Awake()
    {
        base.Awake();
        GetComponentInParent<AttackEvent>().SetAnimationEnd(() => { IsAttackEnd = true; });
        anim = GetComponentInParent<Animator>();
        m_MonsterRenderer = GetComponentInParent<MonsterRenderer>();
        IsIntent = false;
        IsSplit = false;
        CurDeckCount = 0;

    }
    public new void SetUp(int curHP,int maxHP)
    {
        base.SetUp(curHP,maxHP);
    }
    // Start is called before the first frame update
    void Start()
    {

        BattleInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsIntent)
        {
            foreach (var item in Deck[CurDeckCount].Function)
            {
                if (item.Type == AbilityType.Attack)
                {
                    int tmp = AttackManager.Instance.UseAttack(Monster, MainSceneController.Instance.Character, null, item.AbilityKey, item.Value, false);
                    Intent.GetComponent<IntentControl>().SetIntent(tmp, Deck[CurDeckCount].Repeat, Deck[CurDeckCount].Intent);
                }
            }

        }
    }

    void PreBattleOperation()
    {

    }
    void AfterBattleOperation()
    {
        
    }
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        m_MonsterRenderer.SetAnimation(m_MonsterRenderer.AnimClips[1], false, 1.0f);
        m_MonsterRenderer.AddAnimation(m_MonsterRenderer.AnimClips[0], true, 1.0f);
        if (IsDead())
        {
            StartCoroutine(DeathEffect());
        }
    }
    new bool IsDead()
    {
        return base.IsDead();
    }
    IEnumerator DeathEffect()
    {
        float Timer = 0;
        while (m_Skeleton.Skeleton.a > 0.3f)
        {
            Timer += Time.deltaTime;
            m_Skeleton.Skeleton.a = 1.0f - Timer;
            yield return null;
        }
        MainSceneController.Instance.BattleData.CheckMonstersLive(Monster);
        Death();
    }
    new void Death()
    {
        base.Death();
        StopAllCoroutines();
        Destroy(Monster);
    }
    public void SetPattern(MonsterAsset data)
    {
        Pattern = new List<MonsterAction>();

        for (int i = 0; i < data.Pattern.Count; ++i)
        {
            Pattern.Add(data.Pattern[i]);
        }
        SetDeck();
    }
    void SetDeck()
    {
        CurDeckCount = 0;
        //랜덤으로 들어오는경우와 순서대로 들어오는 경우 존재 여기서 처리
        if (IsMonsterPatternRandom)
        {
            int rand;

            Deck = new MonsterAction[3];
            for (int i = 0; i < Deck.Length; ++i)
            {
                rand = Random.Range(0, Pattern.Count);
                Deck[i] = Pattern[rand];
                if (Deck[i].OnlyOnceAction)
                {
                    Pattern.Remove(Deck[i]);
                }
            }
            //만약 패턴이 단일 패턴이 아니라면
            if (Pattern.Count > 1)
            {
                //3패턴이 모두 같을 경우 마지막 패턴은 무조건 다른걸 뽑는다.
                while (Deck[0] == Deck[1] && Deck[1] == Deck[2])
                {
                    rand = Random.Range(0, Pattern.Count);
                    Deck[2] = Pattern[rand];
                }
            }

        }
        else
        {
            Count = Pattern.Count;
            Deck = new MonsterAction[Count];
            for (int i = 0; i < Count; ++i)
            {
                Deck[i] = Pattern[i];
                if (Deck[i].OnlyOnceAction)
                {
                    Pattern.Remove(Deck[i]);
                    Count = Pattern.Count;
                }
            }
        }
    }
    public void SetIntent()
    {
        if (!IsSplit)
        {
            if (IsMonsterPatternRandom)
            {
                if (CurDeckCount >= 3)
                {
                    SetDeck();
                }
            }
            else
            {
                if (CurDeckCount >= Count)
                {
                    SetDeck();
                }
            }
            if (Intent != null)
            {
                Destroy(Intent);
                Intent = null;
            }

            //set할떄 onceAction 확인 set되면 그다음에는 Pattern에서 제거
            IsAttackEnd = false;

            Intent = Instantiate(IntentRes);
            Intent.transform.SetParent(Canvas.transform);
            Intent.transform.localScale = Vector3.one;
            Intent.transform.localPosition = UIcoordinatePos(IntentPos.position);
            Intent.GetComponent<IntentControl>().SetIntent(Deck[CurDeckCount].Function[0].Value, Deck[CurDeckCount].Repeat, Deck[CurDeckCount].Intent);
            IsIntent = true;

        }

    }
    public void BattleInit()
    {
        PowerManager.Instance.AssginBuff(Monster, PowerVariety.Split, MaxHealthPoint/2, true);
    }
    public IEnumerator Action()
    {
        if (IsSplit == false) { 
        IsIntent = false;
        List<GameObject> Player = new List<GameObject>();
        Player.Add(MainSceneController.Instance.Character);
        Attack();
            for (int i = 0; i < Deck[CurDeckCount].Repeat; ++i)
            {
                foreach (var func in Deck[CurDeckCount].Function)
                {
                    MonsterTargetType target = Deck[CurDeckCount].Target;
                    switch (func.Type)
                    {
                        case AbilityType.Attack:
                            AttackManager.Instance.UseAttack(Monster, MainSceneController.Instance.Character, func.SkillEffect, func.AbilityKey, func.Value, true);
                            break;
                        case AbilityType.Skill:
                            SkillManager.Instance.UseSkill(Monster, MainSceneController.Instance.Character, func.AbilityKey, func.Value, true);
                            break;
                        case AbilityType.Power:
                            if (target == MonsterTargetType.Self)
                            {
                                PowerManager.Instance.AssginBuff(Monster, func.variety, func.Value, true);
                            }
                            else if (target == MonsterTargetType.Player)
                            {
                                PowerManager.Instance.AssginBuff(MainSceneController.Instance.Character, func.variety, func.Value, true);
                            }
                            break;
                    }
                }

                yield return new WaitForSeconds(0.3f);
            }
            CurDeckCount++;
            Intent.GetComponent<IntentControl>().OnAction();
            IsAttackEnd = true;
        }
        else
        {
            Color color = m_Skeleton.Skeleton.GetColor();
            float alpha = 1.0f;
            Intent.GetComponent<IntentControl>().OnAction();
            while (m_Skeleton.Skeleton.GetColor().a > 0.3f)
            {
                Color tmp = new Color(color.r, color.g, color.b, alpha);
                m_Skeleton.Skeleton.SetColor(tmp);
                alpha -= Time.deltaTime;
                yield return null;
            }
            IsAttackEnd = true;

            SummonMiddle();

            Destroy(Monster);
        }
    }
    public bool GetAttackEnd()
    {
        return IsAttackEnd;
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void Split()
    {
        IsSplit = true;
        if (Intent != null)
        {
            Destroy(Intent);
        }
        Intent = Instantiate(IntentRes);
        Intent.transform.SetParent(Canvas.transform);
        Intent.transform.localScale = Vector3.one;
        Intent.transform.localPosition = UIcoordinatePos(IntentPos.position);
        Intent.GetComponent<IntentControl>().SetIntent(0, 0, IntentType.UnKnown);
        IsIntent = true;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        Destroy(Intent);
        
    }

    public void SummonMiddle()
    {
        Vector3 tmp = Monster.transform.localPosition;
        MainSceneController.Instance.Spawner.SummonMonster("MiddleSlime", CurrentHealthPoint, tmp - new Vector3(3, 0, 0));
        MainSceneController.Instance.Spawner.SummonMonster("MiddleSlime", CurrentHealthPoint, tmp + new Vector3(3, 0, 0));
    }
}
