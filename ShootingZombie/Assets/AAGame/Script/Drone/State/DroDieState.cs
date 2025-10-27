using UnityEngine;

public class DroDieState : MonoBehaviour,IState
{
    private readonly DroneController drone;

    public DroDieState(DroneController drone)
    {
        this.drone = drone;
    }

    public void EnterState()
    {
        drone.Die();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
        drone.ExitDie();
    }
}
