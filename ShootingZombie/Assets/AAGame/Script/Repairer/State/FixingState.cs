using UnityEngine;

public class FixingState : IState
{
    private readonly RepairerControl repairer;

    public FixingState(RepairerControl repairer)
    {
        this.repairer = repairer;
    }

    public void EnterState()
    {
        repairer.Fix();
    }

    public void UpdateState()
    {
       
    }

    public void ExitState()
    {
        repairer.effectFix.SetActive(false);
    }
}
