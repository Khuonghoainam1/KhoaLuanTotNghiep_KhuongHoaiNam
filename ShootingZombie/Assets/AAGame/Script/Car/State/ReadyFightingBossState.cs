using UnityEngine;

public class ReadyFightingBossState : IState
{
    private readonly TrainManager car;

    public ReadyFightingBossState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.ReadyFightingBoss();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
