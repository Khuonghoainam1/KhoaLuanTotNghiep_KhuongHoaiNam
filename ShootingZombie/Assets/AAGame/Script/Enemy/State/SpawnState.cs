using UnityEngine;

public class SpawnState : IState
{
    private readonly EnemyBase enemy;


    public SpawnState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.Spawn();
        //enemy.GetComponent<CapsuleCollider2D>().enabled = false;

        enemy.EnterSpawn();
    }

    public void UpdateState()
    {
        //enemy.timeInstate += Time.deltaTime;
        //if(enemy.timeInstate >= enemy.anim.GetAnimData(AnimID.spawn, 1).duration)
        //{
        //    enemy.ChangeState(new RunState(enemy));
        //}

        enemy.UpdateSpawn();
    }

    public void ExitState()
    {
        //enemy.GetComponent<CapsuleCollider2D>().enabled = true;
        enemy.ExitSpawn();
    }
}
