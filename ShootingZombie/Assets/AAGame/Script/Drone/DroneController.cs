using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yurowm.GameCore;
using DG.Tweening;

public class DroneController : MonoBehaviour
{
    protected IState currentState;
    private DroneState droneState;
    [HideInInspector]
    public float timeInState = 0;
    public Transform posBullet;
    public EnemyBase target;
    private float droDetectRange = 30f;
    private List<Vector3> posList = new List<Vector3>();
    public float liveTime;
    private bool isMovingToDown;

    private void OnEnable()
    {
        GameEvent.OnPlayerWin.RemoveListener(OnPlayerWin);
        GameEvent.OnPlayerWin.AddListener(OnPlayerWin);
    }

    private void Start()
    {
        ChangeState(new DroSpawnState(this));
        GameEvent.OnPlayerLose.AddListener(() => ChangeState(new DroDieState(this)));
        GameEvent.OnRevive.AddListener(() => ChangeState(new DroFindingState(this)));
    }

    private void Update()
    {
        currentState.UpdateState();
        if(GameManager.Instance.gameState == GameState.Playing)
        {
            liveTime += Time.deltaTime;
            if(liveTime > 36000)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        currentState.ExitState();
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
        this.gameObject.name = "Drone---" + droneState.ToString();
    }


    public void OnPlayerWin()
    {
        ChangeState(new DroFreeState(this));
    }


    /// <summary>
    /// Drone move random when flying
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRandom()
    {
        while (true)
        {
            if (this.droneState == DroneState.Die)
            {
                yield break;
            }
            yield return new WaitForSeconds(Random.Range(4.1f, 6.1f));
            yield return new WaitUntil(() => droneState != DroneState.Attack);
            this.transform.DOLocalMove(posList.GetRandom(), 2f);
        }
    }

    public void Spawn()
    {
        posList.Clear();
        droneState = DroneState.Spawn;
        transform.parent = GameManager.Instance.trainManager.transform;
        transform.localPosition = new Vector3(8, 15, 0);
        transform.localScale = Vector3.one;
        if(GlobalData.gameMode == GameMode.CollectFuel)
        {
            Vector3 pos1 = new Vector3(-9, 15,0);
            posList.Add(pos1);
            Vector3 pos2 = new Vector3(9.5f,13,0);
            posList.Add(pos2);
            Vector3 pos3 = new Vector3(-4,20,0);
            posList.Add(pos3);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            Vector3 pos1 = new Vector3(transform.localPosition.x + 5, transform.localPosition.y, transform.localPosition.z);
            posList.Add(pos1);
            Vector3 pos2 = new Vector3(transform.localPosition.x - 13, transform.localPosition.y - 2f, transform.localPosition.z);
            posList.Add(pos2);
            Vector3 pos3 = new Vector3(transform.localPosition.x + 3, transform.localPosition.y + 5, transform.localPosition.z);
            posList.Add(pos3);
        }

        transform.localPosition = new Vector3(8, 25, 0);
        this.transform.DOLocalMove(posList.GetRandom(), 2f);
        StartCoroutine(MoveRandom());

        //if (GlobalData.gameMode == GameMode.CollectFuel)
        //{
        //    transform.localScale = new Vector3(-1,1,1);
        //    posList[0] = new Vector3(posList[0].x - 15f, posList[0].y, posList[0].z);
        //    posList[1] = new Vector3(posList[1].x - 15f, posList[1].y, posList[1].z);
        //    posList[2] = new Vector3(posList[2].x - 15f, posList[2].y, posList[2].z);
        //    transform.localPosition = new Vector3(8-15, 25, 0);
        //}
    }

    public void SpawnPlane()
    {
        
    }
    public void Free()
    {
        droneState = DroneState.Free;
    }

    /// <summary>
    /// Enter finding state
    /// </summary>
    public void FindingZombie()
    {
        droneState = DroneState.Finding;
        StartCoroutine(Finding());
    }

    /// <summary>
    /// Drone Finding zombie by sec
    /// </summary>
    /// <returns></returns>
    IEnumerator Finding()
    {
        target = null;
        if (target != null && target.enemyState != EnemyState.Die)
        {
            ChangeState(new DroAttackState(this));
            yield break;
        }

        while (target == null || target.enemyState == EnemyState.Die || target.enemyState == EnemyState.Spawn)
        {
            if (GameManager.Instance.enemiesCurrentAmount > 0)
            {
                foreach (EnemyBase enemy in GameManager.Instance.listEnemy)
                {
                    float dis = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (dis < droDetectRange && enemy.enemyState != EnemyState.Die && enemy.enemyState != EnemyState.Spawn && enemy.transform.position.y<8f)
                    {
                        this.target = enemy;
                        if (dis < (droDetectRange - 20))
                        {
                            this.target = null;
                            continue;
                        }
                        ChangeState(new DroAttackState(this));
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Enter attack state,drone shooting zombie
    /// </summary>
    public void Attack()
    {
        droneState = DroneState.Attack;
        isMovingToDown = false;
    }

    public void UpdateAttack()
    {
        timeInState += Time.deltaTime;
        if(target != null && target.enemyState != EnemyState.Die)
        {
            if(timeInState < 0.2f)
            {
                return;
            }
            timeInState = 0;
            
            BulletBot bullet = ContentPoolable.Emit(ItemID.bullet_bot_1) as BulletBot;
            bullet.transform.position = this.posBullet.position;
            Vector3 dir;
            if (target.typeEnemy == TypeEnemy.Boss_World)
            {
                dir = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z) - this.posBullet.position;
            }
            else
            {
                if (GlobalData.gameMode == GameMode.CollectFuel)
                {
                    dir = new Vector3(target.transform.position.x + 4f, target.transform.position.y + 2f, target.transform.position.z) - this.posBullet.position;
                }
                else
                {
                    if(target.transform.position.y > 4f)
                    {
                        dir = new Vector3(target.transform.position.x, target.transform.position.y+3f, target.transform.position.z) - this.posBullet.position;
                    }
                    else
                    {
                        dir = target.transform.position - this.posBullet.position;
                    }
                }
            }



            bullet.transform.right = dir;
            bullet.AddForce();

            float dis = Vector3.Distance(this.transform.position,target.transform.position);
            if (dis <= 16f && !isMovingToDown)
            {
                this.transform.DOLocalMove(posList[1], 2f);
                isMovingToDown = true;
            }
        }

        if (target == null || target.enemyState == EnemyState.Die)
        {
            ChangeState(new DroFindingState(this));
        }
        
    }

    public void ExitAttack()
    {
        isMovingToDown = false;
    }

    public void Die()
    {
        droneState = DroneState.Die;
    }

    public void ExitDie()
    {
        StartCoroutine(MoveRandom());
    }
}

public enum DroneState
{
    Spawn,
    Free,
    Finding,
    Attack,
    Die,
}
