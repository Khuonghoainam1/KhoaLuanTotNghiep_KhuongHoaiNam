using UnityEngine;

public class LeaveTheCarState : IState
{
    private readonly EnemyBase enemy;


    public LeaveTheCarState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.Spawn();
        //enemy.GetComponent<CapsuleCollider2D>().enabled = false;

        enemy.EnterLeaveTheCar();
    }

    public void UpdateState()
    {
        //enemy.timeInstate += Time.deltaTime;
        //if(enemy.timeInstate >= enemy.anim.GetAnimData(AnimID.spawn, 1).duration)
        //{
        //    enemy.ChangeState(new RunState(enemy));
        //}

        enemy.UpdateLeaveTheCar();
    }

    public void ExitState()
    {
        //enemy.GetComponent<CapsuleCollider2D>().enabled = true;
        enemy.ExitLeaveTheCar();
    }
}
