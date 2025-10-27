using UnityEngine;

public class BossAttack50HealthState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossAttack50HealthState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterAttack50();
    }

    public void UpdateState()
    {
        boss.UpdateAttack50();
    }

    public void ExitState()
    {
        boss.ExitAttack50();
    }
}
