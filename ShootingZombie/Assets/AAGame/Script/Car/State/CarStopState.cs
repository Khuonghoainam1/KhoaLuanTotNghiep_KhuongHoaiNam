using UnityEngine;

public class CarStopState : IState
{
    private readonly TrainManager car;

    public CarStopState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.Stop();
        GameManager.Instance.cam.GetComponent<CameraFollow>().Offset = new Vector3(0, 4, 0);
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
        GameManager.Instance.cam.GetComponent<CameraFollow>().Offset = new Vector3(9, 4, 0);
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            GameManager.Instance.cam.GetComponent<CameraFollow>().Offset = new Vector3(15, 4, 0);
        }
    }
}
