using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    AudioSource audio;
    [SerializeField]
    AudioClip[] SE;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySE(int num)
    {
        audio.clip = SE[num];
        audio.Play();
    }

}
