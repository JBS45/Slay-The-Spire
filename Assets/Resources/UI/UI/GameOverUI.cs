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

    [SerializeField]
    TMP_Text Detail;

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

        Detail.text = "";
        Detail.text += string.Format("오른 층 수({0})\n", MainSceneController.Instance.PlayerData.CurrentFloor);
        Detail.text += string.Format("처치한 몬스터({0})\n", MainSceneController.Instance.PlayerData.Monster);
        Detail.text += string.Format("처치한 엘리트({0})\n", MainSceneController.Instance.PlayerData.Elite);
        Detail.text += string.Format("처치한 보스({0})\n", MainSceneController.Instance.PlayerData.Boss);

    }
    void ButtonEvent()
    {
        MainSceneController.Instance.UIControl.FadeInEffect(SceneChange);
    }
    void SceneChange()
    {
        AudioManager.Clear();
        SaveLoadManager.Instance.Delete();
        SceneManager.LoadScene("TitleScene");
        System.GC.Collect();
    }
}
