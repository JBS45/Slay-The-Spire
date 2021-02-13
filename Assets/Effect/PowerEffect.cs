using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerEffect : MonoBehaviour, SkillEffect
{
    [SerializeField]
    SpriteRenderer Renderer;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setting(Transform target,Sprite sprite)
    {
        transform.SetParent(target);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        Renderer.sprite = sprite;

        OnExcute();
    }
    public void Setting(Transform target)
    {
        transform.SetParent(target);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        OnExcute();
    }
    public void OnExcute()
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        while (Renderer.color.a>0.1f)
        {
            Renderer.transform.localScale += new Vector3(Time.deltaTime*2 , Time.deltaTime * 2, Time.deltaTime * 2);
            Renderer.color -= new Color(0, 0, 0, Time.deltaTime*2);
            yield return null;
        }
        Destroy(this.gameObject);
    }
    
}
