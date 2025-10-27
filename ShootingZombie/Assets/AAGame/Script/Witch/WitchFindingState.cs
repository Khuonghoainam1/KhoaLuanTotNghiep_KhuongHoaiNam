using UnityEngine;

public class WitchFindingState : MonoBehaviour, IState
{
    private readonly WitchController witch;

    public WitchFindingState(WitchController witch)
    {
        this.witch = witch;
    }
    public void EnterState()
    {
        witch.EnterFinding();
    }

    public void UpdateState()
    {
        witch.UpdateFinding();
    }

    public void ExitState()
    {
        witch.ExitFinding();
    }
}
