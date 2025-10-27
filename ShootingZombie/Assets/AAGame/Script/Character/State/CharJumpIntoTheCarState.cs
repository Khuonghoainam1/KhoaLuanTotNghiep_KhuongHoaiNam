using DG.Tweening;
using UnityEngine;

public class CharJumpIntoTheCarState : IState
{
    private readonly CharacterBase character;

    public CharJumpIntoTheCarState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.CharJumpIntoTheCarState();
        character.transform.DOJump(character.posIntherCar.position, 2f, 1, 0.8f).OnComplete(character.OnGetInTheCar);
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
