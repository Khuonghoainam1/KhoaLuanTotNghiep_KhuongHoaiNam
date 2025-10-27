public class CharShootingBossState : IState
{
    private readonly CharacterBase character;

    public CharShootingBossState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.EnterShootingBoss();
    }

    public void UpdateState()
    {
        character.UpdateShootingBoss();
    }

    public void ExitState()
    {
        character.ExitShootingBoss();
    }
}
