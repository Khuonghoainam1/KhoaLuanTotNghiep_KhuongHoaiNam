using UnityEngine;

public class Attack100HealthState : MonoBehaviour, IState
{
    private readonly EnemyBase enemy;

    public Attack100HealthState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }
    public void EnterState()
    {
        enemy.EnterAttack100();
    }

    public void UpdateState()
    {
        enemy.UpdateAttack100();
    }

    public void ExitState()
    {
        enemy.ExitAttack100();
    }
}
