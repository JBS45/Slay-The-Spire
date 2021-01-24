using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public abstract void OnExcute(GameObject Performer, GameObject Target, FunctionModule Func,int EnchantCount);
    public abstract int PredictValue(GameObject Performer, GameObject Target, FunctionModule Func, int EnchantCount);
}
