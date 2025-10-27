using UnityEngine;

public class CharShootState : IState
{
    private readonly CharacterBase character;

    public CharShootState(CharacterBase character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        character.Shoot();
    }

    public void UpdateState()
    {
        float dis = Vector3.Distance(character.transform.position, character.target.transform.position);
        if (character.target == null || character.target.enemyState == EnemyState.Die || dis >= (character.botDetectRange + 2))
        {
            character.ChangeState(new CharFindingTargetState(character));
        }

        if (character.typeCharacter == TypeCharacter.Bazoka)
        {
            if (dis < (character.botDetectRange - 13) && GlobalData.gameMode == GameMode.Normal)
            {
                character.ChangeState(new CharFindingTargetState(character));
            }
        }

        //aim car
        if (character.target != null && character.posAimZombie != null)
        {
            if (character.typeCharacter == TypeCharacter.Bazoka)
            {
                if (character.target.typeEnemy == TypeEnemy.Enemy_Fly)
                {
                    character.posAimZombie.transform.position = new Vector3(character.target.transform.position.x, character.target.transform.position.y+8f, character.target.transform.position.z);
                }
                else if(character.target.typeEnemy == TypeEnemy.Boss_World)
                {
                    character.posAimZombie.transform.position = new Vector3(character.target.transform.position.x, character.target.transform.position.y + 10f, character.target.transform.position.z);
                }
                else
                {
                    character.posAimZombie.transform.position = new Vector3(character.target.transform.position.x, character.transform.position.y, character.target.transform.position.z);
                }
            }
            else
            {
                character.posAimZombie.transform.position = character.target.transform.position;
            }
        }
    }

    public void ExitState()
    {

    }
}
