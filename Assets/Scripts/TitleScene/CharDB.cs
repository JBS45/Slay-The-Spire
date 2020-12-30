using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDB : MonoBehaviour
{
    static CharDB _instance = null;
    public static CharDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharDB>();
            }
            return _instance;
        }
    }

    CharacterType m_PlayCharacter;
    public CharacterAsset[] m_CharInfo;


    private void Awake()
    {
        m_PlayCharacter = CharacterType.None;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayChar(CharacterType type)
    {
        m_PlayCharacter = type;
    }
    public CharacterAsset GetCharacterAsset()
    {
        switch (m_PlayCharacter)
        {
            case CharacterType.Ironclad:
                return m_CharInfo[0];
            default:
                return null;

        }
    }
}
