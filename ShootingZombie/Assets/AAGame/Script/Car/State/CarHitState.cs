using UnityEngine;

public class CarHitState : IState
{
    private readonly TrainManager car;

    public CarHitState(TrainManager car)
    {
        this.car = car;
    }

    public void EnterState()
    {
        car.GetHit();
    }

    public void UpdateState()
    {
        car.timeInstate += Time.deltaTime;
        if(car.timeInstate >= car.characterAnim.GetAnimData(AnimID.hit).duration/1.5f)
        {
            if (GlobalData.gameMode == GameMode.BossWorld)
            {
                car.ChangeState(new FightingBossState(car));
            }
            else
            {
                car.ChangeState(new CarRunningState(car));
            }
        }
        car.RotationGun();
        if (GlobalData.gameMode != GameMode.BossWorld)
        {
            car.CarMoving();
        }
    }

    public void ExitState()
    {
    }
}
