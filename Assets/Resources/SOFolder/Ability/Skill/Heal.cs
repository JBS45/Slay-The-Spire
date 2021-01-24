using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Heal")]
public class Heal : Ability
{

    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value + (EnchantCount * Func.EnchantRate);

        Target.GetComponentInChildren<Stat>().Cure((int)result);
        
    }
    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value + (EnchantCount * Func.EnchantRate);

        return (int)result;
    }

}