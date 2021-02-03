using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField]
    GameObject ButtonRes;

    List<GameObject> ButtonList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSetting(string ButtonText,Delvoid del)
    {
        if (ButtonList == null)
        {
            ButtonList = new List<GameObject>();
        }
        GameObject tmp = Instantiate(ButtonRes, transform);
        tmp.GetComponentInChildren<TMP_Text>().text = ButtonText;
        tmp.GetComponentInChildren<Button>().onClick.AddListener(()=> { del(); });
        ButtonList.Add(tmp);
    }
    public void Clear()
    {
        if (ButtonList!=null&&ButtonList.Count > 0)
        {
            foreach (var item in ButtonList)
            {
                Destroy(item);
            }
            ButtonList.Clear();
        }
    }
}
