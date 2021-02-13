using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicLayout : MonoBehaviour
{
    GridLayoutGroup Layout;

    List<GameObject> Child;

    private void Awake()
    {
        Layout = GetComponent<GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in Layout.GetComponentsInChildren<CardUIScript>())
        {
            float size = Layout.cellSize.x/ 240;
            item.transform.localScale = new Vector3(size, size, size);
        }
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
