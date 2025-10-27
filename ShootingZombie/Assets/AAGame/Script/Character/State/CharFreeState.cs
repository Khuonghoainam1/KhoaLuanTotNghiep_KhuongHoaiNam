public class CharFreeState : IState
{
    private readonly CharacterBase character;

    public CharFreeState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.Free();
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
