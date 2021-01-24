using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    AudioSource audio;
    [SerializeField]
    AudioClip BGM;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = BGM;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBGM()
    {
        audio.Play();
    }
    public bool IsPlay()
    {
        return audio.isPlaying;
    }
}
