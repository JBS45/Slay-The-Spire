using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryDB : MonoBehaviour
{
    static MysteryDB _instance = null;
    public static MysteryDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MysteryDB>();
            }
            return _instance;
        }
    }

    Dictionary<string, Mystery> _data;
    public Dictionary<string, Mystery> DB { get => _data; }

    [SerializeField]
    List<Mystery> EventKeys;

    List<Mystery> _CurrentKey;
    public List<Mystery> CurrentKey { get => _CurrentKey; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _data = new Dictionary<string, Mystery>();
        _CurrentKey = new List<Mystery>();
        AllEvent();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AllEvent()
    {
        Mystery[] tmp = Resources.LoadAll<Mystery>("SOFolder/Mystery");
        foreach(var item in tmp)
        {
            _data.Add(item.Key, item);
        }
    }
    public void SaveInit(List<string> key)
    {
        foreach (var item in key) {
            _CurrentKey.RemoveAll(element => element.Key == item);
        }
    }
    public void NoSaveInit()
    {
        foreach(var item in EventKeys)
        {
            _CurrentKey.Add(item);
        }
    }
    public string RandomSelector()
    {
        if (_CurrentKey.Count == 0) return null;

        int random = Random.Range(0, _CurrentKey.Count);
        string TmpKey = _CurrentKey[random].Key;
        _CurrentKey.Remove(_CurrentKey.Find(item => item.Key == TmpKey));

        return TmpKey;
    }
}
