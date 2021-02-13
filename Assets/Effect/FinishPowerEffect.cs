using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishPowerEffect : MonoBehaviour, SkillEffect
{
    [SerializeField]
    Image Sprite;
    [SerializeField]
    TMP_Text Text;

    bool IsEnd;
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
        Sprite.sprite = sprite;

        OnExcute();
    }
    public void Setting(Transform target)
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        //OnExcute();
    }
    public void OnExcute()
    {
        StartCoroutine(FadeOutMoveUp());
    }
    IEnumerator FadeOutMoveUp()
    {
        Vector3 Target = transform.localPosition + new Vector3(0, 200, 0);
        while (Vector3.Distance(transform.localPosition, Target) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Target, 10.0f);
            yield return null;
        }
        MainSceneController.Instance.SEManager.BattleSEPlay(BattelSEType.Buff);
        transform.localPosition = Target;
        while (Sprite.color.a > 0.1f)
        {
            Sprite.color -= new Color(0, 0, 0, Time.deltaTime * 2.0f);
            Text.color -= new Color(0, 0, 0, Time.deltaTime * 2.0f);
            yield return null;
        }
        IsEnd = true;
        Destroy(this.gameObject);
    }
}
