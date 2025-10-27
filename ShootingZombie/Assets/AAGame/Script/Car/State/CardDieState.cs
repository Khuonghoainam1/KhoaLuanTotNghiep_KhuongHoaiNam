using UnityEngine;

public class CardDieState : IState
{
    private readonly TrainManager car;

    public CardDieState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.Die();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
