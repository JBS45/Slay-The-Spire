using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharacterRenderer : MonoBehaviour
{
    public SkeletonAnimation m_SkelAnim;
    public AnimationReferenceAsset[] m_AnimClips;

    public enum AnimState
    {
        None,Idle,Hit
    }

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
        
    }
    void SetAnimation(AnimationReferenceAsset animClip,bool loop,float timeScale)
    {
        if (animClip.name.Equals(CurrentAnim)) return;

        m_SkelAnim.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;

        CurrentAnim = animClip.name;
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
