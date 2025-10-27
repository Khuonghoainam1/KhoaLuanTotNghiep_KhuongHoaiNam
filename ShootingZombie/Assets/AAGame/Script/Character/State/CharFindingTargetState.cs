using UnityEngine;

public class CharFindingTargetState : IState
{
    private readonly CharacterBase character;

    public CharFindingTargetState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.FindingTarget();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {

    }
}
