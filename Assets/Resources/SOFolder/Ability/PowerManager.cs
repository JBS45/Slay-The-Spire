using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager: MonoBehaviour
{
    static PowerManager _instance = null;
    public static PowerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PowerManager>();
            }
            return _instance;
        }
    }
    [SerializeField]
    Sprite[] PowerImage;

    [Header("Effect"),SerializeField]
    GameObject PowerEffectRes;
    [SerializeField]
    GameObject FinishPowerEffectRes;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int AssginBuff(GameObject Target,PowerVariety variety,int value,bool IsUse)
    {
        int result = 0;
        switch (variety)
        {
            case PowerVariety.Strength:
                if (IsUse)
                {
                    Buff(Target, PowerVariety.Strength, value, PowerImage[0]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Agillity:
                if (IsUse)
                {
                    Buff(Target, PowerVariety.Agillity, value, PowerImage[1]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Fragile:
                if (IsUse)
                {
                    Debuff(Target, PowerVariety.Fragile, value, PowerImage[2]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Weak:
                if (IsUse)
                {
                    Debuff(Target, PowerVariety.Weak, value, PowerImage[3]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.injure:
                if (IsUse)
                {
                    Debuff(Target, PowerVariety.injure, value, PowerImage[4]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Entangle:
                if (IsUse)
                {
                    Debuff(Target, PowerVariety.Entangle, value, PowerImage[5]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Ritual:
                if (IsUse)
                {
                    Buff(Target, PowerVariety.Ritual, value, PowerImage[6]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Rage:
                if (IsUse)
                {
                    Buff(Target, PowerVariety.Rage, value, PowerImage[7]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Split:
                if (IsUse)
                {
                    Buff(Target, PowerVariety.Split, value, PowerImage[8]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
            case PowerVariety.Demon:
                if (IsUse)
                {
                    Buff(Target, PowerVariety.Demon, value, PowerImage[9]);
                }
                else
                {
                    result = Mathf.Abs(value);
                }
                break;
        }
        return result;
    }
    public void FinishPower(GameObject Target,PowerVariety variety)
    {
        switch (variety)
        {
            case PowerVariety.Fragile:
                Target.GetComponentInChildren<Stat>().MakeFinishPowerEffect(FinishPowerEffectRes, PowerImage[2]);
                break;
            case PowerVariety.Weak:
                Target.GetComponentInChildren<Stat>().MakeFinishPowerEffect(FinishPowerEffectRes, PowerImage[3]);
                break;
            case PowerVariety.injure:
                Target.GetComponentInChildren<Stat>().MakeFinishPowerEffect(FinishPowerEffectRes, PowerImage[4]);
                break;
            case PowerVariety.Entangle:
                Target.GetComponentInChildren<Stat>().MakeFinishPowerEffect(FinishPowerEffectRes, PowerImage[5]);
                break;
        }
    }
    void FinishPowerEffect(GameObject Target,PowerVariety variety,Sprite sprite)
    {
        Target.GetComponentInChildren<Stat>().MakeSkillEffect(PowerEffectRes, sprite);
    }
    void Buff(GameObject Target, PowerVariety variety, int value,Sprite sprite)
    {
        MainSceneController.Instance.AtkSEManager.BattleSEPlay(BattelSEType.Buff);
        Target.GetComponentInChildren<Stat>().MakeSkillEffect(PowerEffectRes,sprite);
        if (Target.GetComponentInChildren<Stat>().Powers.Exists(Power => Power.Variety == variety))
        {
            Target.GetComponentInChildren<Stat>().Powers.Find(Power => Power.Variety == variety).Value += value;
            Target.GetComponentInChildren<Stat>().PowerRefresh();
        }
        else
        {
            Power tmpPower = new Power();
            tmpPower.Value = value;
            tmpPower.Type = PowerType.Buff;
            tmpPower.Variety = variety;
            tmpPower.SetTarget(Target);
            Target.GetComponentInChildren<Stat>().Powers.Add(tmpPower);
            Target.GetComponentInChildren<Stat>().AddPowerUI(tmpPower, sprite);
        }
    }
    void Debuff(GameObject Target, PowerVariety variety, int value, Sprite sprite)
    {
        MainSceneController.Instance.AtkSEManager.BattleSEPlay(BattelSEType.Debuff);
        Target.GetComponentInChildren<Stat>().MakeSkillEffect(PowerEffectRes, sprite);
        if (Target.GetComponentInChildren<Stat>().Powers.Exists(Power => Power.Variety == variety))
        {
            Target.GetComponentInChildren<Stat>().Powers.Find(Power => Power.Variety == variety).Value += value;
            Target.GetComponentInChildren<Stat>().PowerRefresh();
        }
        else
        {
            Power tmpPower = new Power();
            tmpPower.Value = value;
            tmpPower.Type = PowerType.Debuff;
            tmpPower.Variety = variety;
            tmpPower.SetTarget(Target);
            Target.GetComponentInChildren<Stat>().Powers.Add(tmpPower);
            Target.GetComponentInChildren<Stat>().AddPowerUI(tmpPower, sprite);
        }
    }
}
