using UnityEngine;

public class BossWinState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossWinState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterWin();
    }

    public void UpdateState()
    {
        boss.UpdateWin();
    }

    public void ExitState()
    {
        boss.ExitWin();
    }
}
