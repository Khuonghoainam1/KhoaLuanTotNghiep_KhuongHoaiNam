using AA_Game;
using UnityEngine;
using DG.Tweening;

public class RepairerControl : Item
{
    protected IState currentState;
    [HideInInspector]
    public float timeInstate;
    [HideInInspector]
    public Transform posFix;
    public CharacterAnim anim;
    public RepairerState repairerState;
    public NameRepairer repairerName;
    public Vector3 posWaving;
    public GameObject effectFix;

    private void OnEnable()
    {
        //GameEvent.OnStartGame.RemoveListener(OnStartGame);
        //GameEvent.OnStartGame.AddListener(OnStartGame);

        GameEvent.OnMoveToPlay.RemoveListener(OnStartGame);
        GameEvent.OnMoveToPlay.AddListener(OnStartGame);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(IState newState)
    {
        timeInstate = 0;
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    public void RepairerMovingToFix()
    {
        repairerState = RepairerState.MovingTofix;
        effectFix.SetActive(false);
        anim.PlayAnim(AnimID.run, true, 1, false);
        transform.DOMove(posFix.position, 5f).OnComplete(() =>
         {
             ChangeState(new FixingState(this));
             //if (repairerName != NameRepairer.RepairerGreen)
             //{
             //    ChangeState(new FixingState(this));
             //}
             //else
             //{
             //    ChangeState(new JumpToPosFix(this));
             //}
         });
    }



    public void Fix()
    {
        repairerState = RepairerState.Fixing;
        anim.PlayAnim(AnimID.fixing, true, 1, false);
        effectFix.SetActive(true);
    }

    public void GetOffTheCar()
    {
        repairerState = RepairerState.GetOff;
        anim.PlayAnim(AnimID.run, true, 2, false);
        transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
    }

    public void Waving()
    {
        repairerState = RepairerState.Waving;
        anim.PlayAnim(AnimID.waving, true, 1, false);
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    public void OnStartGame()
    {
        ChangeState(new GetOffCarSate(this));
    }
}

public enum RepairerState
{
    MovingTofix,
    Fixing,
    GetOff,
    Waving,
}

public enum NameRepairer
{
    RepairerBlue,
    RepairerRed,
    RepairerGreen,
}
