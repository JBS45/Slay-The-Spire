using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera m_Camera;

    [SerializeField] float m_Force;
    [SerializeField] Vector3 m_Offset = Vector3.zero;

    Quaternion m_OriginQuat;
    float m_Timer;

    // Start is called before the first frame update
    private void Awake()
    {
        m_OriginQuat = transform.rotation;
        m_Timer = 0.0f;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraShakeFunc(float time,float Force)
    {
        m_Force = Force;
        StartCoroutine(CameraShake(time));
    }
    IEnumerator CameraShake(float time)
    {
        Vector3 TmpOriginRot = transform.eulerAngles;


        while (m_Timer<=time)
        {
            float TmpRotX = Random.Range(-m_Offset.x, m_Offset.x);
            float TmpRotY = Random.Range(-m_Offset.y, m_Offset.y);
            float TmpRotZ = Random.Range(-m_Offset.z, m_Offset.z);

            Vector3 TmpRandom = TmpOriginRot + new Vector3(TmpRotX, TmpRotY, TmpRotZ);
            Quaternion TmpQuat = Quaternion.Euler(TmpRandom);
            while (Quaternion.Angle(transform.rotation, TmpQuat) > 0.001f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, TmpQuat, m_Force);
                m_Timer += Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }
        m_Timer = 0.0f;
        transform.rotation = m_OriginQuat;
    }
}
