public class CharDieState : IState
{
    private readonly CharacterBase character;

    public CharDieState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.Die();
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
