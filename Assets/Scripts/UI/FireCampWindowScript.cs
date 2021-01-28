using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCampWindowScript : MonoBehaviour
{
    [SerializeField]
    GameObject Title;
    [SerializeField]
    GameObject Text;

    [SerializeField]
    GameObject RestBtn;
    [SerializeField]
    GameObject EnchantBtn;

    [SerializeField]
    GameObject ProccedBtn;

    GameObject CardWindow;

    Vector3 ProceedBtnOriginPos;

    private void Awake()
    {
        RestBtn.GetComponent<CampFireButton>().SetButtonEvent(Rest);
        ProceedBtnOriginPos = ProccedBtn.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Rest()
    {
        int CureValue = (int)(MainSceneController.Instance.PlayerData.MaxHp * 0.3f);
        MainSceneController.Instance.Character.GetComponentInChildren<Stat>().Cure(CureValue);
        MainSceneController.Instance.UIControl.FadeInEffect(FireOff);
    }
    public void Enchant()
    {
        if(CardWindow == null)
        {
            CardWindow = MainSceneController.Instance.UIControl.MakeCardWindow();
            CardWindow.GetComponent<CardWindow>().Cancel.onClick.AddListener(() => {CardWindow.SetActive(false); });
            CardWindow.GetComponent<CardWindow>().SetEnchantCardButtonEvent(FireOff);
        }
        else
        {
            CardWindow.SetActive(true);
        }

    }

    public void FireOff()
    {
        MainSceneController.Instance.Background.FireOff();
        StartCoroutine(OnEnableProceedButton());
        MainSceneController.Instance.UIControl.FadeOutEffect(()=> { });
        RestBtn.SetActive(false);
        EnchantBtn.SetActive(false);
        Title.SetActive(false);
        Text.SetActive(false);
        if (CardWindow!=null){
            Destroy(CardWindow);
        }
    }
    IEnumerator OnEnableProceedButton()
    {
        ProccedBtn.transform.localPosition = ProceedBtnOriginPos;
        Vector3 target = ProceedBtnOriginPos - new Vector3(250, 0, 0);
        while (Vector3.Distance(ProccedBtn.transform.localPosition, target) > 1.0f)
        {
            ProccedBtn.transform.localPosition = Vector3.MoveTowards(ProccedBtn.transform.localPosition, target, 10.0f);
            yield return null;
        }
        ProccedBtn.transform.localPosition = target;
    }
    public void Proceed()
    {
        MainSceneController.Instance.GetUIControl().OpenMapProgress();
    }
}
