﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Timing
{
    None,TurnBegin,TurnEnd,BattleStart,BattleEnd,CardUse,CardDraw,CardExtinct,CardDiscard
}

public enum PowerType
{
    Buff,Debuff
}
public enum PowerVariety
{
    None=0,Strength, Agillity, Fragile, Weak, injure, Entangle,Ritual,Rage,Split,Demon
}
[System.Serializable]
public class Power:IDrawEvent,ITurnBegin,ITurnEnd,ICardUse,ICardExtinct,IBattleStart,IBattleEnd
{
    public Timing Timing;
    public PowerType Type;
    public PowerVariety Variety;
    public int Value;
    public bool IsEnable = true;
    GameObject Target;


    public void DrawCard(CardData data)
    {
        switch (Variety)
        {
            case PowerVariety.Entangle:
                if (data.CardType == CardType.Attack)
                {
                    data.IsEnable = false;
                }
                break;
        }
    }
    public void TurnBegin()
    {
        switch (Variety)
        {
            case PowerVariety.Ritual:
                PowerManager.Instance.AssginBuff(Target, PowerVariety.Strength, Value,true);
                break;
            case PowerVariety.Demon:
                PowerManager.Instance.AssginBuff(Target, PowerVariety.Strength, Value, true);
                break;
        }
    }
    public void TurnEnd()
    {
        switch (Variety)
        {
            case PowerVariety.Fragile:
            case PowerVariety.injure:
            case PowerVariety.Weak:
            case PowerVariety.Entangle:
                Value--;
                if (Value <= 0)
                {
                    IsEnable = false;
                    PowerManager.Instance.FinishPower(Target, Variety);
                }
                break;
        }
        
    }
    public void CardUse(CardData data)
    {
        switch (Variety)
        {
            case PowerVariety.Rage:
                if (data.CardType == CardType.Skill)
                {
                    PowerManager.Instance.AssginBuff(Target, PowerVariety.Strength, Value, true);
                }
                break;
        }
    }
    public void CardExtinct(CardData data)
    {

    }
    public void BattleStart()
    {

    }
    public void BattleEnd()
    {

    }
    public void GetDamage()
    {
        switch (Variety)
        {
            case PowerVariety.Split:
                if (Target.GetComponentInChildren<Stat>().CurrentHealthPoint <= Value)
                {
                    //타겟의 인터페이스 split함수를 실행하면 될거 같음
                    Target.GetComponentInChildren<ISplit>().Split();
                }
                break;
        }
    }
    public void SetTarget(GameObject target)
    {
        Target = target;
    }
}

