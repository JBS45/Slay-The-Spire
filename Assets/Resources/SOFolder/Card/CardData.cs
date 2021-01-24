using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class FunctionModule
{
    [Header("Ability")]
    public Ability CardAbility;

    [Header("Effect")]
    public GameObject SkillEffect;
    public Sprite SkillSprite;

    [Header("PowerType")]
    public PowerVariety variety;

    [Header("General Info")]
    public int Value;

    public int Repeat;
    public int RepeatEnchantRate;
    public int EnchantRate;

    public int Stack;

    [TextArea(2, 3)]
    public string Decription;

}

[System.Serializable]
public class CardData: ITargetLoader
{
    bool m_IsEnable;
    public bool IsEnable { get => m_IsEnable; set => m_IsEnable = value; }

    bool m_IsExtinct;
    public bool IsExtinct { get => m_IsExtinct; }

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

    Sprite m_CardImage;
    public Sprite CardImage { get { return m_CardImage; } }

    bool m_MultipleEnchant;
    public bool MultipleEnchant { get { return m_MultipleEnchant; } }

    int m_EnchantCount = 0;
    public int EnchantCount { get { return m_EnchantCount; } set { m_EnchantCount = value; } }



    List<FunctionModule> FunctionAndValue;
    public List<FunctionModule> Action { get { return FunctionAndValue; } }

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
        m_CharType = asset.charType;
        m_CardName = asset.CardName;
        m_CardType = asset.cardType;
        m_Rarity = asset.Rarity;
        m_Targets = asset.Targets;
        m_Cost = asset.Cost;

        m_CardImage = asset.CardImage;
 

        m_MultipleEnchant = false;
        m_EnchantCount = 0;

        FunctionAndValue = asset.FunctionAndValue;

        TargetsList = new List<GameObject>();

    }
    public CardData(CardData data)
    {
        m_IsEnable = false;
        m_IsExtinct = data.m_IsExtinct;
        m_CharType = data.m_CharType;
        m_CardName = data.m_CardName;
        m_CardType = data.m_CardType;
        m_Rarity = data.m_Rarity;
        m_Targets = data.m_Targets;
        m_Cost = data.m_Cost;

        m_CardImage = data.CardImage;


        m_MultipleEnchant = false;
        m_EnchantCount = 0;

        FunctionAndValue = data.FunctionAndValue;

        TargetsList = new List<GameObject>();

    }
    public IEnumerator OnExcute()
    {
        MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>().IsCardUsing(true);

        if (Action[0].Repeat > 1)
        {
            int tmpRepeat = Action[0].Repeat + (Action[0].RepeatEnchantRate * m_EnchantCount);
            for (int i = 0; i < tmpRepeat; ++i)
            {
                Action[0].CardAbility.OnExcute(MainSceneController.Instance.Character, Target, Action[0], EnchantCount);
                yield return new WaitForSeconds(0.2f);
            }
        }
        else
        {
            foreach (var Func in Action)
            {
                int tmpRepeat = Func.Repeat + (Func.RepeatEnchantRate * m_EnchantCount);
                for (int i = 0; i < tmpRepeat; ++i)
                {
                    Func.CardAbility.OnExcute(MainSceneController.Instance.Character, Target, Func, EnchantCount);
                }
            }
        }
        if (MainSceneController.Instance.BattleData.CurrentBattelState == BattleDataState.Battle)
        {
            MainSceneController.Instance.UIControl.GetCurUI().GetComponent<BattleUIScript>().IsCardUsing(false);
            ClearTarget();
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
