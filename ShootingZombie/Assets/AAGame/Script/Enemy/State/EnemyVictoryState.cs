using UnityEngine;

public class EnemyVictoryState : IState
{
    private readonly EnemyBase enemy;


    public EnemyVictoryState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.EnterWin();
    }

    public void UpdateState()
    {
        enemy.UpdateWin();
    }

    public void ExitState()
    {
        enemy.ExitWin();
    }
}
