using Spine.Unity;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    public SkeletonAnimation anim;

    private void Start()
    {
        anim = GetComponent<SkeletonAnimation>();
        Attack1Anim();
    }

    public void Attack1Anim()
    {
        anim.AnimationState.SetAnimation(1, "attack", true);
    }

    public void Attack2Anim()
    {
        anim.AnimationState.SetAnimation(1, "attack2", true);
    }

    public void DieAnim()
    {
        anim.AnimationState.SetAnimation(1, "die", true);
    }

    public void HitAnim()
    {
        anim.AnimationState.SetAnimation(1, "hit", true);
    }

    public void IdleAnim()
    {
        anim.AnimationState.SetAnimation(1, "idle", true);
    }

    public void RunAnim()
    {
        anim.AnimationState.SetAnimation(1, "run", true);
    }

    public void SpawnAnim()
    {
        anim.AnimationState.SetAnimation(1, "spawn", true);
    }
}
