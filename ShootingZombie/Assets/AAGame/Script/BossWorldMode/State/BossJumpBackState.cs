using UnityEngine;

public class BossJumpBackState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossJumpBackState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterJumpBack();
    }

    public void UpdateState()
    {
        boss.UpdateJumpBack();
    }

    public void ExitState()
    {
        boss.ExitJumpBack();
    }
}
