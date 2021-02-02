using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;

public class Merchant : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer Carpet;

    [SerializeField]
    SkeletonAnimation m_Skeleton;

    ShopWindowScript ShopScript;

    Color Origin;

    bool IsEnable = true;
    public bool Enable { get => IsEnable; set => IsEnable = value; }
    bool IsClick = false;
    private void Awake()
    {
        Origin = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        Carpet.color = Origin;
        IsClick = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        Carpet.color = Color.white;
    }
    private void OnMouseExit()
    {
        Carpet.color = Origin;
    }
    private void OnMouseDown()
    {
        if (IsEnable)
        {
            IsClick = true;
        }
    }
    private void OnMouseUp()
    {
        if (IsClick&&IsEnable&&!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool IsHit = Physics2D.GetRayIntersection(ray);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.transform.gameObject.tag == "NPC")
            {
                IsClick = false;
                ShopScript.ShowShop();
            }
            IsEnable = false;
        }
    }
    public void SetShowWindow(ShopWindowScript shop)
    {
        ShopScript = shop;
        ShopScript.SetMerchant(this);
    }
}
