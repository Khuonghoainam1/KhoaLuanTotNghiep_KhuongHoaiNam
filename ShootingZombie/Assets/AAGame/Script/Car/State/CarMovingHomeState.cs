using UnityEngine;

public class CarMovingHomeState : IState
{
    private readonly TrainManager car;

    public CarMovingHomeState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.MovingHome();
        GameManager.Instance.cam.GetComponent<CameraFollow>().Offset = new Vector3(0, 4, 0);
    }

    public void UpdateState()
    {
        car.CarMoving();
    }

    public void ExitState()
    {
        car.MaxSpeed = car.MaxSpeed / 1.5f;
    }
}
