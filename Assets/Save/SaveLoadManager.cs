using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    static SaveLoadManager _instance = null;
    public static SaveLoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveLoadManager>();
            }
            return _instance;
        }
    }

    string FileName = "/Save/SaveFile.json";
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void Save(SaveDataStruct data)
    {
        StartCoroutine(CheckFile(FileName, ()=> { _Save(data, FileName); }));
    }
    IEnumerator CheckFile(string filename,Delvoid del)
    {
        if (File.Exists(Application.dataPath + filename))
        {
            del();
        }
        else
        {
            File.Create(Application.dataPath + filename).Dispose();
            while (File.Exists(Application.dataPath + filename))
            {
                yield return null;
            }
            del();
            
        }
    }
    void _Save(SaveDataStruct data, string filename)
    {
        string jData = JsonConvert.SerializeObject(data);
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jData);
        //string format = System.Convert.ToBase64String(bytes);
        //                                   "/JsonTest.json"
        //File.WriteAllText(Application.dataPath + filename, format);
        File.WriteAllText(Application.dataPath + filename, jData);

    }
    public bool Load(ref SaveDataStruct data)
    {
        if (File.Exists(Application.dataPath + FileName))
        {
            string jData = File.ReadAllText(Application.dataPath + FileName);
            //byte[] bytes = System.Convert.FromBase64String(jData);
            //string reformat = System.Text.Encoding.UTF8.GetString(bytes);


            //data = JsonConvert.DeserializeObject<PlayerInfo>(reformat);
            data = JsonConvert.DeserializeObject<SaveDataStruct>(jData);
            Debug.Log(data);
            if (data.IsSave)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
    public void Delete()
    {
        
    }
}
