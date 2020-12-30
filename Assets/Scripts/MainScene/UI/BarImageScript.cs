using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BarImageScript : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public enum PointerEnterImageChangeType
    {
        Expand,Rotate
    }
    public Image m_Image;
    public PointerEnterImageChangeType m_Type;
    [SerializeField,Range(-1,1)]
    float ChangeRate;
    Vector2 OriginSize;
    Quaternion OriginRotate;
    void Awake()
    {
        OriginSize = m_Image.rectTransform.localScale;
        OriginRotate = m_Image.rectTransform.localRotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData e)
    {
        switch (m_Type)
        {
            case PointerEnterImageChangeType.Expand:
                m_Image.rectTransform.localScale = Vector3.one * 1.3f;
                break;
            case PointerEnterImageChangeType.Rotate:
                Vector3 tmp = new Vector3(0, 0, 90) * ChangeRate;
                StartCoroutine(Rotate(Quaternion.Euler(tmp)));
                break;
        }
    }
    public void OnPointerExit(PointerEventData e)
    {
        switch (m_Type)
        {
            case PointerEnterImageChangeType.Expand:
                m_Image.rectTransform.localScale = OriginSize;
                break;
            case PointerEnterImageChangeType.Rotate:
                StartCoroutine(Rotate(OriginRotate));
                break;
        }
    }

    IEnumerator Rotate(Quaternion Rotate)
    {
        while(Quaternion.Angle(m_Image.rectTransform.localRotation,Rotate)> 0.1f){
            m_Image.rectTransform.localRotation = Quaternion.RotateTowards(m_Image.rectTransform.localRotation, Rotate,10.0f);
            yield return null;
        }
        m_Image.rectTransform.localRotation = Rotate;
    }
}
