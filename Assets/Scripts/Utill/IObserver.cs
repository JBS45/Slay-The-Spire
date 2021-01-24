using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardObserver
{
    //각 덱의 갯수나 어떤 카드가 사용되었는지등의 온갖 정보를 넘겨줌
    void UpdateData(int deck, int discard, int extinction, int hand, int currEnergy);
    void DrawCard(CardData data);
}
