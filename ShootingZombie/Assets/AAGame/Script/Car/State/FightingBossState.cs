using UnityEngine;

public class FightingBossState : IState
{
    private readonly TrainManager car;

    public FightingBossState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.EnterFightingBoss();
    }

    public void UpdateState()
    {
        car.RotationGun();
    }

    public void ExitState()
    {

    }
}
