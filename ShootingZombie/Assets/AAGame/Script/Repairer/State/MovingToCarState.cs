using UnityEngine;

public class MovingToCarState : IState
{
    private readonly RepairerControl repairer;

    public MovingToCarState(RepairerControl repairer)
    {
        this.repairer = repairer;
    }

    public void EnterState()
    {
        repairer.RepairerMovingToFix();
    }

    public void UpdateState()
    {
       //repairer.transform.position = Vector2.MoveTowards(repairer.transform.position, repairer.posFix.position, 6 * Time.deltaTime);
       // float distance = Vector3.Distance(repairer.transform.position, repairer.posFix.position);
       // if(distance < 0.5f)
       // {
       //     if (repairer.repairerName != NameRepairer.RepairerGreen)
       //     {
       //         repairer.ChangeState(new FixingState(repairer));
       //     }
       //     else
       //     {
       //         repairer.ChangeState(new JumpToPosFix(repairer));
       //     }
       // }
    }

    public void ExitState()
    {

    }
}
