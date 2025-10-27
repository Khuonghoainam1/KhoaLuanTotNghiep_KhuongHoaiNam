using UnityEngine;

public class CharMovingToCarState : IState
{
    private readonly CharacterBase character;

    public CharMovingToCarState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.MovingToCar();
    }

    public void UpdateState()
    {
        character.transform.position = Vector2.MoveTowards(character.transform.position, character.posMoveToCar.position, 10 * Time.deltaTime);
        float distance = Vector3.Distance(character.transform.position, character.posMoveToCar.position);
        if (distance < 0.5f)
        {
            character.ChangeState(new CharJumpIntoTheCarState(this.character));
        }
    }

    public void ExitState()
    {

    }
}
