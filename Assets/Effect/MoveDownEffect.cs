using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownEffect : MonoBehaviour, SkillEffect
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
    public void OnExcute()
    {
        StartCoroutine(FadeOutMoveDown());
    }
    IEnumerator FadeOutMoveDown()
    {
        transform.localPosition = new Vector3(0, 1, 0);
        while (Vector3.Distance(transform.localPosition,Vector3.zero) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,Vector3.zero, Time.deltaTime*10.0f);
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
