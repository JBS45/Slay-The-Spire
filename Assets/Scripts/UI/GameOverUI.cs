using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    GameObject Banner;

    [SerializeField]
    GameObject Button;

    [SerializeField]
    Transform TextContent;
    [SerializeField]
    TMP_Text TextRes;

    List<TMP_Text> TextList;
    private void Awake()
    {
        TextList = new List<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUI(bool IsClear)
    {
        Banner.GetComponentInChildren<TMP_Text>().text = IsClear ? "승리" : "굴복";
        Button.GetComponentInChildren<Button>().onClick.AddListener(ButtonEvent);

    }
    void ButtonEvent()
    {
        MainSceneController.Instance.UIControl.FadeInEffect(SceneChange);
    }
    void SceneChange()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
