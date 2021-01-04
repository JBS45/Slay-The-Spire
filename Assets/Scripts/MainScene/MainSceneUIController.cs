using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUIController : MonoBehaviour
{

    public Canvas UICanvas;
    public Image FadePanel;
    public BarScript InfoBar;
    public GameObject ZeroPanel;
    public GameObject BattleUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void FadeInEffect(Delvoid Del)
    {
        FadePanel.enabled = true;
        StartCoroutine(FadeIn(Del));
        
    }

    public void FadeOutEffect(Delvoid Del)
    {
        FadePanel.enabled = true;
        StartCoroutine(FadeOut(Del));

    }

    IEnumerator FadeIn(Delvoid Del)
    {

        FadePanel.transform.SetAsLastSibling();
        float alpha = FadePanel.color.a;
        while (alpha >= 0.01f)
        {
            alpha -= Time.deltaTime * 2.0f;
            FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, alpha);
            yield return null;
        }
        FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, 0);
        FadePanel.enabled = false;
        if (Del != null)
        {
            Del();
        }
    }
    IEnumerator FadeOut(Delvoid Del)
    {
        FadePanel.transform.SetAsLastSibling();
        float alpha = FadePanel.color.a;
        while (alpha <= 0.99f)
        {
            alpha += Time.deltaTime * 2.0f;
            FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, alpha);
            yield return null;
        }
        FadePanel.color = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, 1);
        FadePanel.enabled = false;
        if (Del != null)
        {
            Del();
        }

    }
    public void ZeroFloorUI()
    {
        GameObject Obj;
        Obj = Instantiate(ZeroPanel);
        Obj.transform.SetParent(UICanvas.transform);
        Obj.transform.localScale = Vector3.one;
        Obj.transform.localPosition = new Vector3(-480.0f, -390.0f, 0.0f);
    }
    public void MakeBattleUI()
    {
        GameObject obj = Instantiate(BattleUI);
        obj.transform.SetParent(UICanvas.transform);
        obj.transform.localScale = Vector3.one;
    }

    
}
