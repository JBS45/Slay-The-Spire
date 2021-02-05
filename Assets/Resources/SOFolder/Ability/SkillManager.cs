using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    static SkillManager _instance = null;
    public static SkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SkillManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    GameObject DefenceRes;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public int UseSkill(GameObject Performer,GameObject Target,string key ,int value, bool IsUse)
    {
        int result = 0;
        switch (key)
        {
            case "Defence":
                result = Defence(Performer, Target, value, IsUse);
                break;
            case "Draw":
                result = Draw(Performer, Target, value, IsUse);
                break;
            case "Heal":
                result = Heal(Performer, Target, value, IsUse);
                break;
            case "Energy":
                result = Energy(Performer, Target, value, IsUse);
                break;
            case "Slime":
                result = AddSlime(Performer, Target, value, IsUse);
                break;
            case "NoAction":
                result = NoAction(Performer, Target, value, IsUse);
                break;
        }

        return result;
    }

    int Defence(GameObject Performer, GameObject Target,int value, bool IsUse)
    {
        float result = value;
        int agi = 0;
        //민첩이 있으면 힘만큼 증가

        if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Agillity))
        {
            agi = Performer.GetComponentInChildren<Stat>().Powers.Find(power => power.Variety == PowerVariety.Agillity).Value;
        }

        result += agi;
        //손상 걸려있으면
        if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.injure))
        {
            result *= 0.75f;
        }

        if (IsUse)
        {
            Performer.GetComponentInChildren<Stat>().SetDefence((int)result);
            Performer.GetComponentInChildren<Stat>().MakeSkillEffect(DefenceRes);
        }
        return (int)result;
    }

    int Draw(GameObject Performer, GameObject Target, int value, bool IsUse)
    {
        float result = value;
        if (IsUse)
        {
            MainSceneController.Instance.BattleData.DrawCard((int)result,null);
        }
        return (int)result;
    }
    int Heal(GameObject Performer,GameObject Target,int value,bool IsUse)
    {
        float result = value;
        if (IsUse)
        {
            Target.GetComponentInChildren<Stat>().Cure((int)result);
        }
        return (int)result;
    }
    int Energy(GameObject Performer, GameObject Target, int value, bool IsUse)
    {
        float result = value;
        if (IsUse)
        {
            MainSceneController.Instance.BattleData.CurrentEnergy += (int)result;
        }
        return (int)result;
    }
    int AddSlime(GameObject Performer, GameObject Target, int value, bool IsUse)
    {
        float result = value;
        if (IsUse)
        {
            CardData tmp = new CardData(CardDB.Instance.Condition.Card.Find(item => item.CardName == "점액투성이"));
            MainSceneController.Instance.BattleData.CardData.Discard.Add(tmp);
        }
        return (int)result;
    }
    int NoAction(GameObject Performer, GameObject Target, int value, bool IsUse)
    {
        return 0;
    }
}
