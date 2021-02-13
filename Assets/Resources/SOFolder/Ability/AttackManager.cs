using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    static AttackManager _instance = null;
    public static AttackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AttackManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public int UseAttack(GameObject Performer, GameObject Target, GameObject Effect,Sprite sprite, string key, int value, bool IsUse)
    {
        int result = 0;
        switch (key)
        {
            case "Kill":
                InstantKill();
                break;
            case "OneEffect":
                result = AttackOneEffect(Performer, Target, Effect,sprite, value, IsUse);
                break;
            case "Attack":
                result = Attack(Performer, Target, Effect,sprite, value, IsUse);
                break;
        }

        return result;
    }
    void InstantKill()
    {
        List<GameObject> tmpTarget = MainSceneController.Instance.BattleData.Monsters;
        for (int i = 0; i < tmpTarget.Count; ++i)
        {
            float result = tmpTarget[i].GetComponentInChildren<Stat>().CurrentHealthPoint;

            if (result >= tmpTarget[i].GetComponentInChildren<Stat>().CurrentHealthPoint)
            {
                result = tmpTarget[i].GetComponentInChildren<Stat>().CurrentHealthPoint - 1;
            }

            tmpTarget[i].GetComponentInChildren<Stat>().GetDamage((int)result);
        }
    }
    int AttackOneEffect(GameObject Performer, GameObject Target,GameObject Effect, Sprite sprite, int value, bool IsUse)
    {

        float result;
        float TmpValue = BasePerformer(Performer, value);
        //타겟이 취약 걸려있으면
        result = BaseTarget(Target,TmpValue);

        if (IsUse)
        {
            Target.GetComponentInChildren<Stat>().GetDamage((int)result);
            GameObject obj = Instantiate(Effect,MainSceneController.Instance.Spawner.MonsterSpawnPoint);
            obj.GetComponent<SkillEffect>().Setting(MainSceneController.Instance.Spawner.MonsterSpawnPoint);
            Camera.main.GetComponent<CameraController>().CameraShakeFunc(0.05f, 1.0f);
        }

        return (int)result;
    }

    int Attack(GameObject Performer, GameObject Target, GameObject Effect, Sprite sprite, int value, bool IsUse)
    {
        float result;
        float TmpValue = BasePerformer(Performer, value);
        //타겟이 취약 걸려있으면
        result=BaseTarget(Target,TmpValue);

        if (IsUse)
        {
            Target.GetComponentInChildren<Stat>().GetDamage((int)result);
            Target.GetComponentInChildren<Stat>().MakeSkillEffect(Effect,sprite);
            Camera.main.GetComponent<CameraController>().CameraShakeFunc(0.05f, 1.0f);
        }

        return (int)result;
    }
    float BasePerformer(GameObject Performer, int value)
    {
        float result = value;
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

        return result;
    }
    float BaseTarget(GameObject Target, float value)
    {
        float result = value;
        if (Target != null)
        {
            if (Target.GetComponentInChildren<Stat>().Powers.Exists(power => power.Variety == PowerVariety.Fragile))
            {
                result *= 1.5f;
            }
        }

        return result;
    }
}
