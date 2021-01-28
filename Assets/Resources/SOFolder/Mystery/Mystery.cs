using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MysteryAsset")]
public class Mystery : ScriptableObject
{
    public string Key;
    public string Name;

    [PreviewSprite]
    public Sprite sprite;

    [TextArea(1,6)]
    public string Description;

    public string NextKey;
    public List<MysteryButton> Button;

}
[System.Serializable]
public class MysteryButton
{
    public string ButtonString;
    public List<string> ButtonEventKey;
}
