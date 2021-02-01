using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/SimpleStritke")]
public class SimpleStrike : Ability
{

    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        AttackManager.Instance.UseAttack(Performer, Target, Func.SkillEffect, "Attack", Func.Value, true);
    }
    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EncahntCount)
    {
        float result = Func.Value;
        int str = 0;
        if (Performer != null)
        {
            //힘이 있으면 힘만큼 증가
            if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Strength))
            {
                str = Performer.GetComponentInChildren<Stat>().GetComponentInChildren<Stat>().Powers.Find(power => power.Variety == PowerVariety.Strength).Value;
            }

            result += str;
            //약화 걸려있으면
            if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Weak))
            {
                result *= 0.75f;
            }
        }
        //타겟이 취약 걸려있으면
        if (Target!=null&&Target != null && Target.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Fragile))
        {
            result *= 1.5f;
        }

        return (int)result;
    }
}