using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
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
    public void SetData(Transform target,Sprite sprite)
    {
        transform.SetParent(target);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        Renderer.sprite = sprite;

        StartCoroutine(FadeOut());
    }
    public void SetData(Transform target, Sprite sprite,Vector2 size)
    {
        transform.SetParent(target);
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0, 0.25f, 0);
        transform.localScale = size;
        Renderer.sprite = sprite;

        StartCoroutine(FadeOutMoveDown());
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
        Debug.Log("?"); 
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
