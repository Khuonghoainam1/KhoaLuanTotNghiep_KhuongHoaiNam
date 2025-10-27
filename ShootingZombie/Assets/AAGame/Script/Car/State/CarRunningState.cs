using UnityEngine;

public class CarRunningState : IState
{
    private readonly TrainManager car;

    public CarRunningState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.Running();
    }

    public void UpdateState()
    {
        car.RotationGun();
        car.CarMoving();
    }

    public void ExitState()
    {
    }
}
