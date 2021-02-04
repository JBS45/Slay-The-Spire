using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BackgroundType
{
    Battle,FireCamp
}
public class BackgroundScript : MonoBehaviour
{
   
    public enum WallType
    {
        Type1=0, Type2, Type3
    }

    WallType m_Wall;

    [Header("Battle")]
    [SerializeField]
    SpriteRenderer GameOverBackground;
    [SerializeField]
    SpriteRenderer[] Background;

    [SerializeField]
    SpriteRenderer[] WallSprite;

    [SerializeField]
    GameObject[] SpawnPoint;

    [Header("Fire Camp")]
    [SerializeField]
    SpriteRenderer CampFireBackGround;
    [SerializeField]
    SpriteRenderer[] Fire;
    [SerializeField]
    ParticleSystem[] FireEffect;

    [SerializeField]
    ParticleSystem[] SmokeEffect;

    [SerializeField]
    SpriteRenderer Shoulder;
    [SerializeField]
    SpriteRenderer Shoulder2;
    // Start is called before the first frame update
    void Start()
    {
        WallChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackgroundChange(BackgroundType type)
    {
        Alloff();

        switch (type)
        {
            case BackgroundType.Battle:
                foreach (var item in SpawnPoint)
                {
                    item.SetActive(true);
                }
                foreach (var item in Background)
                {
                    item.enabled = true;
                }
                foreach(var item in WallSprite)
                {
                    item.enabled = true;
                }
                WallChange();
                break;
            case BackgroundType.FireCamp:
                CampFireBackGround.enabled = true;
                foreach (var item in Fire)
                {
                    item.enabled = true;
                }
                foreach (var item in FireEffect)
                {
                    item.Play();
                }
                foreach (var item in SmokeEffect)
                {
                    item.Play();
                }
                Shoulder.enabled = true;
                StartCoroutine(ShoulderMove());
                break;
        }
    }
    public void FireOff()
    {
        foreach (var item in Fire)
        {
            item.enabled = false;
        }
        foreach (var item in FireEffect)
        {
            item.Stop();
        }
        foreach (var item in SmokeEffect)
        {
            item.Stop();
        }
        Shoulder.enabled = false;
        Shoulder2.enabled = true;
    }
    void Alloff()
    {
        foreach (var item in Background)
        {
            item.enabled = false;
        }
        foreach(var item in WallSprite)
        {
            item.enabled = false;
        }
        foreach(var item in SpawnPoint)
        {
            item.SetActive(false);
        }
        CampFireBackGround.enabled = false;
        foreach(var item in Fire)
        {
            item.enabled = false;
        }
        foreach(var item in FireEffect)
        {
            item.Stop();
        }
        foreach (var item in SmokeEffect)
        {
            item.Stop();
        }
        Shoulder.enabled = false;
        Shoulder2.enabled = false;
    }

    void WallTypeChange(WallType Wall)
    {

        m_Wall = Wall;
        switch (m_Wall)
        {
            case WallType.Type1:
                WallSprite[0].enabled = true;
                WallSprite[1].enabled = false;
                WallSprite[2].enabled = false;
                break;
            case WallType.Type2:
                WallSprite[0].enabled = true;
                WallSprite[1].enabled = true;
                WallSprite[2].enabled = false;
                break;
            case WallType.Type3:
                WallSprite[0].enabled = false;
                WallSprite[1].enabled = false;
                WallSprite[2].enabled = true;
                break;
        }
    }
    void WallChange()
    {
        int random = Random.Range(0, 3);
        WallTypeChange((WallType)random);
    }
    IEnumerator ShoulderMove()
    {
        Shoulder.transform.localPosition = new Vector3(-2, 1, 0);
        Vector3 target = new Vector3(0, 1, 0);
        while (Vector3.Distance(Shoulder.transform.localPosition, target) > 0.1f)
        {
            Shoulder.transform.localPosition = Vector3.MoveTowards(Shoulder.transform.localPosition,target, 0.3f);
            yield return null;
        }
        Shoulder.transform.localPosition = target;
    }
    public void ChangeGameOver()
    {
        foreach(var item in Background)
        {
            item.enabled = false;
        }
        foreach (var item in WallSprite)
        {
            item.enabled = false;
        }
        GameOverBackground.enabled = true;
    }
}
