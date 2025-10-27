using UnityEngine;

public class DroAttackState : MonoBehaviour,IState
{
    private readonly DroneController drone;

    public DroAttackState(DroneController drone)
    {
        this.drone = drone;
    }

    public void EnterState()
    {
        drone.Attack();
    }

    public void UpdateState()
    {
        drone.UpdateAttack();
    }

    public void ExitState()
    {
        drone.ExitAttack();
    }
}
