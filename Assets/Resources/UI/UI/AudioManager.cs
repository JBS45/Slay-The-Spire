using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundObserver
{
    void SoundUpdate(float volume);
}

public class AudioManager : MonoBehaviour
{
    static float _SEVolume=0.5f;
    public static float SEVolume { get => _SEVolume; }
    static float _BGMVolume=0.5f;
    public static float BGMVolume { get => _BGMVolume; }

    static List<ISoundObserver> _Observers = new List<ISoundObserver>();
    public static List<ISoundObserver> Observers { get => _Observers; }

    static List<ISoundObserver> _BGMObservers = new List<ISoundObserver>();
    public static List<ISoundObserver> OBGMbservers { get => _BGMObservers; }


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.GetInt("Save") != 0)
        {
            _SEVolume = PlayerPrefs.GetFloat("SE");
            _BGMVolume = PlayerPrefs.GetFloat("BGM");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetSEVolume(float volume)
    {
        _SEVolume = volume;
        PlayerPrefs.SetFloat("SE", volume);
        Notify();
    }
    public static void SetBGMVolume(float volume)
    {
        _BGMVolume = volume;
        PlayerPrefs.SetFloat("BGM", volume);
        Notify();
    }
    public static void Attach(ISoundObserver observer)
    {
        _Observers.Add(observer);
        Notify();
    }
    public static void BGMAttach(ISoundObserver observer)
    {
        _BGMObservers.Add(observer);
        Notify();
    }
    static void Notify()
    {
        _Observers.RemoveAll(item => item == null);
        _BGMObservers.RemoveAll(item => item == null);
        foreach (var item in _Observers)
        {
            item.SoundUpdate(_SEVolume);
        }
        foreach (var item in _BGMObservers)
        {
            item.SoundUpdate(_BGMVolume);
        }
    }
    static public void Clear()
    {
        _Observers.Clear();
        _BGMObservers.Clear();
    }


}
