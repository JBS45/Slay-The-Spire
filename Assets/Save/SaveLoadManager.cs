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

    string FileName = "/SaveFile.json";
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
        if (File.Exists(Application.persistentDataPath + filename))
        {
            del();
        }
        else
        {
            File.Create(Application.persistentDataPath + filename).Dispose();
            while (File.Exists(Application.persistentDataPath + filename))
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
        File.WriteAllText(Application.persistentDataPath + filename, jData);

    }
    public bool Load(ref SaveDataStruct data)
    {
        if (File.Exists(Application.persistentDataPath + FileName))
        {
            string jData = File.ReadAllText(Application.persistentDataPath + FileName);
            //byte[] bytes = System.Convert.FromBase64String(jData);
            //string reformat = System.Text.Encoding.UTF8.GetString(bytes);
            if (jData.Length < 1) return false;

            //data = JsonConvert.DeserializeObject<PlayerInfo>(reformat);
            data = JsonConvert.DeserializeObject<SaveDataStruct>(jData);

            if (data!=null&&data.IsSave)
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
        if (File.Exists(Application.persistentDataPath + FileName))
        {
            File.Delete(Application.persistentDataPath + FileName);
        }
    }
}
