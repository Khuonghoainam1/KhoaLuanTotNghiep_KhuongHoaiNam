using UnityEngine;

public class FreeState : MonoBehaviour, IState
{
    private readonly EnemyBase enemy;

    public FreeState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }
    public void EnterState()
    {
        enemy.EnterFree();
    }

    public void UpdateState()
    {
        enemy.UpdateFree();
    }

    public void ExitState()
    {
        enemy.ExitFree();
    }
}
