using UnityEngine;

public class CarMovingToUpgradeState : IState
{
    private readonly TrainManager car;

    public CarMovingToUpgradeState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.MovingToStation();
        GameManager.Instance.cam.GetComponent<CameraFollow>().Offset = new Vector3(0, 4, 0);
    }

    public void UpdateState()
    {
        car.CarMoving();
    }

    public void ExitState()
    {
        car.MaxSpeed = car.MaxSpeed / 1.5f;
        //GameManager.Instance.cam.GetComponent<CameraFollow>().Offset = new Vector3(9, 4, 0);
    }
}
