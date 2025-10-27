using UnityEngine;

public class FollowCar : IState
{
    private readonly EnemyBase enemy;

    public FollowCar(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        //enemy.Moving();
        //enemy.speed = enemy.speedBase + Random.Range(0,2f);
        //enemy.GetComponent<CapsuleCollider2D>().enabled = true;
        //enemy.CheckWalkTrue();

        enemy.EnterFollowCar();
    }

    public void UpdateState()
    {
        //enemy.RunUpdate();


        enemy.UpdateFollowCar();
    }

    public void ExitState()
    {
        //enemy.distanceAttack -= enemy.randomDistanceAttack;

        //enemy.CheckWalkFalse();


        enemy.ExitFollowCar();
    }
}
