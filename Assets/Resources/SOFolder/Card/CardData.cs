using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class FunctionModule
{
    [Header("Ability")]
    public AbilityType Type;
    public string AbilityKey;

    [Header("Effect")]
    public GameObject SkillEffect;
    public Sprite SkillSprite;

    [Header("PowerType")]
    public PowerVariety variety;

    [Header("General Info")]
    public int Value;

    public int Stack;

    [TextArea(2, 3)]
    public string Decription;


    public FunctionModule(FunctionModule data)
    {
        Type = data.Type;
        AbilityKey = data.AbilityKey;
        SkillEffect = data.SkillEffect;
        SkillSprite = data.SkillSprite;
        variety = data.variety;
        Value = data.Value;
        Stack = data.Stack;
        Decription = data.Decription;
    }
}
[System.Serializable]
public enum AbilityType
{
    Attack,Skill,Power
}
[System.Serializable]
public class CardEnchantData
{
    public bool EnchantIsExtinct;
    public string EnchantCardName;
    public TargetOptions EnchantTargets;
    public int EnchantCost;
    public int EnchantRepeat;

    public List<int> EnchantValue;
}

[System.Serializable]
public class CardData : ITargetLoader
{
    bool m_IsEnable;
    public bool IsEnable { get => m_IsEnable; set => m_IsEnable = value; }

    bool m_IsExtinct;
    public bool IsExtinct { get => m_IsExtinct; }

    bool m_IsAllCost;
    public bool IsAllCost { get => m_IsAllCost; }

    CharacterType m_CharType;
    public CharacterType CharType { get { return m_CharType; } }

    string m_CardName;
    public string CardName { get { return m_CardName; } }

    CardType m_CardType;
    public CardType CardType { get { return m_CardType; } }

    RarityOptions m_Rarity;
    public RarityOptions Rarity { get { return m_Rarity; } }
    TargetOptions m_Targets;
    public TargetOptions Targets { get { return m_Targets; } }

    int m_Cost;
    public int Cost { get { return m_Cost; } set { m_Cost = Cost; } }

    int m_Repeat;
    public int Repeat { get { return m_Repeat; } set { m_Cost = m_Repeat; } }

    Sprite m_CardImage;
    public Sprite CardImage { get { return m_CardImage; } }

    bool m_MultipleEnchant;
    public bool MultipleEnchant { get { return m_MultipleEnchant; } }

    int m_EnchantCount = 0;
    public int EnchantCount { get { return m_EnchantCount; } set { m_EnchantCount = value; } }


    CardEnchantData m_EnchantData;
    public CardEnchantData Enchant { get { return m_EnchantData; } }

    List<FunctionModule> FunctionAndValue;
    public List<FunctionModule> Action { get { return FunctionAndValue; } }

    List<int> m_BaseValue;
    public List<int> BaseValue { get { return m_BaseValue; } }


    List<GameObject> TargetsList;

    GameObject Target;

    //어빌리티를 갖고
    //타겟 정해져서 사용되는 시점에 이 클래스에 타겟을 정해주고
    //타겟에게 효과가 사용되게 OnExcute 수정
    //특히 방어와 데미지는 민첩,손상 과 힘, 취약, 약화에 영향을 받음


