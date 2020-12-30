using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneEventButton : MonoBehaviour
{
    public TMP_Text Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonSetting(string text,Delvoid Del)
    {
        Text.text = text;
        if (Del != null)
        {
            GetComponent<Button>().onClick.AddListener(() => { Del(); });
        }
    }
}
