using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapIconScript : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    Button IconButton;
    [SerializeField]
    Image Icon;
    [SerializeField]
    Image Outline;
    [SerializeField]
    Image Circle;

    [Header("Spirte Image"), Space(10.0f)]
    [SerializeField]
    Sprite[] StateImages;
    [SerializeField]
    Sprite[] OutlineImages;

    [SerializeField]
    Sprite[] BossImages;
    [SerializeField]
    Sprite[] BossOutlineImages;

    Animator Anim;


    MapNodeType NodeType;
    BossType Boss;
    bool IsCanGo;

    IEnumerator e;

    void Awake()
    {
        IsCanGo = false;
        Outline.enabled = false;
        Circle.enabled = false;
        Anim = GetComponentInChildren<Animator>();

        Icon.color = Color.black;
        Outline.color = Color.white;
        e = IsCanGoButtonAction();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCanGo && e != null)
        {
            e.MoveNext();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsCanGo)
        {
            Outline.enabled = true;
        }
        transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        Icon.color = Color.black;
        Outline.color = Color.white;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Outline.enabled = false;
        transform.localScale = Vector3.one;
        Icon.color = Color.black;
        Outline.color = Color.white;
    }
    public void SetNodeType(MapNodeType type)
    {
        NodeType = type;

        switch (type)
        {
            case MapNodeType.Treasure:
                Icon.sprite = StateImages[0];
                Outline.sprite = OutlineImages[0];
                break;
            case MapNodeType.Mistery:
                Icon.sprite = StateImages[1];
                Outline.sprite = OutlineImages[1];
                break;
            case MapNodeType.Rest:
                Icon.sprite = StateImages[2];
                Outline.sprite = OutlineImages[2];
                break;
            case MapNodeType.Merchant:
                Icon.sprite = StateImages[3];
                Outline.sprite = OutlineImages[3];
                break;
            case MapNodeType.Monster:
                Icon.sprite = StateImages[4];
                Outline.sprite = OutlineImages[4];
                break;
            case MapNodeType.Elite:
                Icon.sprite = StateImages[5];
                Outline.sprite = OutlineImages[5];
                break;
            case MapNodeType.None:
                Icon.enabled = false;
                Outline.enabled = false;
                break;
        }
    }
    public void SetBossType(BossType type)
    {
        Boss = type;
        switch (Boss)
        {
            case BossType.BossSlime:
                Icon.sprite = BossImages[2];
                Outline.sprite = BossImages[2];
                break;
            /*case BossType.Guardian:
                Icon.sprite=BossImages[0];
                Outline.sprite = BossImages[0];
                break;
            case BossType.Hexaghost:
                Icon.sprite = BossImages[1];
                Outline.sprite = BossImages[1];
                break;*/
        }
        Icon.SetNativeSize();
        Outline.SetNativeSize();
    }
    IEnumerator IsCanGoButtonAction()
    {

        float size;

        float delta=0;
        float dir;

        while (true)
        {
            delta += Time.deltaTime;
            dir = 0.3f * Mathf.Cos(Mathf.Deg2Rad*delta*200.0f);
            size = 1.3f + dir;
            transform.localScale = new Vector3(size, size, size);

            yield return null;
        }
        
    }
    public void ButtonOff()
    {
        IconButton.onClick.RemoveAllListeners();
        IconButton.enabled = false;
        IsCanGo = false;
    }

    public void ButtonOn(Delvoid del)
    {
        IconButton.enabled = true;
        IsCanGo = true;
        IconButton.onClick.AddListener(
            () => { del();
                   Circle.enabled = true;
                   Anim.SetTrigger("Check");
                    });
        
    }
}
