using UnityEngine;

public class FlyToHitCar : IState
{
    private readonly EnemyBase enemy;

    public FlyToHitCar(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.EnterFlyHitCar();
    }

    public void UpdateState()
    {
        enemy.UpdateFlyHitCar();
    }

    public void ExitState()
    {
        enemy.ExitFlyHitCar();
    }
}
