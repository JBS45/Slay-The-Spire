using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Relic")]
public class Relic : ScriptableObject
{
    public bool CanGet;
    [Header("General Info")]
    public string Name;
    public CharacterType CharType;
    public string Description;

    [Header("Image Info")]
    public GameObject Prefab;
    public Sprite RelicImage;

    [Header("Stack")]
    public bool IsStack;
    public bool IsDecrease;
    public int StackCount;
    public int MaxStack;

    [Header("Function Info")]
    public Timing ExcuteTiming;
    public TargetOptions Target;
    public List<FunctionModule> Action;


}
