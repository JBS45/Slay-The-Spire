using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RelicData : IDrawEvent, ITurnBegin, ITurnEnd, ICardUse, ICardExtinct, IBattleStart, IBattleEnd
{
    bool m_IsEnable;
    public bool IsEnable { get => m_IsEnable; set => m_IsEnable = value; }
    string m_Name;
    public string Name { get => m_Name; set => m_Name = value; }
    CharacterType m_CharType;
    public CharacterType CharType { get => m_CharType; set => m_CharType = value; }
    string m_Description;
    public string Description { get => m_Description; set => m_Description = value; }
    GameObject m_Prefab;
    public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
    Sprite m_RelicImage;
    public Sprite RelicImage { get => m_RelicImage; set => m_RelicImage = value; }

    bool m_IsStack;
    public bool IsStack { get => m_IsStack; set => m_IsStack = value; }
    bool m_IsDecrease;
    int m_StackCount;
    public int StackCount { get => m_StackCount; set => m_StackCount = value; }
    int m_MaxStack;
    public int MaxStack { get => m_MaxStack; set => m_MaxStack = value; }

    Timing m_ExcuteTiming;
    public Timing ExcuteTiming { get => m_ExcuteTiming; }
    TargetOptions m_Target;
    public TargetOptions Target { get => m_Target; }
    List<FunctionModule> m_Action;
    public List<FunctionModule> Action { get => m_Action; }

    IObservers LinkPrefab;

    public void DrawCard(CardData data)
    {
        if (ExcuteTiming != Timing.CardDraw) return;
        StackCheck();
    }
    public void TurnBegin()
    {
        if (ExcuteTiming != Timing.TurnBegin) return;
        StackCheck();
    }
    public void TurnEnd()
    {
        if (ExcuteTiming != Timing.TurnEnd) return;
        StackCheck();
    }
    public void CardUse(CardData data)
    {
        if (ExcuteTiming != Timing.CardUse) return;
        StackCheck();
    }
    public void CardExtinct(CardData data)
    {
        if (ExcuteTiming != Timing.CardExtinct) return;
        StackCheck();
    }
    public void BattleStart()
    {
        if (ExcuteTiming != Timing.BattleStart) return;
        StackCheck();
    }
    public void BattleEnd()
    {
        if (ExcuteTiming != Timing.BattleEnd) return;
        StackCheck();
    }

    void StackCheck()
    {
        if (IsEnable)
        {
            if (IsStack)
            {
                if (m_IsDecrease)
                {
                    if (StackCount > 0)
                    {
                        StackCount--;
                        OnExcute();
                    }
                    if (StackCount == 0)
                    {
                        IsEnable = false;
                    }

                }
                else
                {
                    if (StackCount >= MaxStack)
                    {
                        StackCount = 0;
                        OnExcute();
                    }
                }
            }
            else
            {
                OnExcute();
            }
        }
    }
    void OnExcute()
    {
        LinkPrefab.UpdateData();
        switch (Target)
        {
            case TargetOptions.NoTarget:
                foreach (var Func in Action)
                {
                    switch (Func.Type)
                    {
                        case AbilityType.Skill:
                            SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, MainSceneController.Instance.Character, Func.AbilityKey, Func.Value, true);
                            break;
                        case AbilityType.Power:
                            PowerManager.Instance.AssginBuff(MainSceneController.Instance.Character, Func.variety, Func.Value,true);
                            break;
                    }
                }
                break;
            case TargetOptions.AllEnemy:
                foreach (var monster in MainSceneController.Instance.BattleData.Monsters)
                {
                    foreach (var Func in Action)
                    {
                        switch (Func.Type)
                        {
                            case AbilityType.Attack:
                                AttackManager.Instance.UseAttack(MainSceneController.Instance.Character, monster, Func.SkillEffect, Func.AbilityKey, Func.Value, true);
                                break;
                            case AbilityType.Skill:
                                SkillManager.Instance.UseSkill(MainSceneController.Instance.Character, monster, Func.AbilityKey, Func.Value, true);
                                break;
                            case AbilityType.Power:
                                PowerManager.Instance.AssginBuff(monster, Func.variety, Func.Value,true);
                                break;
                        }
                    }
                }
                break;
        }
    }
    public void Attach(IObservers observer)
    {
        LinkPrefab = observer;
    }
    public void Detach()
    {
        LinkPrefab = null;
    }

    public RelicData(Relic data)
    {
        IsEnable = true;
        Name = data.Name;
        CharType = data.CharType;
        Description = data.Description;

        Prefab = data.Prefab;
        RelicImage = data.RelicImage;

        IsStack = data.IsStack;
        m_IsDecrease = data.IsDecrease;
        StackCount = data.StackCount;
        MaxStack = data.MaxStack;

        m_ExcuteTiming = data.ExcuteTiming;
        m_Target = data.Target;
        m_Action = data.Action;
    }

}
[System.Serializable]
public class SaveRelic
{
    public string RelicName;
    public int StackCount;

    public SaveRelic()
    {

    }
    public SaveRelic(RelicData data)
    {
        RelicName = data.Name;
        StackCount = data.StackCount;
    }
    public RelicData GetRelic()
    {
        RelicData tmp = RelicDB.Instance.RelicDatas.Find(item => item.Name == RelicName);
        tmp.StackCount = StackCount;

        return tmp;
    }
}
