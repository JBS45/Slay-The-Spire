using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SkillEffect
{
    void Setting(Transform target,Sprite sprite);
    void Setting(Transform target);
    void OnExcute();
}
public class SimpleEffect : MonoBehaviour, SkillEffect
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
            Renderer.color -= new Color(0, 0, 0, Time.deltaTime*2);
            yield return null;
        }
        Destroy(this.gameObject);
    }
    IEnumerator FadeOutMoveDown()
    {
        while (Vector3.Distance(Vector3.zero, transform.localPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,Vector3.zero, Time.deltaTime*2.0f);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        while (Renderer.color.a > 0.1f)
        {
            Renderer.color -= new Color(0, 0, 0, Time.deltaTime * 2.0f);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
