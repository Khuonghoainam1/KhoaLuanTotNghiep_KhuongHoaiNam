public class CharStartState : IState
{
    private readonly CharacterBase character;

    public CharStartState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.CharStart();
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
