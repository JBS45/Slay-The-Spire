using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IronWave : MonoBehaviour, SkillEffect
{
    [SerializeField]
    GameObject[] Waves;

    Transform target;
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
        this.target = target;
        transform.SetParent(MainSceneController.Instance.Character.transform);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        OnExcute();
    }
    public void Setting(Transform target)
    {
        this.target = target;
        transform.SetParent(MainSceneController.Instance.Character.transform);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        OnExcute();
    }
    public void OnExcute()
    {
        StartCoroutine(Wave());
    }
    IEnumerator Wave()
    {
        float distance = (target.localPosition.x + target.parent.localPosition.x)-(MainSceneController.Instance.Character.transform.localPosition.x+ MainSceneController.Instance.Character.transform.parent.localPosition.x);
        int Last = Waves.Length;
        for (int i = 0; i < Last; ++i)
        {
            Waves[i].transform.localPosition = new Vector3(1+(distance / Last) * i*1.3f, 10+i*3, 0);
        }
        while (Waves[Last - 1].transform.localPosition.y > 0.1f)
        {
            foreach(var item in Waves)
            {
                item.transform.localPosition = new Vector3(item.transform.localPosition.x, Mathf.Lerp(item.transform.localPosition.y, 0, Time.deltaTime * 10f), item.transform.localPosition.z);
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }

}
