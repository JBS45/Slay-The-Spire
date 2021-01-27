using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;


public class MonsterRenderer : MonoBehaviour
{
    [Header("Spine Render")]
    [SerializeField]
    SkeletonAnimation m_SkelAnim;
    public SkeletonAnimation Skeleton { get => m_SkelAnim; }
    [SerializeField]
    AnimationReferenceAsset[] m_AnimClips;
    public AnimationReferenceAsset[] AnimClips { get => m_AnimClips; }

    List<TrackEntry> entry;
    public List<TrackEntry> AnimEntry { get => entry; }

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

    public TrackEntry SetAnimation(AnimationReferenceAsset animClip, bool loop,float timeScale)
    {
        TrackEntry tmp = m_SkelAnim.state.SetAnimation(0, animClip, loop);

        tmp.timeScale = timeScale;
        CurrentAnim = animClip.name;

        return tmp;
    }
    public void AddAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        if (animClip.name.Equals(CurrentAnim)) return;

        m_SkelAnim.state.AddAnimation(0, animClip, loop, 0).timeScale = timeScale;

        CurrentAnim = animClip.name;
    }

}
