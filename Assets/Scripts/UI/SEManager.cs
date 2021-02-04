using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattelSEType
{
    Win
}

public class SEManager : MonoBehaviour
{
    AudioSource audio;
    [SerializeField]
    AudioClip[] UISE;

    [SerializeField]
    AudioClip[] BattleSE;

    

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
        audio.clip = UISE[num];
        audio.Play();
    }
    public void PlaySE(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
    public void BattleSEPlay(BattelSEType type)
    {
        switch (type)
        {
            case BattelSEType.Win:
                audio.clip = BattleSE[0];
                audio.Play();
                break;
        }
    }
}
