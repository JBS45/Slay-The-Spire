using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/AllPercentDamage")]
public class AllPercentDamage : Ability
{


    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        AttackManager.Instance.UseAttack(Performer, Target, Func.SkillEffect,"Kill", Func.Value, true);
    }

    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value;
        int str = 0;
        //힘이 있으면 힘만큼 증가
        if (MainSceneController.Instance.Character.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Strength))
        {
            str = Performer.GetComponentInChildren<Stat>().Powers.Find(power => power.Variety == PowerVariety.Strength).Value;
        }

        result += str;
        //플레이어가 약화 걸려있으면
        if (MainSceneController.Instance.Character.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Weak))
        {
            result *= 0.75f;
        }


        return (int)result;
    }

}