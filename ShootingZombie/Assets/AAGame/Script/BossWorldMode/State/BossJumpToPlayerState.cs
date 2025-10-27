using UnityEngine;

public class BossJumpToPlayerState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossJumpToPlayerState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterJumpToPlayer();
    }

    public void UpdateState()
    {
        boss.UpdateJumpToPlayer();
    }

    public void ExitState()
    {
        boss.ExitJumpToPlayer();
    }
}
