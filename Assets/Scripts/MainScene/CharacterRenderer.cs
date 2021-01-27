using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum AnimState
{
    None, Idle, Hit
}
public class CharacterRenderer : MonoBehaviour
{
    [Header("Spine Render")]
    [SerializeField]
    SkeletonAnimation m_SkelAnim;
    [SerializeField]
    AnimationReferenceAsset[] m_AnimClips;

    AnimState m_AnimState = AnimState.None;
    string CurrentAnim;
    private void Awake()
    {
        m_AnimState = AnimState.None;
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeAnimState(AnimState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_AnimState== AnimState.Hit&& m_SkelAnim.AnimationState.Tracks.Items[0].IsComplete)
        {
            ChangeAnimState(AnimState.Idle);
        }
    }

    void SetAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        if (animClip.name.Equals(CurrentAnim)) return;

        m_SkelAnim.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        

       CurrentAnim = animClip.name;
    }

    public void SetHitAnimation()
    {
        m_SkelAnim.state.SetAnimation(0, m_AnimClips[1], false);
        m_SkelAnim.state.AddAnimation(0, m_AnimClips[0], true, 0);
    }
    public void ChangeAnimState(AnimState state)
    {
        if (state == m_AnimState) return;
        m_AnimState = state;

        switch (m_AnimState)
        {
            case AnimState.Idle:
                SetAnimation(m_AnimClips[0], true, 1.0f);
                break;
            case AnimState.Hit:
                SetAnimation(m_AnimClips[1], false, 1.0f);
                break;
        }
    }
}
