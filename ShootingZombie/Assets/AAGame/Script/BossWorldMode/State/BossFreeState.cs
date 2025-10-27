using UnityEngine;

public class BossFreeState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossFreeState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterFree();
    }

    public void UpdateState()
    {
        boss.UpdateFree();
    }

    public void ExitState()
    {
        boss.ExitFree();
    }
}
