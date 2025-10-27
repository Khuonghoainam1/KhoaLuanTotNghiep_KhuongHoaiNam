using UnityEngine;

public class BossSpawnState : MonoBehaviour, IState
{
    private readonly BossWorldBase boss;

    public BossSpawnState(BossWorldBase boss)
    {
        this.boss = boss;
    }
    public void EnterState()
    {
        boss.EnterSpawn();
    }

    public void UpdateState()
    {
        boss.UpdateSpawn();
    }

    public void ExitState()
    {
        boss.ExitSpawn();
    }
}
