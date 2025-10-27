using UnityEngine;

public class Attack50HealthState : MonoBehaviour, IState
{
    private readonly EnemyBase enemy;

    public Attack50HealthState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }
    public void EnterState()
    {
        enemy.EnterAttack50();
    }

    public void UpdateState()
    {
        enemy.UpdateAttack50();
    }

    public void ExitState()
    {
        enemy.ExitAttack50();
    }
}
