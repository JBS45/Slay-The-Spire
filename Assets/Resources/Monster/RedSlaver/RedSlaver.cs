using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RedSlaver : Stat,IMonsterPatten
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

    private new void Awake()
    {
        base.Awake();
        GetComponentInParent<AttackEvent>().SetAnimationEnd(() => { IsAttackEnd = true; });
        anim = GetComponentInParent<Animator>();
        m_MonsterRenderer = GetComponentInParent<MonsterRenderer>();
        Deck = new MonsterAction[3];
        CurDeckCount = 0;
    }
    public new void SetUp(int curHP,int maxHP)
    {

        base.SetUp(curHP,maxHP);

    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            //일단 무조건 3개 뽑는다
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
    }
    public void SetIntent()
    {
        if (CurDeckCount >= 3)
        {
            SetDeck();
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

        
    }
    public IEnumerator Action()
    {
        List<GameObject> Player = new List<GameObject>();
        Player.Add(MainSceneController.Instance.Character);
        Attack();
        for (int i = 0; i < Deck[CurDeckCount].Repeat; ++i)
        {
            if (Deck[CurDeckCount].OnlyOnceAction)
            {
                m_MonsterRenderer.SetAnimation(m_MonsterRenderer.AnimClips[1], true,1.0f);
            }
            foreach (var func in Deck[CurDeckCount].Function)
            {
                func.CardAbility.OnExcute(Monster, MainSceneController.Instance.Character,func, 0);
            }

            yield return new WaitForSeconds(0.3f);
        }
        CurDeckCount++;
        Intent.GetComponent<IntentControl>().OnAction();
        IsAttackEnd = true;
    }
    public bool GetAttackEnd()
    {
        return IsAttackEnd;
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Destroy(HPBar);
        Destroy(Intent);
    }

}
