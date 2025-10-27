using UnityEngine;

public class BossGetHitState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossGetHitState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterGetHit();
    }

    public void UpdateState()
    {
        boss.UpdateGetHit();
    }

    public void ExitState()
    {
        boss.ExitGetHit();
    }
}
