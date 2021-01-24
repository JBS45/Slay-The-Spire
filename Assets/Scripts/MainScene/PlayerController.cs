using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController
{
    BattleUIScript BattleUI;
    BattleData BattleData;
    GameObject Hand;

    bool IsKeyDown = false;
    bool IsCardUsing = false;
    
    public PlayerController(BattleUIScript UI, BattleData Data)
    {
        BattleUI = UI;
        BattleData = Data;
    }

    public void InputHandler()
    {
        if (!IsCardUsing)
        {
            KeyBoardInput();
            MouseInput();
        }

    }
    void KeyBoardInput()
    {
        int KeyNum = 48;
        if (!IsKeyDown)
        {
            for (int i = 0; i < 10; ++i)
            {
                if ((Input.GetKeyDown((KeyCode)KeyNum + (i+1)) && BattleUI.Hand.Count >= (i+1)))
                {
                    if (BattleUI.Hand[i].GetComponent<BattleCardData>().IsEnable)
                    {
                        SetHand(BattleUI.Hand[i]);
                        Hand.GetComponent<BattleCardData>().ChangeState(HandCardState.CardSelect);
                        IsKeyDown = true;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha0) && BattleUI.Hand.Count >= 10)
            {
                if (BattleUI.Hand[9].GetComponent<BattleCardData>().IsEnable)
                {
                    SetHand(BattleUI.Hand[9]);
                    Hand.GetComponent<BattleCardData>().ChangeState(HandCardState.CardSelect);
                    IsKeyDown = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && 
            BattleData.CurrentTurn==Turn.Player&&BattleData.CurrentTurnState==TurnState.Turn)
        {
            BattleUI.TurnEnd();
        }
    }
    void MouseInput()
    {
        if (Hand != null)
        {
            IsKeyDown = true;
        }
        if (Hand!=null&& Input.GetMouseButtonDown(0))
        {

            if (Hand.GetComponent<BattleCardData>().Data.Targets == TargetOptions.Enemy)
            {
                GameObject Target = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool IsHit = Physics2D.GetRayIntersection(ray);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);


                if (IsHit) Target = hit.transform.gameObject;


                if (Target != null && Target.tag == "Monster")
                {
                    //.다른 카드들 락 걸어놓은거 해제
                    BattleUI.GetComponent<BattleUIScript>().SetCardState(HandCardState.Idle);
                    //대상을 먼저 지정
                    Hand.GetComponent<BattleCardData>().Data.SetTarget(Target);
                    Hand.GetComponent<BattleCardData>().ChangeState(HandCardState.Using);
                }
                else
                {
                    //현재 상태에서 벗어남
                    Target = null;
                    BattleUI.GetComponent<BattleUIScript>().SetCardState(HandCardState.Idle);
                    BattleUI.GetComponent<BattleUIScript>().CardAlign();
                }

            }
            else
            {
                //타겟 지정이 아님

                //범위내 클릭이면 사용
                if (Hand.GetComponent<BattleCardData>().IsInRange)
                {
                    BattleUI.GetComponent<BattleUIScript>().SetCardState(HandCardState.Idle);
                    Hand.GetComponent<BattleCardData>().ChangeState(HandCardState.Using);
                }
                //범위외 클릭이면 비사용
                else
                {
                    BattleUI.GetComponent<BattleUIScript>().SetCardState(HandCardState.Idle);
                    BattleUI.GetComponent<BattleUIScript>().CardAlign();
                }
            }

            Hand = null;
            IsKeyDown = false;
            
        }
    }
    public void SetHand(GameObject hand)
    {
        Hand = hand;
        Hand.GetComponent<BattleCardData>().SetLastSibling();
    }
    public void SetIsCardUsing(bool IsUsing)
    {
        IsCardUsing = IsUsing;
    }
}
