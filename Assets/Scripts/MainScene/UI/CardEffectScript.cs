using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBattleCard
{
    void OnEffectOn(int CurEnergy);
}

public class CardEffectScript : MonoBehaviour
{

    [Header("After Image ")]
    [SerializeField]
    GameObject AfterImage;
    Image[] AfterImages;

    [SerializeField]
    Color HighlightColor;
    [SerializeField]
    Image Highlight;
    [SerializeField]
    TrailRenderer Trail;

    Animator Anim;
    private void Awake()
    {
        Trail.enabled = false;
        Anim = GetComponent<Animator>();
        AfterImages = AfterImage.GetComponentsInChildren<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        afterEffectOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void afterEffectOn()
    {
        foreach(var item in AfterImages)
        {
            item.color = HighlightColor;
            item.enabled = true;
        }
        Highlight.enabled = true;

        Anim.SetBool("AfterImage", true);
    }
    public void afterEffectOff()
    {
        foreach (var item in AfterImages)
        {
            item.color = HighlightColor;
            item.enabled = false;
        }
        Highlight.enabled = false;

        Anim.SetBool("AfterImage", false);
    }
    public void InRangeAnimation()
    {
        foreach (var item in AfterImages)
        {
            item.color = Color.white;
        }
        Anim.SetTrigger("InRange");
    }
    public void OnTrail()
    {
        Trail.enabled = true;
    }
}
