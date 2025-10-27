using UnityEngine;

public class GetOffCarSate : IState
{
    private readonly RepairerControl repairer;

    public GetOffCarSate(RepairerControl repairer)
    {
        this.repairer = repairer;
    }

    public void EnterState()
    {
        repairer.GetOffTheCar();
    }

    public void UpdateState()
    {
        repairer.transform.position = Vector2.MoveTowards(repairer.transform.position, repairer.posWaving, 10 * Time.deltaTime);
        float distance = Vector3.Distance(repairer.transform.position, repairer.posWaving);
        if (distance < 0.5f)
        {
            repairer.ChangeState(new WavingState(repairer));
        }
    }

    public void ExitState()
    {

    }
}
