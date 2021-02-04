using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    AudioSource audio;
    [SerializeField]
    AudioClip[] BGM;

    int CurrentClipNum;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        CurrentClipNum = -1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBGM(int ClipNum)
    {
        if (CurrentClipNum != ClipNum)
        {
            CurrentClipNum = ClipNum;
            audio.clip = BGM[ClipNum];
            audio.Play();
        }
    }
    public bool IsPlay()
    {
        return audio.isPlaying;
    }
}
