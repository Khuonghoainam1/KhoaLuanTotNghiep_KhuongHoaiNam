using UnityEngine;

public class AttackState : IState
{
    private readonly EnemyBase enemy;

    public AttackState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.EnterAttack();
    }

    public void UpdateState()
    {
        enemy.UpdateAttack();
    }

    public void ExitState()
    {
        enemy.ExitAttack();
    }
}

