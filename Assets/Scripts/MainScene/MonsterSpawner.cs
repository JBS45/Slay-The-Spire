using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform MonsterSpawnPoint;

    public enum SpawnMonsterType
    {
        Neow = 0,Merchant
    }

    GameObject Neow;
    GameObject Merchant;
    GameObject[] Monsters;

    private void Awake()
    {

        Neow = Resources.Load<GameObject>("NPC/Neow/Neow");

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MonsterSpawn(SpawnMonsterType MonsterType)
    {
        GameObject obj;
        switch (MonsterType)
        {
            case SpawnMonsterType.Neow:
                obj = Instantiate(Neow);
                obj.tag = "NPC";
                obj.transform.SetParent(MonsterSpawnPoint);
                MainSceneController.Instance.m_BattleData.GetMonsterData().Add(obj);
                break;
        }
    }

}
