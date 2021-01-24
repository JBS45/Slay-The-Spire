using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextUI : MonoBehaviour
{

    [SerializeField]
    TMP_Text DamageText;

    [SerializeField]
    float Angle;

    int Force;
    float Timer;
    float Gravity = 9.8f;
    Vector3 OriginPos;
    Vector3 TargetPos;

    bool IsGetDamage = false;
    bool IsAttakDefence = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGetDamage)
        {
            ArcMove();
        }
        if (IsAttakDefence)
        {
            TopMove();
        }
    }
    public void AttackDefence(int damage,Vector3 Target)
    {
        DamageText.text = damage.ToString();
        DamageText.color = Color.cyan;
        IsAttakDefence = true;
        TargetPos = Target;
        OriginPos = transform.localPosition;
    }
    void TopMove()
    {
        Timer += Time.deltaTime * 10.0f;
        float TmpX = (40.0f-(5*Timer)) * Mathf.Cos(Angle * Mathf.Deg2Rad*Timer) ;
        float TmpY = 40.0f * Timer;
        transform.localPosition = OriginPos + new Vector3(TmpX, TmpY, 0);

        if (TargetPos.y-transform.localPosition.y < 1.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(int damage,int force,Vector3 DamagePos)
    {
        DamageText.text = damage.ToString();
        Force = force;
        IsGetDamage = true;
        OriginPos = transform.localPosition;
    }
    public void SetDamage(int damage, int force, Vector3 DamagePos,Color color)
    {
        DamageText.text = damage.ToString();
        DamageText.color = color;
        Force = force;
        IsGetDamage = true;
        OriginPos = transform.localPosition;
    }
    void ArcMove()
    {
        Timer += Time.deltaTime * 10.0f;
        float TmpX = Force * Mathf.Cos(Angle * Mathf.Deg2Rad) * Timer;
        float TmpY = (Force * Mathf.Sin(Angle * Mathf.Deg2Rad) * Timer)-(0.5f*Timer*Timer*Gravity);
        transform.localPosition = OriginPos+new Vector3(TmpX, TmpY, 0);

        if (transform.localPosition.y < -1000)
        {
            Destroy(this.gameObject);
        }
    }
}
