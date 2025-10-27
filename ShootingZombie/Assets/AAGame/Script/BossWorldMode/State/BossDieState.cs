using UnityEngine;

public class BossDieState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossDieState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterDie();
    }

    public void UpdateState()
    {
        boss.UpdateDie();
    }

    public void ExitState()
    {
        boss.ExitDie();
    }
}
