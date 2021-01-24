using System.Collections;
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
    None=0,Strength, Agillity, Fragile, Weak, injure, Entangle
}
[System.Serializable]
public class Power:IDrawEvent,ITurnBegin,ITurnEnd,ICardUse,ICardExtinct,IBattleStart,IBattleEnd
{
    protected Timing m_Timing;
    protected PowerType m_Type;
    public PowerType Type { get => m_Type; set => m_Type = value; }
    protected PowerVariety m_Variety;
    public PowerVariety Variety { get => m_Variety; set => m_Variety = value; }
    protected int m_Value;
    public int Value { get => m_Value; set => m_Value = value; }
    protected bool m_IsEnable = true;
    public bool IsEnable { get => m_IsEnable; set => m_IsEnable = value; }



    public void DrawCard(CardData data)
    {
        switch (m_Variety)
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
        switch (m_Variety)
        {
            
        }
    }
    public void TurnEnd()
    {
        switch (m_Variety)
        {
            case PowerVariety.Fragile:
            case PowerVariety.injure:
            case PowerVariety.Weak:
            case PowerVariety.Entangle:
                Value--;
                if (Value <= 0)
                {
                    IsEnable = false;
                }
                break;
        }
        
    }
    public void CardUse(CardData data)
    {

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
}

