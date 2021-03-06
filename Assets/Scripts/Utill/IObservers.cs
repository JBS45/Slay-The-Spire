﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservers
{
    //각 덱의 갯수나 어떤 카드가 사용되었는지등의 온갖 정보를 넘겨줌
    void UpdateData();
}
public interface IShopWindow
{
    void ShopUISetting(int gold, ShopWindowDelegate del);
    void ShopRefresh();
    void ShopButtonEvent(ShopWindowDelegate del);
    void HandEnter();
    void HandExit();
}
public delegate void ShopWindowDelegate(IShopWindow shopWindow);

public interface IDrawEvent
{
    void DrawCard(CardData data);
}

public interface ITurnBegin
{
    void TurnBegin();
}

public interface ITurnEnd
{
    void TurnEnd();
}

public interface ICardUse
{
    void CardUse(CardData data);
}

public interface ICardExtinct
{
    void CardExtinct(CardData data);
}
public interface IBattleStart
{
    void BattleStart();
}
public interface IBattleEnd
{
    void BattleEnd();
}
public interface IGetDamage
{
    void GetDamage();
}
public interface IDeckChange
{
    CardData OriginDeckChange(CardData data);
}