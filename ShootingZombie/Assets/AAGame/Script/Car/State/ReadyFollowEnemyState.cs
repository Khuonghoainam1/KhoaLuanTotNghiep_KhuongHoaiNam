using UnityEngine;

public class ReadyFollowEnemyState : IState
{
    private readonly TrainManager car;

    public ReadyFollowEnemyState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.EnterReadyFollowEnemy();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
