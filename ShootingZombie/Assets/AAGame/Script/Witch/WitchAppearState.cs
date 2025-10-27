using UnityEngine;

public class WitchAppearState : MonoBehaviour, IState
{
    private readonly WitchController witch;

    public WitchAppearState(WitchController witch)
    {
        this.witch = witch;
    }
    public void EnterState()
    {
        witch.EnterSpawn();
    }

    public void UpdateState()
    {
        witch.UpdateSpawn();
    }

    public void ExitState()
    {
        witch.ExitSpawn();
    }
}
