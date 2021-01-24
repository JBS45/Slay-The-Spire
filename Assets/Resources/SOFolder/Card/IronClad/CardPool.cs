using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardPool")]
public class CardPool : ScriptableObject
{
    [SerializeField]
    List<CardAsset> _BaseCard;
    public List<CardAsset> BaseCard { get => _BaseCard; }
    [SerializeField]
    List<CardAsset> _Card;
    public List<CardAsset> Card { get => _Card; }
}
