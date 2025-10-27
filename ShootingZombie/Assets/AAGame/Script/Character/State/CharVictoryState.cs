public class CharVictoryState : IState
{
    private readonly CharacterBase character;

    public CharVictoryState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.Victory();
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
