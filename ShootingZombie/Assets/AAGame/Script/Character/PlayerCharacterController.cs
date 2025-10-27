using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : CharacterBase
{
    public CharacterSkin characterSkin;

    private void OnEnable()
    {
        currentState = new CharStartState(this);
        currentState.EnterState();
        GameEvent.OnPlayerWin.RemoveListener(OnWin);
        GameEvent.OnPlayerWin.AddListener(OnWin);
        GameEvent.OnPlayerLose.RemoveListener(OnLose);
        GameEvent.OnPlayerLose.AddListener(OnLose);


        GameEvent.OnStartGame.RemoveListener(OnStartGame);
        GameEvent.OnStartGame.AddListener(OnStartGame);

        GameEvent.OnMoveToPlay.RemoveListener(OnMoveToPlay);
        GameEvent.OnMoveToPlay.AddListener(OnMoveToPlay);

        GameEvent.OnReviveGame.RemoveListener(OnRevive);
        GameEvent.OnReviveGame.AddListener(OnRevive);

       // GameEvent.OnUnlockNewSkin.RemoveAllListeners();
       // GameEvent.OnUnlockNewSkin.AddListener(SetNewSkin);


        GameEvent.OnEquipSkin.RemoveListener(SetNewSkin);
        GameEvent.OnEquipSkin.AddListener(SetNewSkin);

        GameEvent.OnShooting.RemoveListener(Shoot);
        GameEvent.OnShooting.AddListener(Shoot);

        GameEvent.OnEndShooting.RemoveListener(Free);
        GameEvent.OnEndShooting.AddListener(Free);

        SetNewSkin(null);
    }


    public void SetNewSkin(UserBot temp)
    {
        characterSkin.SetSkin(User.Instance.UserPlayerUsing.skin.ToString());
    }

    public void OnRevive()
    {
        ChangeState(new CharFreeState(this));
    }

    public override void ChangeState(IState newState)
    {
        timeInstate = 0;
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public override void Spawn()
    {

    }

    public override void OnWin()
    {
        ChangeState(new CharVictoryState(this));
    }

    public override void OnLose()
    {
        ChangeState(new CharDieState(this));
    }

    public override void Free()
    {
        characterState = CharacterState.Free;
        anim.PlayAnim(AnimID.idle, true, 1, false);
    }

    public override void CharStart()
    {
        characterState = CharacterState.Start;
        anim.PlayAnim(AnimID.victory, true, 1, false);
    }

    public override void FindingTarget()
    {

    }

    public override void MovingToCar()
    {
        characterState = CharacterState.MovingToCar;
        anim.PlayAnim(AnimID.run, true, 1.5f, false);
    }

    public override void OnGetInTheCar()
    {
        ChangeState(new CharFreeState(this));
    }

    public override void CharJumpIntoTheCarState()
    {
        characterState = CharacterState.JumpIntoTheCar;
        anim.PlayAnim(AnimID.jump, false, 1, false);
    }

    public void OnStartGame()
    {
        ChangeState(new CharFreeState(this));
    }


    public void OnMoveToPlay()
    {
        ChangeState(new CharMovingToCarState(this));
    }



    public override void Shoot()
    {
        anim.PlayAnim(AnimID.shoot, true, 1, false);
    }

    public override void Hit()
    {

    }
    public override void Die()
    {
        characterState = CharacterState.Die;
        anim.PlayAnim(AnimID.die, false, 1, false);
    }

    public override void Victory()
    {
        anim.PlayAnim(AnimID.victory, true, 1, false);
    }

    public override void Reload()
    {
        characterState = CharacterState.Reload;
        anim.PlayAnim(AnimID.idle, true, 1, false);
    }

    public override void EnterStun()
    {
        base.EnterStun();
        GameManager.Instance.trainManager.playerGun.GunFlashOff();
        GameManager.Instance.trainManager.isStun = true;
    }

    public override void ExitStun()
    {
        base.ExitStun();
        GameManager.Instance.trainManager.playerGun.GunFlashOn();
        GameManager.Instance.trainManager.isStun = false;
    }
}
