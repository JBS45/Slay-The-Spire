using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ParticleSystemJobs;

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
    [SerializeField]
    ParticleSystem ExtinctionEffect;
    [SerializeField]
    Gradient ExtinctionGradient;

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
        Anim.enabled = true;
        foreach (var item in AfterImages)
        {
            item.color = HighlightColor;
            item.enabled = true;
        }
        Highlight.enabled = true;

        Anim.SetBool("AfterImage", true);
    }
    public void afterEffectOff()
    {
        Anim.enabled = false;
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
    public void Extinction()
    {
        ParticleSystem.MainModule main = ExtinctionEffect.main;
        //main.startColor = ExtinctionGradient.Evaluate(Random.Range(0f, 1f));
        ExtinctionEffect.Play();
    }
}
