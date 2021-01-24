using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RewardType
{
    Gold,Potion,Card
}
public enum PotionType
{
    None=0,Strength,Agility,Energy,Draw,Fire,Explosion
}
public class RewardButtonScript : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField]
    Sprite Gold;
    [SerializeField]
    Sprite Card;

    [Header("General Info")]
    [SerializeField]
    Button m_Button;
    [SerializeField]
    Image RewardImage;
    [SerializeField]
    TMP_Text Text;

    RewardType m_Type;
    public RewardType Type { get => m_Type; }


    int m_Gold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetReward(RewardType type,int gold, PotionType potion,GameObject reward)
    {
        m_Type = type;
        switch (m_Type)
        {
            case RewardType.Gold:
                RewardImage.sprite = Gold;
                m_Gold = gold;
                Text.text = m_Gold.ToString() + " 골드";
                m_Button.onClick.AddListener(
                    ()=>    {   MainSceneController.Instance.PlayerData.CurrentMoney += gold;
                                MainSceneController.Instance.PlayerData.Notify();
                                Destroy(this.gameObject);
                    });
                break;
            case RewardType.Potion:
                break;
            case RewardType.Card:
                RewardImage.sprite = Card;
                Text.text = "덱에 카드를 추가";
                m_Button.onClick.AddListener(
                    () => {
                        MainSceneController.Instance.UIControl.AddRewardWinodow.SetActive(true);
                        reward.SetActive(false);
                    });
                break;
        }
        
    }

}
