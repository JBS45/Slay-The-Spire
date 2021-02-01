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

    public List<MysteryButton> Button;

}
[System.Serializable]
public class MysteryButton
{
    public Condtion condtion = new Condtion();
    public string NextKey;
    public List<ButtonFunc> ButtonEvents;
}

public enum ConditionType
{
    None,Gold
}
[System.Serializable]
public class Condtion
{
    public ConditionType type = ConditionType.None;
    public float value;
}
[System.Serializable]
public class ButtonFunc
{
    public string ButtonString;
    public string ButtonEventKey;
    public int Value;
}
