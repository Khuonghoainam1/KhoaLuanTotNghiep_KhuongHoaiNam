using UnityEngine;

public class BossAttackState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossAttackState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterAttack();
    }

    public void UpdateState()
    {
        boss.UpdateAttack();
    }

    public void ExitState()
    {
        boss.ExitAttack();
    }
}
