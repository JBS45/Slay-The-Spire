using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicData : IDrawEvent, ITurnBegin, ITurnEnd, ICardUse, ICardExtinct, IBattleStart, IBattleEnd
{

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
    bool m_IsOnceStack;
    public bool IsOnceStack { get => m_IsOnceStack; set => m_IsOnceStack = value; }
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
        if (IsStack)
        {
            if (IsOnceStack)
            {
                if (StackCount > 0)
                {
                    StackCount--;
                    OnExcute();
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
    void OnExcute()
    {
        LinkPrefab.UpdateData();
        switch (Target)
        {
            case TargetOptions.NoTarget:
                foreach (var Func in Action)
                {
                    Func.CardAbility.OnExcute(MainSceneController.Instance.Character, MainSceneController.Instance.Character, Func, 0);
                }
                break;
            case TargetOptions.AllEnemy:
                foreach (var monster in MainSceneController.Instance.BattleData.Monsters)
                {
                    foreach (var Func in Action)
                    {
                        Func.CardAbility.OnExcute(MainSceneController.Instance.Character, monster, Func, 0);
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
        Name = data.Name;
        CharType = data.CharType;
        Description = data.Description;

        Prefab = data.Prefab;
        RelicImage = data.RelicImage;

        IsStack = data.IsStack;
        IsOnceStack = data.IsOnceStack;
        StackCount = data.StackCount;
        MaxStack = data.MaxStack;

        m_ExcuteTiming = data.ExcuteTiming;
        m_Target = data.Target;
        m_Action = data.Action;
    }

}
