using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharSelectButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterType m_CharButton;
    public Sprite[] ButtonSprites;
    public Color OutlineColor;
    public Image ButtonImage;
    public Image HighlightImage;

    [SerializeField]
    AudioClip SoundEffect;

    Toggle m_Toggle;

    float m_alpha;
    float m_timer;

    bool Selected;

    SEManager m_SEManager;
    // Start is called before the first frame update
    void Awake()
    {
        HighlightImage.color = OutlineColor;
        Selected = false;
        ButtonImage.color = Color.gray;
        m_alpha = 0.5f;
        m_timer = 0.0f;
        m_Toggle = GetComponent<Toggle>();

        GetComponentInChildren<Button>().onClick.AddListener(OnPushButton);

        m_SEManager = GameObject.Find("SEManager").GetComponent<SEManager>();
    }
    void Start()
    {
        CharTypeToImage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Selected)
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {

        m_timer += 300.0f * Time.deltaTime * Mathf.Deg2Rad;
        m_alpha = 0.7f + (0.3f * Mathf.Cos(m_timer));
        HighlightImage.color = new Color(OutlineColor.r, OutlineColor.g, OutlineColor.b, m_alpha);
    }

    void CharTypeToImage()
    {
        switch (m_CharButton)
        {
            case CharacterType.None:
                ButtonImage.sprite = ButtonSprites[0];
                GetComponentInChildren<Button>().enabled = false;
                m_Toggle.enabled = false;
                break;
            case CharacterType.Ironclad:
                ButtonImage.sprite = ButtonSprites[1];
                break;
            case CharacterType.Silent:
                break;
            case CharacterType.Defect:
                break;
            case CharacterType.Watcher:
                break;
        }
    }

    public void OnPushButton()
    {
        m_SEManager.PlaySE(1);

        GetComponentInParent<SelectPanelScript>().CharTypeToBackground(m_CharButton, PlaySound);
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        m_SEManager.PlaySE(0);
        if (m_CharButton != CharacterType.None)
        {
            ButtonImage.color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (m_CharButton != CharacterType.None)
        {
            ButtonImage.color = Color.gray;
        }
    }
    void PlaySound()
    {
        if (SoundEffect != null)
        {
            m_SEManager.PlaySE(SoundEffect);
        }
    }
}
