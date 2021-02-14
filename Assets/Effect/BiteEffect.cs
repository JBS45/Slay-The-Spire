using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BiteEffect : MonoBehaviour, SkillEffect
{
    [SerializeField]
    GameObject Top;
    [SerializeField]
    GameObject Bottom;
    [SerializeField]
    AudioClip clip;
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
        StartCoroutine(Bite());
    }
    IEnumerator Bite()
    {
        Top.transform.localPosition = new Vector3(0, 1.5f, 0);
        Bottom.transform.localPosition = new Vector3(0, -1.5f, 0);

        Vector3 TopTarget = new Vector3(0, 0.5f, 0);
        Vector3 BottomTarget = new Vector3(0, 0.5f, 0);

        MainSceneController.Instance.AtkSEManager.PlaySE(clip);
        while (Vector3.Distance(Top.transform.localPosition, TopTarget) > 0.1f)
        {
            Top.transform.localPosition = Vector3.Lerp(Top.transform.localPosition, TopTarget, Time.deltaTime * 5f);
            Bottom.transform.localPosition = Vector3.Lerp(Bottom.transform.localPosition, BottomTarget, Time.deltaTime * 5f);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
