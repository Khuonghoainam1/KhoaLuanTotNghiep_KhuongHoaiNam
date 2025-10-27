using UnityEngine;

public class EnemyLoseState : IState
{
    private readonly EnemyBase enemy;

    public EnemyLoseState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.Lose();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
