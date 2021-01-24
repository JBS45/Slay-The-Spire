using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power/AllBuff")]
public class AllBuff : Ability
{

    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        List<GameObject> TmpTarget = MainSceneController.Instance.BattleData.Monsters;
        foreach (var target in TmpTarget)
        {
            float result = Func.Value + (EnchantCount * Func.EnchantRate);
            if (target.GetComponentInChildren<Stat>().Powers.Exists(Power => Power.Variety == Func.variety))
            {
                target.GetComponentInChildren<Stat>().Powers.Find(Power => Power.Variety == Func.variety).Value += (int)result;
                target.GetComponentInChildren<Stat>().MakeSkillEffect(Func.SkillSprite, Vector3.one);
            }
            else
            {
                Power tmpPower = new Power();
                tmpPower.Value = (int)result;
                tmpPower.Type = PowerType.Debuff;
                tmpPower.Variety = Func.variety;
                target.GetComponentInChildren<Stat>().Powers.Add(tmpPower);
                target.GetComponentInChildren<Stat>().MakeSkillEffect(Func.SkillSprite, Vector3.one);
            }
        }
    }
    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value + (EnchantCount * Func.EnchantRate);

        return (int)result;
    }

}