using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    BossSlime=0
}
public enum NPCType
{
    Neow = 0, Merchant,Chest
}
public enum MonsterType
{
    BlueSlaver=0,RedSlaver
}
public class MonsterSpawner : MonoBehaviour
{
    public Transform MonsterSpawnPoint;


    [SerializeField]
    GameObject Neow;
    [SerializeField]
    GameObject Merchant;
    [SerializeField]
    GameObject Chest;
    [SerializeField]
    MonsterSpawn MonsterPool;

    private void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NPCSpawn(NPCType Type)
    {
        GameObject obj;
        switch (Type)
        {
            case NPCType.Neow:
                obj = Instantiate(Neow);
                obj.tag = "NPC";
                obj.transform.SetParent(MonsterSpawnPoint);
                MainSceneController.Instance.BattleData.Monsters.Add(obj);
                break;
            case NPCType.Merchant:
                obj = Instantiate(Merchant, MonsterSpawnPoint);
                obj.tag = "NPC";
                obj.transform.localPosition = Vector3.zero;
                MainSceneController.Instance.BattleData.Monsters.Add(obj);
                break;
            case NPCType.Chest:
                obj = Instantiate(Chest, MonsterSpawnPoint);
                obj.tag = "NPC";
                obj.transform.localPosition = Vector3.zero;
                MainSceneController.Instance.BattleData.Monsters.Add(obj);
                break;
        }
    }
    public void MonsterSpawn()
    {

        int random = Random.Range(0, MonsterPool.MonsterPool.Count);
        int randomHP;

        List<MonsterAsset> tmp = new List<MonsterAsset>();
        tmp = MonsterPool.MonsterPool[random].Monsters;
        for (int i = 0; i < tmp.Count; ++i)
        {
            GameObject obj = Instantiate(tmp[i].Prefab);
            obj.tag = "Monster";
            obj.transform.SetParent(MonsterSpawnPoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localPosition = new Vector3(0,0,0) - new Vector3(4f, 0, 0)* (tmp.Count-1) + new Vector3(6f, 0, 0)*i;
            randomHP = Random.Range(tmp[i].MinHp, tmp[i].MaxHp + 1);
            obj.GetComponentInChildren<Stat>().SetUp(randomHP, randomHP);
            obj.GetComponentInChildren<IMonsterPatten>().SetPattern(tmp[i]);
            MainSceneController.Instance.BattleData.Monsters.Add(obj);
        }
    }

    public void EliteSpawn()
    {

        int random = Random.Range(0, MonsterPool.Elite.Count);
        int randomHP;

        List<MonsterAsset> tmp = new List<MonsterAsset>();
        tmp = MonsterPool.Elite[random].Monsters;
        for (int i = 0; i < tmp.Count; ++i)
        {
            GameObject obj = Instantiate(tmp[i].Prefab);
            obj.tag = "Monster";
            obj.transform.SetParent(MonsterSpawnPoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localPosition = new Vector3(0, 0, 0) - new Vector3(4f, 0, 0) * (tmp.Count - 1) + new Vector3(6f, 0, 0) * i;
            randomHP = Random.Range(tmp[i].MinHp, tmp[i].MaxHp + 1);
            obj.GetComponentInChildren<Stat>().SetUp(randomHP, randomHP);
            obj.GetComponentInChildren<IMonsterPatten>().SetPattern(tmp[i]);
            MainSceneController.Instance.BattleData.Monsters.Add(obj);
        }
    }
    public void BossSpawn(BossType boss)
    {

        int randomHP;

        List<MonsterAsset> tmp = new List<MonsterAsset>();
        tmp = MonsterPool.Boss[(int)boss].Monsters;
        for (int i = 0; i < tmp.Count; ++i)
        {
            GameObject obj = Instantiate(tmp[i].Prefab);
            obj.tag = "Monster";
            obj.transform.SetParent(MonsterSpawnPoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localPosition = new Vector3(0, 0, 0) - new Vector3(4f, 0, 0) * (tmp.Count - 1) + new Vector3(6f, 0, 0) * i;
            randomHP = Random.Range(tmp[i].MinHp, tmp[i].MaxHp + 1);
            obj.GetComponentInChildren<Stat>().SetUp(randomHP, randomHP);
            obj.GetComponentInChildren<IMonsterPatten>().SetPattern(tmp[i]);
            MainSceneController.Instance.BattleData.Monsters.Add(obj);
        }
    }
}
