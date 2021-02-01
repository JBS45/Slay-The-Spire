using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    Sprite[] ChestImage;

    [SerializeField]
    SpriteRenderer renderer;

    [SerializeField]
    Color Origin;

    bool IsEnable;
    bool IsClick;

    private void Awake()
    {
        IsEnable = true;
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
        renderer.color = Color.white;
    }
    private void OnMouseExit()
    {
        renderer.color = Origin;
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
        if (IsClick && IsEnable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool IsHit = Physics2D.GetRayIntersection(ray);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.transform.gameObject.tag == "NPC")
            {
                IsClick = false;
                MainSceneController.Instance.UIControl.MakeReward();
            }
            IsEnable = false;
            renderer.sprite = ChestImage[1];
        }
    }
}