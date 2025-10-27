using UnityEngine;

public class CardUgradingState : IState
{
    private readonly TrainManager car;

    public CardUgradingState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.Upgrading();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
