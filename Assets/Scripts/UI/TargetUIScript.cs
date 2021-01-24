using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUIScript : MonoBehaviour
{
    [SerializeField]
    GameObject LeftTop;
    [SerializeField]
    GameObject RightTop;
    [SerializeField]
    GameObject LeftBot;
    [SerializeField]
    GameObject RightBot;

    Vector3[] Targets = new Vector3[4];
    void Awake()
    {
;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTargetSize(Vector3 LT, Vector3 RT, Vector3 LB, Vector3 RB)
    {
        Targets[0] = LT;
        Targets[1] = RT;
        Targets[2] = LB;
        Targets[3] = RB;

        LeftTop.transform.localPosition = Targets[0];
        RightTop.transform.localPosition = Targets[1];
        LeftBot.transform.localPosition = Targets[2];
        RightBot.transform.localPosition = Targets[3];
    }
    public void TargetOn()
    {
        StartCoroutine(TargetAnimation());
    }
    IEnumerator TargetAnimation()
    {
        LeftTop.transform.localPosition = Targets[0] + new Vector3(-50, 50, 0);
        RightTop.transform.localPosition = Targets[1] + new Vector3(50, 50, 0);
        LeftBot.transform.localPosition = Targets[2] + new Vector3(-50, -50, 0);
        RightBot.transform.localPosition = Targets[3] + new Vector3(50, -50, 0);

        while (Vector3.Distance(LeftTop.transform.localPosition, Targets[0]) > 1.0f)
        {
            LeftTop.transform.localPosition = Vector3.MoveTowards(LeftTop.transform.localPosition, Targets[0], 10.0f);
            RightTop.transform.localPosition = Vector3.MoveTowards(RightTop.transform.localPosition, Targets[1], 10.0f);
            LeftBot.transform.localPosition = Vector3.MoveTowards(LeftBot.transform.localPosition, Targets[2], 10.0f);
            RightBot.transform.localPosition = Vector3.MoveTowards(RightBot.transform.localPosition, Targets[3], 10.0f);

            yield return null;
        }

        LeftTop.transform.localPosition = Targets[0];
        RightTop.transform.localPosition = Targets[1];
        LeftBot.transform.localPosition = Targets[2];
        RightBot.transform.localPosition = Targets[3];
    }
}
