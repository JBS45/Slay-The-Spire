using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OutlineButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Outline;
    public Image ButtonImage;
    public Color OutlineColor;
    float m_alpha;
    float m_timer;

    // Start is called before the first frame update

    void Awake()
    {
        Outline.color = OutlineColor;
        ButtonImage.color = Color.gray;
        m_alpha = 0.5f;
        m_timer = 0.0f;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    void ChangeColor()
    {

        m_timer += 300.0f*Time.deltaTime * Mathf.Deg2Rad;
        m_alpha = 0.7f+(0.3f*Mathf.Cos(m_timer));
        Outline.color = new Color(OutlineColor.r, OutlineColor.g, OutlineColor.b, m_alpha);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        ButtonImage.color = Color.white;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        ButtonImage.color = Color.gray;
    }
}