    public CardData(CardAsset asset)
    {
        m_IsEnable = false;
        m_IsExtinct = asset.IsExtinct;
        m_IsAllCost = asset.IsAllCost;
        m_CharType = asset.charType;
        m_CardName = asset.CardName;
        m_CardType = asset.cardType;
        m_Rarity = asset.Rarity;
        m_Targets = asset.Targets;
        m_Repeat = asset.Repeat;
        m_Cost = asset.Cost;

        m_CardImage = asset.CardImage;


        m_MultipleEnchant = false;
        m_EnchantCount = 0;

        m_EnchantData = asset.EnchantData;

        FunctionAndValue = new List<FunctionModule>();
        foreach (var item in asset.FunctionAndValue)
        {
            FunctionModule tmp = new FunctionModule(item);
            FunctionAndValue.Add(tmp);
        }

        m_BaseValue = new List<int>();
        foreach(var action in FunctionAndValue)
        {
            m_BaseValue.Add(Mathf.Abs(action.Value));
        }

        TargetsList = new List<GameObject>();

    }
    public CardData(CardData data)
    {
        m_IsEnable = false;
        m_IsExtinct = data.IsExtinct;
        m_IsAllCost = data.IsAllCost;
        m_CharType = data.m_CharType;
        m_CardName = data.m_CardName;
        m_CardType = data.m_CardType;
        m_Rarity = data.m_Rarity;
        m_Targets = data.m_Targets;
        m_Repeat = data.m_Repeat;
        m_Cost = data.m_Cost;


        m_CardImage = data.CardImage;


        m_MultipleEnchant = data.MultipleEnchant;
        m_EnchantCount = data.EnchantCount;

        m_EnchantData = data.m_EnchantData;

        FunctionAndValue = new List<FunctionModule>();
        foreach (var item in data.FunctionAndValue)
        {
            FunctionModule tmp = new FunctionModule(item);
            FunctionAndValue.Add(tmp);
        }
        m_BaseValue = new List<int>();
        foreach (var action in FunctionAndValue)
        {
            m_BaseValue.Add(action.Value);
        }

        TargetsList = new List<GameObject>();

    }
    public IEnumerator OnExcute()
    {
        MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>().IsCardUsing(true);

        if (Targets == TargetOptions.Enemy)
        {
            for (int i = 0; i < Repeat; ++i)
            {
                foreach (var action in Action)
                {
                    switch (action.Type)
                    {
                        case AbilityType.Attack:
                            AttackManager.Instance.UseAttack(MainSceneController.Instance.Character, Target, action.SkillEffect, action.AbilityKey, action.Value, true);
                            break;
                        case AbilityType.Skill:
                            SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, Target, action.AbilityKey, action.Value, true);
                            break;
                        case AbilityType.Power:
                            PowerManager.Instance.AssginBuff(Target, action.variety, action.Value, true);
                            break;
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (Targets == TargetOptions.AllEnemy)
        {
            for (int i = 0; i < Repeat; ++i)
            {
                foreach (var monster in MainSceneController.Instance.BattleData.Monsters)
                {

                    foreach (var action in Action)
                    {
                        switch (action.Type)
                        {
                            case AbilityType.Attack:
                                AttackManager.Instance.UseAttack(MainSceneController.Instance.Character, monster, action.SkillEffect, action.AbilityKey, action.Value, true);
                                break;
                            case AbilityType.Skill:
                                SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, monster, action.AbilityKey, action.Value, true);
                                break;
                            case AbilityType.Power:
                                PowerManager.Instance.AssginBuff(monster, action.variety, action.Value, true);
                                break;
                        }
                    }
                }
                yield return new WaitForSeconds(0.2f);

            }
        }
        else
        {
            for (int i = 0; i < Repeat; ++i)
            {
                foreach (var action in Action)
                {
                    switch (action.Type)
                    {
                        case AbilityType.Attack:
                            AttackManager.Instance.UseAttack(MainSceneController.Instance.Character, MainSceneController.Instance.Character, action.SkillEffect, action.AbilityKey, action.Value, true);
                            break;
                        case AbilityType.Skill:
                            SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, MainSceneController.Instance.Character, action.AbilityKey, action.Value, true);
                            break;
                        case AbilityType.Power:
                            PowerManager.Instance.AssginBuff(MainSceneController.Instance.Character, action.variety, action.Value, true);
                            break;
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        if (MainSceneController.Instance.BattleData.CurrentBattelState == BattleDataState.Battle)
        {
            MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>().IsCardUsing(false);
            ClearTarget();
        }
    }
    public void CardEnchant()
    {
        if (MultipleEnchant || EnchantCount == 0)
        {
            m_IsExtinct = Enchant.EnchantIsExtinct;
            m_CardName = Enchant.EnchantCardName;
            m_Targets = Enchant.EnchantTargets;
            m_Cost -= Enchant.EnchantCost;
            m_Repeat += Enchant.EnchantRepeat;

            for (int i = 0; i < Action.Count; ++i)
            {
                Action[i].Value += Enchant.EnchantValue[i];
            }
            EnchantCount++;
        }
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }
    public GameObject GetTarget()
    {
        return Target;
    }
    public void ClearTarget()
    {
        Target = null;
    }
}
