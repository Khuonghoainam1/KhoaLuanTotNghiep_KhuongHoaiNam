public class CharStunState : IState
{
    private readonly CharacterBase character;

    public CharStunState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.EnterStun();
    }

    public void UpdateState()
    {
        character.UpdateStun();
    }

    public void ExitState()
    {
        character.ExitStun();
    }
}
