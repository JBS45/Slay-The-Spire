using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEvent : MonoBehaviour
{
    Delvoid AnimationEnd;

    public void OnAnimationEnd()
    {
        AnimationEnd.Invoke();
    }
    public void SetAnimationEnd(Delvoid del)
    {
        AnimationEnd += del;
    }
}
