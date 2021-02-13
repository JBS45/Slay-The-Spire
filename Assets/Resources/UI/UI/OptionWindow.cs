using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class OptionWindow : MonoBehaviour
{
    [SerializeField]
    Slider BGMSlider;
    [SerializeField]
    Slider SESlider;

    [SerializeField]
    GameObject Exit;

    [SerializeField]
    GameObject SaveGoTitle;

    private void Awake()
    {
        BGMSlider.value = AudioManager.BGMVolume;
        SESlider.value = AudioManager.SEVolume;
        if(SceneManager.GetActiveScene().name== "TitleScene")
        {
            SaveGoTitle.SetActive(false);
        }

        Exit.GetComponentInChildren<Button>().onClick.AddListener(SaveData);
        SaveGoTitle.GetComponentInChildren<Button>().onClick.AddListener(SaveExit);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SaveData()
    {
        AudioManager.SetSEVolume(SESlider.value);
        AudioManager.SetBGMVolume(BGMSlider.value);
        PlayerPrefs.SetInt("Save", 1);
        PlayerPrefs.Save();
        Destroy(this.gameObject);
    }
    void SaveExit()
    {
        SaveData();
        AudioManager.Clear();
        MainSceneController.Instance.UIControl.FadeOutEffect(SceneChange);
    }
    void SceneChange()
    {
        AudioManager.Clear();
        SceneManager.LoadScene("TitleScene");
        System.GC.Collect();
    }
}
