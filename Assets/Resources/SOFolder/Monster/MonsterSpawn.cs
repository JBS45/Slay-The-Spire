using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPattern
{
    public List<MonsterAsset> Monsters;
}
[CreateAssetMenu(menuName = "MonsterSpawn")]
public class MonsterSpawn : ScriptableObject
{
    [Header("Slime")]
    public List<MonsterAsset> Slime;

    [Header("Normal")]

    public List<MonsterPattern> MonsterPool;

    [Header("Elite")]

    public List<MonsterPattern> Elite;

    [Header("Boss")]

    public List<MonsterPattern> Boss;
}
