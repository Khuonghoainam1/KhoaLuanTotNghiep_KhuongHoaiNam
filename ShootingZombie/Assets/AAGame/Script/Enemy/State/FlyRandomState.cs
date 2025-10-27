using UnityEngine;

public class FlyRandomState : IState
{
    private readonly EnemyBase enemy;

    public FlyRandomState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.FlyRandom();

        enemy.EnterFlyRandom();
    }

    public void UpdateState()
    {
        //enemy.UpdateFlyRandom();

        enemy.UpdateFlyRandom();
    }

    public void ExitState()
    {
    }
}

