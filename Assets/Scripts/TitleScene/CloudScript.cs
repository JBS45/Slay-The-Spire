using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudScript : MonoBehaviour
{
    public Image CloudSprite;
    Delvoid CurCloudCount;
    bool IsMoving = false;
    Vector3 m_Dest;
    float m_Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
        {
            Move2Dest();
        }
    }

    public void SettingCloud(Vector3 Start, Vector3 Dest, Sprite sprite,Delvoid Del,float Speed)
    {
        Speed += Random.Range(0.1f, 1.0f);
        int RandomFlipNum = Random.Range(0, 10);

        if (RandomFlipNum >= 6)
        {
            CloudSprite.rectTransform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        CloudSprite.sprite = sprite;
        CloudSprite.SetNativeSize();
        this.gameObject.transform.localPosition = Start;


        //CloudSpawner의 CurCloudCount를 감소시키는 delegate를 받아옴
        CurCloudCount = Del;
        IsMoving = true;
        m_Dest = Dest;
        m_Speed = Speed;

    }

    void Move2Dest()
    {
        //목적지까지 틱당 Speed만큼 이동
        this.gameObject.transform.localPosition = Vector3.MoveTowards(this.gameObject.transform.localPosition, m_Dest, m_Speed);
        if (Vector3.Distance(this.gameObject.transform.localPosition, m_Dest) <= 1.0f)
        {
            this.gameObject.transform.localPosition = m_Dest;
            CurCloudCount();
            Destroy(this.gameObject);
        }
    }
}
