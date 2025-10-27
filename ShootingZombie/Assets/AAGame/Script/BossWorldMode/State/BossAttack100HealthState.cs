using UnityEngine;

public class BossAttack100HealthState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossAttack100HealthState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterAttack100();
    }

    public void UpdateState()
    {
        boss.UpdateAttack100();
    }

    public void ExitState()
    {
        boss.ExitAttack100();
    }
}
