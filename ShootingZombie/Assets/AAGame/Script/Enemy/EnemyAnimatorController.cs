using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    public Animator anim;

    public void SpawnAnimation()
    {
        anim.SetBool("spawn",true);
        anim.SetBool("run", false);
        anim.SetBool("attack", false);
    }

    public void IdleAnimation()
    {
        anim.SetBool("idle", true);
        anim.SetBool("attack", false);
    }

    public void GetHitAnimation()
    {
        anim.SetBool("getHit", true);
    }

    public void RunAnimation()
    {
        anim.SetBool("run", true);
        anim.SetBool("attack", false);
        anim.SetBool("spawn", false);
    }

    public void AttackAnimation()
    {
        anim.SetBool("attack", true);
        anim.SetBool("run", false);
        anim.SetBool("spawn", false);
    }
}
