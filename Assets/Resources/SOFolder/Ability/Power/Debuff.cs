using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power/Debuff")]
public class Debuff : Ability
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
            PowerManager.Instance.AssginBuff(target, Func.variety, Func.Value,true);
        }
    }
    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Mathf.Abs(Func.Value);

        return (int)result;
    }

}