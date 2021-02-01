using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicDB : MonoBehaviour
{
    static RelicDB _instance = null;
    public static RelicDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RelicDB>();
            }
            return _instance;
        }
    }

    [SerializeField]
    List<Relic> Relic;

    List<RelicData> _RelicDatas;
    List<RelicData> _CurrentRelicDatas;
    public List<RelicData> RelicDatas { get => _CurrentRelicDatas; set => _CurrentRelicDatas = value; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _RelicDatas = new List<RelicData>();
        _CurrentRelicDatas = new List<RelicData>();
    }
    public void NoSaveInit()
    {
        foreach(var item in Relic)
        {
            RelicData tmp = new RelicData(item);
            _RelicDatas.Add(tmp);
        }
        _CurrentRelicDatas = _RelicDatas;
    }
    public RelicData RandomSelector()
    {
        if (_CurrentRelicDatas.Count == 0) return null;

        int random = Random.Range(0, _CurrentRelicDatas.Count);
        RelicData tmp = _CurrentRelicDatas[random];
        _CurrentRelicDatas.RemoveAt(random);

        return tmp;
    }
}
