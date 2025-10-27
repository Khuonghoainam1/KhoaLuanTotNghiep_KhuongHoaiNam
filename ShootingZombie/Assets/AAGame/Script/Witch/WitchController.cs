using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class WitchController : MonoBehaviour
{
    public float timeInState;
    public WitchState witchState;
    protected IState currentState;
    public CharacterAnim anim;
    public EnemyBase target;
    private float botDetectRange = 30;
    public GameObject posAimZombie;
    public GameObject[] lazer;
    public float liveTime;
    public List<Vector3> posList = new List<Vector3>();
    public bool isReving;
    private bool isMovingToDown;

    private void OnEnable()
    {
        GameEvent.OnPlayerLose.RemoveListener(OnPlayerDie);
        GameEvent.OnPlayerLose.AddListener(OnPlayerDie);

        GameEvent.OnReviveGame.RemoveListener(OnPlayerRevive);
        GameEvent.OnReviveGame.AddListener(OnPlayerRevive);

        GameEvent.OnPlayerWin.RemoveListener(OnPlayerWin);
        GameEvent.OnPlayerWin.AddListener(OnPlayerWin);

        ChangeState(new WitchAppearState(this));
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }

        if (GameManager.Instance.gameState == GameState.Playing)
        {
            liveTime += Time.deltaTime;
            if (liveTime > 36000)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void ChangeState(IState newState)
    {
        timeInState = 0;
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    private void OnDestroy()
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
    }


    public void OnPlayerWin()
    {
        ChangeState(new WitchWaitState(this));
    }



    //==========SPAWN===========//
    public void EnterSpawn()
    {
        witchState = WitchState.Appear;
        anim.PlayAnim(AnimID.spawn, false, 1, false);
        posList.Clear();

        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 pos1 = new Vector3(9.25f,0.93f,0);
            posList.Add(pos1);
            Vector3 pos2 = new Vector3(1.33f,0.93f,0);
            posList.Add(pos2);
            Vector3 pos3 = new Vector3(-3.31f,-1.36f,0);
            posList.Add(pos3);
        }
        else
        {
            Vector3 pos1 = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            posList.Add(pos1);
            Vector3 pos2 = new Vector3(transform.localPosition.x - 4, transform.localPosition.y, transform.localPosition.z);
            posList.Add(pos2);
            Vector3 pos3 = new Vector3(transform.localPosition.x + 3, transform.localPosition.y + 1, transform.localPosition.z);
            posList.Add(pos3);
        }

        StartCoroutine(MoveRandom());
    }

    public void UpdateSpawn()
    {
        timeInState += Time.deltaTime;
        if (timeInState >= anim.GetAnimData(AnimID.spawn, 1).duration)
        {
            ChangeState(new WitchFindingState(this));
        }
    }

    public void ExitSpawn()
    {

    }


    //==========FINDING===========//
    public void EnterFinding()
    {
        witchState = WitchState.Finding;
        anim.PlayAnim(AnimID.idle, true, 1, false);
        StartCoroutine(Finding());
    }

    IEnumerator Finding()
    {
        if (target != null && target.enemyState != EnemyState.Die && target.enemyState != EnemyState.Spawn)
        {
            ChangeState(new WitchAttackState(this));
            yield break;
        }

        yield return new WaitForSeconds(2f);
        while (isReving == false)
        {
            if (target == null || target.enemyState == EnemyState.Die || target.enemyState == EnemyState.Spawn)
            {
                if (GameManager.Instance.enemiesCurrentAmount > 0)
                {
                    foreach (EnemyBase enemy in GameManager.Instance.listEnemy)
                    {
                        float dis = Vector3.Distance(this.transform.position, enemy.transform.position);
                        if (dis < botDetectRange && enemy.enemyState != EnemyState.Die && enemy.enemyState != EnemyState.Spawn && enemy.transform.position.y < 8f)
                        {
                            this.target = enemy;
                            if(GameManager.Instance.gameState == GameState.Playing)
                            {
                                ChangeState(new WitchAttackState(this));
                                yield break;
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateFinding()
    {
        
    }

    public void ExitFinding()
    {

    }


    //==========ATTACK===========//
    public void EnterAttack()
    {
        isMovingToDown = false;
        witchState = WitchState.Attack;
        anim.PlayAnim(AnimID.shoot, true, 1, false);
        lazer[1].SetActive(true);
        lazer[0].SetActive(true);
        lazer[0].GetComponent<Hovl_DemoLasers>().targetZombie = target;
        lazer[1].GetComponent<Hovl_DemoLasers>().targetZombie = target;
    }

    public void UpdateAttack()
    {
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (target == null || target.enemyState == EnemyState.Die || dis > botDetectRange+2f)
        {
            ChangeState(new WitchFindingState(this));
        }

        if (target != null)
        {
            posAimZombie.transform.position = target.transform.position;
        }

        if(target != null)
        {
            if(dis <= 16f && !isMovingToDown)
            {
                this.transform.DOLocalMove(posList[1], 2f);
                isMovingToDown = true;
            }
        }
    }

    public void ExitAttack()
    {
        lazer[0].SetActive(false);
        lazer[1].SetActive(false);
        isMovingToDown = false;
    }


    //==========WAIT===========//
    public void EnterWait()
    {
        witchState = WitchState.Wait;
        anim.PlayAnim(AnimID.victory, true, 1, false);
        target = null;
    }

    public void UpdateWait()
    {

    }

    public void ExitWait()
    {

    }

    //==========FLY RANDOM===========//
    public void EnterFlyRandom()
    {
        witchState = WitchState.FlyRandom;
        anim.PlayAnim(AnimID.idle, true, 1, false);
    }

    public void UpdateFlyRandom()
    {

    }

    public void ExitFlyRandom()
    {

    }

    /// <summary>
    /// Drone move random when flying
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRandom()
    {
        while (true)
        {
            if (this.witchState == WitchState.Wait)
            {
                yield break;
            }
            yield return new WaitForSeconds(Random.Range(2.1f, 4.1f));
            yield return new WaitUntil(() => witchState != WitchState.Attack);
            this.transform.DOLocalMove(posList.GetRandom(), 2f);
        }
    }


    public void OnPlayerDie()
    {
        ChangeState(new WitchWaitState(this));
        isReving = true;
    }

    public void OnPlayerRevive()
    {
        StartCoroutine(DelayReviveFinding());
    }

    IEnumerator DelayReviveFinding()
    {
        yield return new WaitForSeconds(3f);
        isReving = false;
        ChangeState(new WitchFindingState(this));
        StartCoroutine(MoveRandom());
    }
}

public enum WitchState
{
    Appear,
    Finding,
    Attack,
    Wait,
    FlyRandom,
}
