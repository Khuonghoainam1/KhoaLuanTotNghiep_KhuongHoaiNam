using UnityEngine;

public class DroFreeState : MonoBehaviour,IState
{
    private readonly DroneController drone;

    public DroFreeState(DroneController drone)
    {
        this.drone = drone;
    }

    public void EnterState()
    {
        drone.Free();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
