using AA_Game;
using System.Collections;
using UnityEngine;
using Yurowm.GameCore;

public class BotController : CharacterBase
{
    public CharacterSkin characterSkin;
    public TypeBot typeBot;
    private void OnEnable()
    {
        ChangeState(new CharStartState(this));
        anim.OnHandleEvent.RemoveListener(OnShootEvent);
        anim.OnHandleEvent.AddListener(OnShootEvent);

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

        GameEvent.OnEquipSkin.RemoveListener(SetSkin);
        GameEvent.OnEquipSkin.AddListener(SetSkin);

        

        //GameEvent.OnEquiepGun.RemoveListener(SetSkin);
        //GameEvent.OnEquiepGun.AddListener(SetSkin);

        SetSkin(null);
    }

    public void OnRevive()
    {
        ChangeState(new CharFindingTargetState(this));
    }

    public void SetSkin(UserBot temp)
    {
        if (typeCharacter == TypeCharacter.Piston)
        {
            characterSkin.SetSkin(User.Instance.UserBot1Using.skin.ToString());
        }
        else if (typeCharacter == TypeCharacter.AK)
        {
            characterSkin.SetSkin(User.Instance.UserBot2Using.skin.ToString());
        }
        else if (typeCharacter == TypeCharacter.Bazoka)
        {
            characterSkin.SetSkin(User.Instance.UserBot3Using.skin.ToString());
        }
    }

    public override void ChangeState(IState newState)
    {
        timeInstate = 0;
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
        gameObject.name = characterState.ToString();
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

    public override void MovingToCar()
    {
        characterState = CharacterState.MovingToCar;
        anim.PlayAnim(AnimID.run, true, 1.5f, false);
    }

    public void OnStartGame()
    {
        ChangeState(new CharFindingTargetState(this));
    }


    public void OnMoveToPlay()
    {
        ChangeState(new CharMovingToCarState(this));
    }


    public override void CharJumpIntoTheCarState()
    {
        characterState = CharacterState.JumpIntoTheCar;
        anim.PlayAnim(AnimID.jump, false, 1, false);
    }

    public override void OnGetInTheCar()
    {
        ChangeState(new CharFindingTargetState(this));
    }

    public override void FindingTarget()
    {
        target = null;
        characterState = CharacterState.FindingTarget;
        anim.PlayAnim(AnimID.idle, true, 1, false);
        StartCoroutine(Finding());
    }

    public override void Shoot()
    {
        characterState = CharacterState.Shoot;
        anim.PlayAnim(AnimID.shoot, true, 1, false);


        if (target.typeEnemy == TypeEnemy.Boss_World)
        {
            randomPosToShoot = Random.Range(2, 7);
        }
        else
        {
            randomPosToShoot = Random.Range(0, 1);
        }

        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            if (target.transform.position.y > 2)
            {
                randomPosToShoot = Random.Range(-1f, 0);
            }
        }
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
        characterState = CharacterState.Victory;
        anim.PlayAnim(AnimID.victory, true, 1, false);
    }

    public override void Reload()
    {
        characterState = CharacterState.Reload;
        anim.PlayAnim(AnimID.idle, true, 1, false);
    }

