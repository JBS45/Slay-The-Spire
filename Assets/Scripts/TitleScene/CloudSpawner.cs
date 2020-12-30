using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Delvoid();
public class CloudSpawner : MonoBehaviour
{
    public Sprite[] CloudSpriteResources;
    public GameObject CloudRes;
    int CurCloudCount;
    public int CloudMax;
    public float AvgSpeed;

    int tmpCount;

    Delvoid DelCloudCount;

    public Vector3 TargetPos1;
    public Vector3 TargetPos2;

    private void Awake()
    {
        CurCloudCount = 0;
        tmpCount = 0;
        DelCloudCount += DecreaseCloud;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnCloud(Vector3 Start,Vector3 Dest)
    {
        //스프라이트 배열 안에 있는 그림 중 하나를 랜덤하게 선택한다.
        if (CloudSpriteResources.Length > 0)
        {
            int random = Random.Range(0, CloudSpriteResources.Length - 1);
            GameObject obj = Instantiate(CloudRes);
            obj.transform.SetParent(this.gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.name = "Cloud" + tmpCount;

            //Cloud에 세팅해준다.
            obj.GetComponent<CloudScript>().SettingCloud(Start, Dest, CloudSpriteResources[random], DelCloudCount,AvgSpeed);
            CurCloudCount++;
            tmpCount++;
        }
        
    }
    void DecreaseCloud()
    {
        CurCloudCount--;
    }
    public IEnumerator StartSpawnCloud()
    {
        //처음 게임 시작 될 때 4개 생성
        for(int i = 0; i < 4; ++i)
        {
            Vector3 tmp = new Vector3(Random.Range(-500, -2500), Random.Range(-700, 700), 0);
            SpawnCloud(TargetPos1 + tmp, TargetPos2 + tmp);
        }
        while (true)
        {
            float RandomTime = Random.Range(-0.5f, 2.5f);
            if (CurCloudCount < CloudMax)
            {
                Vector3 tmp = new Vector3(0, Random.Range(-700, 700), 0);
                if (RandomTime <= 1.5f)
                {
                    SpawnCloud(TargetPos1 + tmp, TargetPos2 + tmp);
                }
                else
                {
                    SpawnCloud(TargetPos2 + tmp, TargetPos1 + tmp);
                }
            }
            yield return new WaitForSeconds(1.0f+RandomTime);
        }
    }
}
