using UnityEngine;

public class WitchAttackState : MonoBehaviour, IState
{
    private readonly WitchController witch;

    public WitchAttackState(WitchController witch)
    {
        this.witch = witch;
    }
    public void EnterState()
    {
        witch.EnterAttack();
    }

    public void UpdateState()
    {
        witch.UpdateAttack();
    }

    public void ExitState()
    {
        witch.ExitAttack();
    }
}
