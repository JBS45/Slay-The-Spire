using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Defence")]
public class Defence : Ability
{

    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value + (EnchantCount * Func.EnchantRate);
        int agi = 0;
        //민첩이 있으면 힘만큼 증가
        if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Strength))
        {
            agi = Performer.GetComponentInChildren<Stat>().Powers.Find(power => power.Variety == PowerVariety.Strength).Value;
        }

        result += agi;
        //손상 걸려있으면
        if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.injure))
        {
            result *= 0.75f;
        }

        Performer.GetComponentInChildren<Stat>().SetDefence((int)result);
        if (Func.SkillSprite != null)
        {
            Performer.GetComponentInChildren<Stat>().MakeSkillEffect(Func.SkillSprite, Vector3.one);
        }
    }
    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value + (EnchantCount * Func.EnchantRate);
        int agi = 0;
        //민첩이 있으면 힘만큼 증가
        if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Strength))
        {
            agi = Performer.GetComponentInChildren<Stat>().Powers.Find(power => power.Variety == PowerVariety.Strength).Value;
        }

        result += agi;
        //플레이어가 손상 걸려있으면
        if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.injure))
        {
            result *= 0.75f;
        }

        return (int)result;
    }

}