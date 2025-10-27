using UnityEngine;

public class GetHitState : IState
{
    private readonly EnemyBase enemy;

    public GetHitState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.timeInState = 0;
        //enemy.GetHit();
       // enemy.CheckWalkTrue();
    }

    public void UpdateState()
    {
       // enemy.GetHitUpdate();
    }

    public void ExitState()
    {
       // enemy.CheckWalkFalse();
    }
}
