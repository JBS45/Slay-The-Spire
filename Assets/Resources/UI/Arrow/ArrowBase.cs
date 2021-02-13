using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowBase : MonoBehaviour
{
    [SerializeField]
    GameObject PointRes;
    [SerializeField]
    GameObject ArrowRes;

    GameObject[] Points;

    [SerializeField]
    int NumOfPoints;

    Transform MainCanvas;

    bool IsUpdate;
    private void Awake()
    {
        MainCanvas = GameObject.Find("Canvas").transform;
        Points = new GameObject[NumOfPoints];

        for(int i = 0; i < NumOfPoints-1; ++i)
        {
            Points[i] = Instantiate(PointRes, MainCanvas);
            Points[i].transform.localPosition = Vector3.zero;
        }

        Points[NumOfPoints-1] = Instantiate(ArrowRes, MainCanvas);
        Points[NumOfPoints-1].transform.localPosition = Vector3.zero;
        SetArrowActive(false);

        IsUpdate = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsUpdate)
        {
            DrawTrajectory();
        }
    }

    public void SetArrowActive(bool IsUse)
    {
        foreach(var point in Points)
        {
            point.SetActive(IsUse);
        }
        IsUpdate = IsUse;
    }

    public void ChangeArrowColor(bool HasTarget)
    {
        if (HasTarget)
        {
            foreach (var point in Points)
            {
                point.GetComponentInChildren<Image>().color = new Color(1.0f,0.5f,0.5f);
            }
        }
        else
        {
            foreach (var point in Points)
            {
                point.GetComponentInChildren<Image>().color = Color.white;
            }
        }
    }

    void DrawTrajectory()
    {
        Vector2 Target = GameUtill.CoordCanvasMousePosition(MainCanvas.gameObject);
        Vector2 Origin = transform.localPosition;
        Vector2 midPoint = new Vector2(Origin.x, Target.y);

        for(int i=0;i<NumOfPoints;++i)
        {
            float tmp = (float)i / (float)(NumOfPoints-1);
            Points[i].transform.localPosition = GameUtill.DrawBezierCurve(Origin, midPoint, Target, tmp);
        }
        for(int i = 1; i < NumOfPoints; ++i)
        {
            Points[i].transform.localRotation = Quaternion.FromToRotation(new Vector3(0, 1, 0), Points[i].transform.localPosition - Points[i - 1].transform.localPosition);
        }
    }
}
