using UnityEngine;

public class CarStartState : IState
{
    private readonly TrainManager car;

    public CarStartState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.CarStart();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
