using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/AllStritke")]
public class AllStrikeAllEffect : Ability
{


    public override void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        List<GameObject> tmpTarget = MainSceneController.Instance.BattleData.Monsters;
        for (int i = 0; i < tmpTarget.Count; ++i)
        {
            float result = Func.Value + (EnchantCount * Func.EnchantRate);
            int str = 0;
            //힘이 있으면 힘만큼 증가
            if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Strength))
            {
                str = Performer.GetComponentInChildren<Stat>().Powers.Find(power => power.Variety == PowerVariety.Strength).Value;
            }

            result += str;
            //플레이어가 약화 걸려있으면
            if (Performer.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Weak))
            {
                result *= 0.75f;
            }
            //타겟이 취약 걸려있으면
            if (tmpTarget[i].GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Fragile))
            {
                result *= 1.5f;
            }

            tmpTarget[i].GetComponentInChildren<Stat>().GetDamage((int)result);
            if (Func.SkillSprite != null)
            {
                tmpTarget[i].GetComponentInChildren<Stat>().MakeSkillEffect(Func.SkillSprite);
            }
        }
        Camera.main.GetComponent<CameraController>().CameraShakeFunc(0.05f, 1.0f);
    }

    public override int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount)
    {
        float result = Func.Value + (EnchantCount * Func.EnchantRate);
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