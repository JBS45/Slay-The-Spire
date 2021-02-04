using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : Stat,IObservers
{
    PlayerDataAsset m_PlayerData;
    public PlayerDataAsset PlayerData { get; }

    Animator anim;

    CharacterRenderer renderer;
    [SerializeField]
    SpriteRenderer Corpse;

    MeshRenderer Mesh;


    private new void Awake()
    {
        base.Awake();
        anim = GetComponentInParent<Animator>();
        renderer = GetComponentInParent<CharacterRenderer>();
        Mesh = GetComponent<MeshRenderer>();
        Corpse.enabled = false;
    }
    public void SetUp(PlayerDataAsset data)
    {
        m_PlayerData = data;
        m_PlayerData.Attach(this);
        base.SetUp(m_PlayerData.CurrentHp, m_PlayerData.MaxHp);

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
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        UpdateData();
        m_PlayerData.Notify();
        renderer.SetHitAnimation();

    }
    public override void Cure(int value)
    {
        base.Cure(value);
        UpdateData();
        m_PlayerData.Notify();
    }
    public void UpdateData()
    {
        if (IsDead())
        {
            Mesh.enabled = false;
            Corpse.enabled = true;
            m_PlayerData.CurrentHp = 0;
            m_PlayerData.MaxHp = base.MaxHP;
            SaveLoadManager.Instance.Delete();
            MainSceneController.Instance.BattleData.ChangeBattleState(BattleDataState.Lose);
        }
        else
        {
            m_PlayerData.CurrentHp = base.CurrentHP;
            m_PlayerData.MaxHp = base.MaxHP;
        }
    }
    new bool IsDead()
    {
        return base.IsDead();
    }
}
