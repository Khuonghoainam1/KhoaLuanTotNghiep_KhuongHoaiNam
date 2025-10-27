using UnityEngine;

public class CarStartRunState : IState
{
    private readonly TrainManager car;

    public CarStartRunState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.StartRun();
    }

    public void UpdateState()
    {
        car.timeInstate += Time.deltaTime;
        if (car.timeInstate >= car.characterAnim.GetAnimData(AnimID.start).duration)
        {
            car.ChangeState(new CarRunningState(car));
        }
        car.CarMoving();
    }

    public void ExitState()
    {
    }
}
