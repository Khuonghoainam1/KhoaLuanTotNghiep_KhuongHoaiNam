using UnityEngine;

public class JumpToPosFix : IState
{
    private readonly RepairerControl repairer;

    public JumpToPosFix(RepairerControl repairer)
    {
        this.repairer = repairer;
    }

    public void EnterState()
    {
    }

    public void UpdateState()
    {
       
    }

    public void ExitState()
    {

    }
}
