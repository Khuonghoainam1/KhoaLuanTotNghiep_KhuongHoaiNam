using AA_Game;
using UnityEngine;

public class BackGroundItem : Item
{
    public Transform EndPoint;

    [Header("Floating manager")]
    internal float MoveNear = 0f;
    internal float MoveMid = 0f;
    internal float MoveFar = 0f;

    //Background Move Speed
    [SerializeField]
    private float _movingNearSpeed;
    [SerializeField]
    private float _movingMidSpeed;
    [SerializeField]
    private float _movingFarSpeed;

    TrainManager _trainManager;

    private void Start()
    {
        _trainManager = GameManager.Instance.trainManager;
    }

    private void Update()
    {
        MovingBackGround();
        if (/*User.Instance.GameMode*/ GlobalData.gameMode == GameMode.CollectFuel)
        {
            if (this.transform.position.x < GameManager.Instance.cam.transform.position.x - 60f)
            {
                Destroy(gameObject);
            }

        }
        else 
        {
            if (this.transform.position.x > GameManager.Instance.cam.transform.position.x + 60f)
            {
                Destroy(gameObject);
            }
        }
         

        void MovingBackGround()
        {
            if (_trainManager != null && _trainManager.IsMoving == true)
            {
                if (Time.timeScale != 0)
                {
                    MoveNear = _movingNearSpeed * Time.fixedDeltaTime;
                    MoveMid = _movingMidSpeed * Time.fixedDeltaTime;
                    MoveFar = _movingMidSpeed * Time.fixedDeltaTime;
                }
                else if (_trainManager != null && _trainManager.IsMoving == false)
                {
                    MoveNear = 0 * Time.fixedDeltaTime;
                    MoveMid = 0 * Time.fixedDeltaTime;
                    MoveFar = 0 * Time.fixedDeltaTime;
                }
            }
        }
    }
}
