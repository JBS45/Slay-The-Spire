using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public enum WallType
    {
        Type1=0, Type2, Type3
    }

    WallType m_Wall;

    public SpriteRenderer[] WallSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WallTypeChange(WallType Wall)
    {
        if (m_Wall == Wall) return;
        m_Wall = Wall;
        switch (m_Wall)
        {
            case WallType.Type1:
                WallSprite[0].enabled = true;
                WallSprite[1].enabled = false;
                WallSprite[2].enabled = false;
                break;
            case WallType.Type2:
                WallSprite[0].enabled = true;
                WallSprite[1].enabled = true;
                WallSprite[2].enabled = false;
                break;
            case WallType.Type3:
                WallSprite[0].enabled = false;
                WallSprite[1].enabled = false;
                WallSprite[2].enabled = true;
                break;
        }
    }
    public void WallChange()
    {
        int random = Random.Range(0, 3);
        WallTypeChange((WallType)random);
    }
}
