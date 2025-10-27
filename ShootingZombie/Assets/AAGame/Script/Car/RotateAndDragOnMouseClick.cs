using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RotateAndDragOnMouseClick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler
{
    [SerializeField]
    private Camera _uiCamera;
    private bool isDragging = false;
    private bool isAutoAim = true;
    private Vector3 offset;
    internal Quaternion RotationGun;
    internal Vector3 PositionTarget;
    public RectTransform GunPointer;
    private TrainManager _trainManager;
    private bool isTrainMoving;

    public void SetData(TrainManager trainManager)
    {
        offset = transform.position;
        _trainManager = trainManager;

        GameEvent.OnShootingAuto.RemoveListener(OnShootingAuto);
        GameEvent.OffShootingAuto.RemoveListener(OffShootingAuto);
        GameEvent.OnShootingAuto.AddListener(OnShootingAuto);
        GameEvent.OffShootingAuto.AddListener(OffShootingAuto);

        GunPointer.transform.position = new Vector3(_trainManager.transform.position.x+10,_trainManager.transform.position.y,_trainManager.transform.position.z);
    }

    void Update()
    {
        if (_trainManager != null)
        {
            isTrainMoving = _trainManager.IsMoving;
            GunPointer.gameObject.SetActive(isTrainMoving);
            if (isTrainMoving)
            {
                Vector3 targetPosition = GunPointer.position;
                targetPosition.z = _trainManager.GunPosition.transform.position.z;
                Vector3 direction = targetPosition - _trainManager.GunPosition.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                RotationGun = Quaternion.AngleAxis(angle, Vector3.forward);
            }


            //auto rotate to enemy
            if (isAutoAim == true && GameManager.Instance.isAutoPlay==true)   //neu che do auto
            {
                if(_trainManager.target != null && _trainManager.target.enemyState != EnemyState.Die && _trainManager.target.transform.position.y <= 8f)
                {
                    if (GunPointer.gameObject.activeSelf == true)
                    {
                        if (_trainManager.target.transform.position.y > 1f)
                        {
                            GunPointer.transform.position = new Vector3(_trainManager.target.transform.position.x, _trainManager.target.transform.position.y + 3f, _trainManager.target.transform.position.z);
                            _trainManager.playerGun.posAimTarget.position = GunPointer.transform.position;
                        }
                        else
                        {
                            if (GlobalData.gameMode == GameMode.CollectFuel)
                            {
                                GunPointer.transform.position = new Vector3(_trainManager.target.transform.position.x, _trainManager.target.transform.position.y + 2f, _trainManager.target.transform.position.z);
                                _trainManager.playerGun.posAimTarget.position = GunPointer.transform.position;
                            }
                            else
                            {
                                GunPointer.transform.position = new Vector3(_trainManager.target.transform.position.x, _trainManager.target.transform.position.y + 1f, _trainManager.target.transform.position.z);
                                _trainManager.playerGun.posAimTarget.position = GunPointer.transform.position;
                            }
                        }
                    }
                }
            }
        } 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_trainManager != null && _trainManager.IsMoving == true && GameManager.Instance.gameState == GameState.Playing)
        {
            isDragging = true;
            isAutoAim = false;
            GunPointer.gameObject.SetActive(true);
            _trainManager.StartShooting();
            offset = GunPointer.transform.position - GetMouseWorldPosition();
            if (!GameManager.Instance.isBlockPointer)
            {
                GunPointer.position = Vector3.Lerp(GunPointer.position, GetMouseWorldPosition() + new Vector3(0, 0, offset.z), 50);
                _trainManager.playerGun.posAimTarget.position = GetMouseWorldPosition();
            }
        }

        if(User.Instance[ItemID.TutPlay] == 0)
        {
            GameManager.Instance.TutGamePlay.gameObject.SetActive(false);
            GameManager.Instance.isTutPlayAtive = false;
            GameManager.Instance.EndTut();
            User.Instance[ItemID.TutPlay] = 1;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_trainManager != null && _trainManager.IsMoving == true && GameManager.Instance.gameState == GameState.Playing)
        {
            if (isDragging)
            {
                if (!GameManager.Instance.isBlockPointer)
                {
                    GunPointer.position = Vector3.Lerp(GunPointer.position, GetMouseWorldPosition() + new Vector3(0, 0, offset.z), Time.deltaTime * 50);
                    PositionTarget = Vector3.Lerp(GunPointer.position, GetMouseWorldPosition() + new Vector3(0, 0, offset.z), Time.deltaTime * 50);
                    _trainManager.playerGun.posAimTarget.position = GetMouseWorldPosition();
                }
            }
        }
    }
    public void  OnPointerExit(PointerEventData eventData)
    {
        if (_trainManager != null && _trainManager.IsMoving == true && GameManager.Instance.gameState == GameState.Playing)
        {
            //if (GunPointer.gameObject.activeSelf == true)
            //{
            //    GunPointer.GetComponent<Animator>().Play("target_aim_release");
            //}
            isDragging = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_trainManager != null && _trainManager.IsMoving == true && GameManager.Instance.gameState == GameState.Playing)
        {
            //if(GunPointer.gameObject.activeSelf == true)
            //{
            //    GunPointer.GetComponent<Animator>().Play("target_aim_hold");
            //}
            isDragging = true;
        }
    }
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _uiCamera.transform.position.z - _uiCamera.nearClipPlane;
        return _uiCamera.ScreenToWorldPoint(mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_trainManager != null && _trainManager.IsMoving == true)
        {
            isDragging = false;
            isAutoAim = true;
            //if (GunPointer.gameObject.activeSelf == true)
            //{
            //    GunPointer.GetComponent<Animator>().Play("target_aim_release");
            //}
        }
    }


    public void OnShootingAuto()
    {
        if (_trainManager != null && _trainManager.IsMoving == true)
        {
            GunPointer.gameObject.SetActive(true);
            _trainManager.StartShooting();
            offset = GunPointer.transform.position - GetMouseWorldPosition();
            GunPointer.position = Vector3.Lerp(GunPointer.position, GetMouseWorldPosition() + new Vector3(0, 0, offset.z), 50);
            _trainManager.playerGun.posAimTarget.position = GunPointer.transform.position;
        }
    }

    public void OffShootingAuto()
    {

    }
}
