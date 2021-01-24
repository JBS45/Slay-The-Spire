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
    [Header("Genaral Info")]

    public List<MonsterPattern> MonsterPool;
}
