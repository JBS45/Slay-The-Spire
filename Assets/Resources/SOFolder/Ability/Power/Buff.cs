using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power/Buff")]
public class Buff : Ability
{

    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        List<GameObject> TmpTarget = new List<GameObject>();
        if (Target == null)
        {
            TmpTarget = MainSceneController.Instance.BattleData.Monsters;
        }
        else
        {
            TmpTarget.Add(Target);
        }
        foreach (var target in TmpTarget)
        {
            float result = Func.Value;
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
        float result = Mathf.Abs(Func.Value);

        return (int)result;
    }

}