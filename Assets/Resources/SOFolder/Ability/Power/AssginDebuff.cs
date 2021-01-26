using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power/DeBuff")]
public class AssginDebuff : Ability
{
    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value;
        if (Target.GetComponentInChildren<Stat>().Powers.Exists(Power => Power.Variety == Func.variety))
        {
            Target.GetComponentInChildren<Stat>().Powers.Find(Power => Power.Variety == Func.variety).Value += (int)result;
            Target.GetComponentInChildren<Stat>().MakeSkillEffect(Func.SkillSprite, Vector3.one);
        }
        else
        {
            Power tmpPower = new Power();
            tmpPower.Value = (int)result;
            tmpPower.Type = PowerType.Buff;
            tmpPower.Variety = Func.variety;
            Target.GetComponentInChildren<Stat>().Powers.Add(tmpPower);
            Target.GetComponentInChildren<Stat>().MakeSkillEffect(Func.SkillSprite, Vector3.one);
        }

    }
    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value;

        return (int)result;
    }

}