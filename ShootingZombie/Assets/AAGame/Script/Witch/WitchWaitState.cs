using UnityEngine;

public class WitchWaitState : MonoBehaviour, IState
{
    private readonly WitchController witch;

    public WitchWaitState(WitchController witch)
    {
        this.witch = witch;
    }
    public void EnterState()
    {
        witch.EnterWait();
    }

    public void UpdateState()
    {
        witch.UpdateWait();
    }

    public void ExitState()
    {
        witch.ExitWait();
    }
}
