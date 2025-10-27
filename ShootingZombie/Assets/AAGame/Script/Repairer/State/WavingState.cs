using UnityEngine;

public class WavingState : IState
{
    private readonly RepairerControl repairer;

    public WavingState(RepairerControl repairer)
    {
        this.repairer = repairer;
    }

    public void EnterState()
    {
        repairer.Waving();
    }

    public void UpdateState()
    {
       
    }

    public void ExitState()
    {

    }
}