    public void OnShootEvent(string eventName)
    {

        if (eventName == "shoot")
        {
            if (typeCharacter == TypeCharacter.Bazoka)
            {
                RocketController rocket = ContentPoolable.Emit(ItemID.rocket_1) as RocketController;
                rocket.transform.position = rocketPos.position;
                Vector3 dir1 = -this.transform.position + target.transform.position;
                dir1 = CaculateDir();
                rocket.transform.right = dir1;
                rocket.force = CaculateForce();
                rocket.SetDamage(this.typeBot);
                rocket.AddForce();
                if (User.Instance.GameMode == GameMode.CollectFuel)
                {
                    rocket.transform.parent = GameManager.Instance.trainManager.transform;
                }

                Item flash1 = ContentPoolable.Emit(ItemID.bazoka_flash_1) as Item;
                flash1.transform.parent = this.transform;
                flash1.transform.position = new Vector3(rocketPos.position.x - 1f, rocketPos.position.y, rocketPos.position.z);
                AudioManager.instance.Play("bazoka");
                return;

            }

            //else if (typeCharacter == TypeCharacter.AK || typeCharacter == TypeCharacter.Piston)
            //{
            //    BulletBot bullet = ContentPoolable.Emit(ItemID.bullet_bot_1) as BulletBot;
            //    bullet.transform.position = rocketPos.position;
            //    Vector3 dir = -this.rocketPos.position + new Vector3(target.transform.position.x, target.transform.position.y + 0.5f + randomPosToShoot, target.transform.position.z);
            //    if (target.transform.position.y > 1)
            //    {
            //        dir = -this.rocketPos.position + new Vector3(target.transform.position.x, target.transform.position.y + 3f + randomPosToShoot, target.transform.position.z);
            //    }
            //    bullet.transform.right = dir;
            //    bullet.AddForce();


            //    if (User.Instance.GameMode == GameMode.CollectFuel)
            //    {
            //        bullet.transform.parent = GameManager.Instance.trainManager.transform;
            //    }

            //    Item flash = ContentPoolable.Emit(ItemID.gun_flash_2) as Item;
            //    flash.transform.parent = this.transform;
            //    flash.transform.position = rocketPos.position;
            //}


            BulletBot bullet = ContentPoolable.Emit(ItemID.bullet_bot_1) as BulletBot;
            bullet.typeBot = this.typeBot;
            bullet.transform.position = rocketPos.position;
            Vector3 dir = -this.rocketPos.position + new Vector3(target.transform.position.x, target.transform.position.y + 0.5f + randomPosToShoot, target.transform.position.z);
            if (target.transform.position.y > 1)
            {
                dir = -this.rocketPos.position + new Vector3(target.transform.position.x, target.transform.position.y + 3f + randomPosToShoot, target.transform.position.z);
            }
            bullet.transform.right = dir;
            bullet.SetDamage(this.typeBot);
            bullet.AddForce();


            if (GlobalData.gameMode == GameMode.CollectFuel)
            {
                bullet.transform.parent = GameManager.Instance.trainManager.transform;
            }

            Item flash = ContentPoolable.Emit(ItemID.gun_flash_2) as Item;
            flash.transform.parent = this.transform;
            flash.transform.position = rocketPos.position;
        }
    }

    /// <summary>
    /// Caculate dir for Bazoka bot
    /// </summary>
    /// <returns></returns>
    public Vector3 CaculateDir()
    {
        Vector3 dir;
        //if (User.Instance.GameMode == GameMode.CollectFuel)
        //{
        //    dir = new Vector3(30, 10, 0);
        //}
        //else
        //{
        //    if (target.typeEnemy == TypeEnemy.Enemy_Fly)
        //    {
        //        dir = new Vector3(15, 10, 0);
        //    }
        //    else
        //    {
        //        dir = new Vector3(40, 0, 0);
        //    }
        //}


        if (target.typeEnemy == TypeEnemy.Enemy_Fly)
        {
            dir = new Vector3(15, 10, 0);
        }
        else
        {
            dir = new Vector3(40, 0, 0);
        }
        return dir;
    }

    /// <summary>
    /// CACULATE FOR BAZOKA
    /// </summary>
    /// <returns></returns>
    public float CaculateForce()
    {
        Transform posEnemy = null;
        float speed = 0;
        if (GlobalData.gameMode == GameMode.BossWorld)
        {
            posEnemy = target.transform;
            speed = -50;
        }
        else if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            posEnemy = target.transform;
            speed = 30;
        }
        else
        {
            posEnemy = target.transform;
            speed = target.speed;
        }

        float force = 40;
        float dis = Vector3.Distance(this.transform.position, posEnemy.position);

        //mode collect
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            float deltaY = transform.localPosition.y - posEnemy.position.y;
            force = ((dis / 5) * 35) + (5 * (2 - deltaY)) + speed * 2;
            if (deltaY < 0)
            {
                force += deltaY * 2;
            }
        }
        else
        {
            float deltaY = transform.localPosition.y - posEnemy.position.y;
            force = ((dis / 8.5f) * 35) + (5 * (2 - deltaY)) - speed * 2;
            if (deltaY < 0)
            {
                force -= deltaY * 2;
            }
        }

        return force;
    }


    IEnumerator Finding()
    {
        if (target != null && target.enemyState != EnemyState.Die && target.enemyState != EnemyState.Spawn)
        {
            ChangeState(new CharShootState(this));
            yield break;
        }

        while (target == null || target.enemyState == EnemyState.Die || target.enemyState == EnemyState.Spawn)
        {
            if (GameManager.Instance.enemiesCurrentAmount > 0)
            {
                foreach (EnemyBase enemy in GameManager.Instance.listEnemy)
                {
                    float dis = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (dis < botDetectRange && enemy.enemyState != EnemyState.Die && enemy.enemyState != EnemyState.Spawn && enemy.transform.position.y < 8f)
                    {
                        this.target = enemy;
                        if (typeCharacter == TypeCharacter.Bazoka && GlobalData.gameMode == GameMode.Normal)
                        {
                            if (dis < (botDetectRange - 16))
                            {
                                this.target = null;
                                continue;
                            }
                        }
                        ChangeState(new CharShootState(this));
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
