using UnityEngine;

public class DroSpawnState : MonoBehaviour,IState
{
    private readonly DroneController drone;

    public DroSpawnState(DroneController drone)
    {
        this.drone = drone;
    }

    public void EnterState()
    {
        drone.Spawn();
    }

    public void UpdateState()
    {
        drone.timeInState += Time.deltaTime;
        if (drone.timeInState >= 1f)
        {
            drone.ChangeState(new DroFindingState(drone));
        }
    }

    public void ExitState()
    {
    }
}
