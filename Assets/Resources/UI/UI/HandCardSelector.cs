using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class HandCardSelector : MonoBehaviour
{
    [SerializeField]
    TMP_Text Text;

    [SerializeField]
    Transform CardPos;
    Transform Hand;

    int Amount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWindow(string WindowText,Transform hand)
    {
        Text.text = WindowText;
        Hand = hand;
    }
}
