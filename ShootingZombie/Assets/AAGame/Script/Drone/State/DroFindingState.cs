using UnityEngine;

public class DroFindingState : MonoBehaviour,IState
{
    private readonly DroneController drone;

    public DroFindingState(DroneController drone)
    {
        this.drone = drone;
    }

    public void EnterState()
    {
        drone.FindingZombie();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}
