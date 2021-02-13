using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattelSEType
{
    Win,PlayerTurn,Buff,Debuff,Gold,CardSelect,CashRegister,Block,BlockBreak,GainDefence,Heal,Extinction,Enchant
}

public class SEManager : MonoBehaviour,ISoundObserver
{
    AudioSource audio;
    [SerializeField]
    AudioClip[] UISE;

    [SerializeField]
    AudioClip[] BattleSE;

    

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        AudioManager.Attach(this);
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
            case BattelSEType.PlayerTurn:
                audio.clip = BattleSE[1];
                audio.Play();
                break;
            case BattelSEType.Buff:
                audio.clip = BattleSE[2];
                audio.Play();
                break;
            case BattelSEType.Debuff:
                audio.clip = BattleSE[3];
                audio.Play();
                break;
            case BattelSEType.Gold:
                audio.clip = BattleSE[4];
                audio.Play();
                break;
            case BattelSEType.CardSelect:
                audio.clip = BattleSE[5];
                audio.Play();
                break;
            case BattelSEType.CashRegister:
                audio.clip = BattleSE[6];
                audio.Play();
                break;
            case BattelSEType.Block:
                audio.clip = BattleSE[7];
                audio.Play();
                break;
            case BattelSEType.BlockBreak:
                audio.clip = BattleSE[8];
                audio.Play();
                break;
            case BattelSEType.GainDefence:
                audio.clip = BattleSE[9];
                audio.Play();
                break;
            case BattelSEType.Heal:
                audio.clip = BattleSE[10];
                audio.Play();
                break;
            case BattelSEType.Extinction:
                audio.clip = BattleSE[11];
                audio.Play();
                break;
            case BattelSEType.Enchant:
                audio.clip = BattleSE[12];
                audio.Play();
                break;
        }
    }
    public void SoundUpdate(float volume)
    {
        audio.volume = volume;
    }
}
