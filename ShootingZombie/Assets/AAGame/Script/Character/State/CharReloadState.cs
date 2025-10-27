using UnityEngine;

public class CharReloadState : IState
{
    private readonly CharacterBase character;

    public CharReloadState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.Reload();
    }

    public void UpdateState()
    {
        character.timeInstate += Time.deltaTime;
        if(character.timeInstate >= character.anim.GetAnimData(AnimID.idle).duration)
        {
            character.ChangeState(new CharFindingTargetState(character));
        }
    }

    public void ExitState()
    {

    }
}
