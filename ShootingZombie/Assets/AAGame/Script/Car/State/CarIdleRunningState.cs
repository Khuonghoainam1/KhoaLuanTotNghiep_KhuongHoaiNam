using UnityEngine;

public class CarIdleRunningState : IState
{
    private readonly TrainManager car;

    public CarIdleRunningState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.IdleRunning();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
