using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class MonsterRenderer : MonoBehaviour
{
    [Header("Spine Render")]
    [SerializeField]
    SkeletonAnimation m_SkelAnim;
    [SerializeField]
    AnimationReferenceAsset[] m_AnimClips;

    string CurrentAnim;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        if (animClip.name.Equals(CurrentAnim)) return;

        m_SkelAnim.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;

        CurrentAnim = animClip.name;
    }

}
