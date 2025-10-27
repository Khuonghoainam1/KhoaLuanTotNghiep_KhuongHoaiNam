using UnityEngine;

public class WitchFlyRandomState : MonoBehaviour, IState
{
    private readonly WitchController witch;

    public WitchFlyRandomState(WitchController witch)
    {
        this.witch = witch;
    }
    public void EnterState()
    {
        witch.EnterFlyRandom();
    }

    public void UpdateState()
    {
        witch.UpdateFlyRandom();
    }

    public void ExitState()
    {
        witch.ExitFlyRandom();
    }
}
