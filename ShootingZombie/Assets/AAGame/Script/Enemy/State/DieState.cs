using UnityEngine;

public class DieState : IState
{
    private readonly EnemyBase enemy;

    public DieState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.timeInstate = 0;
        //enemy.Die();
        //enemy.GetComponent<CapsuleCollider2D>().enabled = false;

        enemy.EnterDie();
    }

    public void UpdateState()
    {
        enemy.UpdateDie();
    }

    public void ExitState()
    {
        //enemy.GetComponent<CapsuleCollider2D>().enabled = true;
        enemy.ExitDie();
    }
}
